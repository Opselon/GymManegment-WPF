using GymPro.Application.Abstractions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace GymPro.Infrastructure.Backup;

/// <summary>
/// Takes and prunes SQL Server backups.
/// </summary>
/// <remarks>
/// <para>
/// The original application had no backup story at all: a gym's entire member
/// ledger lived in one MDF file on one desktop. This adapter exists so the
/// schedule is explicit and the retention window is a number in configuration
/// rather than a habit.
/// </para>
/// <para>
/// Backups are issued with <c>BACKUP DATABASE … WITH COPY_ONLY</c> so they do
/// not break an existing differential backup chain. The file is written by the
/// SQL Server service account, not by this process, which is why the target
/// directory must be reachable by the server.
/// </para>
/// </remarks>
public sealed class SqlServerBackupService(
    string connectionString,
    string backupDirectory,
    ILogger<SqlServerBackupService> logger) : IDatabaseBackupService
{
    /// <inheritdoc />
    public string BackupDirectory => backupDirectory;

    /// <inheritdoc />
    public async Task<string> CreateBackupAsync(CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(backupDirectory);
        var targetDir = Path.GetFullPath(backupDirectory);
        Directory.CreateDirectory(targetDir);

        var builder = new SqlConnectionStringBuilder(connectionString);
        var database = builder.InitialCatalog;

        if (string.IsNullOrWhiteSpace(database))
        {
            throw new InvalidOperationException(
                "The connection string has no database name, so there is nothing to back up.");
        }

        // Timestamped to the second: two backups in the same minute must not
        // overwrite each other, or a failed restore has no fallback.
        var fileName = $"{database}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.bak";
        var fullPath = Path.Combine(targetDir, fileName);

        await using var connection = new SqlConnection(builder.ConnectionString);
        await connection.OpenAsync(cancellationToken);

        // The database name cannot be parameterised in a BACKUP statement, so it
        // is bracketed and the inner bracket pair escaped.
        var quotedDatabase = $"[{database.Replace("]", "]]", StringComparison.Ordinal)}]";

        await using var command = connection.CreateCommand();
        command.CommandText =
            $"BACKUP DATABASE {quotedDatabase} TO DISK = @target WITH COPY_ONLY, INIT, CHECKSUM";
        command.Parameters.AddWithValue("@target", fullPath);

        BackupLog.BackingUp(logger, database, fullPath);

        try
        {
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
        catch (SqlException ex)
        {
            // SQL Server writes the file itself, so the usual cause is a path it
            // cannot reach or a permission the service account lacks. Wrap it so
            // callers depend on the abstraction, not on SqlClient.
            throw new InvalidOperationException(
                $"SQL Server could not write {fullPath}. The directory must be on a path the "
                + "server's service account can write to, and for a remote server it must be a "
                + $"UNC path. Server said: {ex.Message}",
                ex);
        }

        return fullPath;
    }

    /// <inheritdoc />
    public IReadOnlyList<string> PruneBackups(int keepNewest)
    {
        if (keepNewest < 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(keepNewest), "Retention must keep at least one backup.");
        }

        if (!Directory.Exists(backupDirectory))
        {
            return [];
        }

        var stale = Directory.GetFiles(Path.GetFullPath(backupDirectory), "*.bak")
            .OrderByDescending(f => f, StringComparer.OrdinalIgnoreCase)
            .Skip(keepNewest)
            .ToList();

        var deleted = new List<string>(stale.Count);
        foreach (var file in stale)
        {
            try
            {
                File.Delete(file);
                deleted.Add(file);
            }
            catch (IOException ex)
            {
                // A locked or in-use file must not abort the whole prune: the
                // other candidates are still worth clearing.
                BackupLog.StaleBackupLocked(logger, file, ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                BackupLog.StaleBackupDenied(logger, file, ex);
            }
        }

        if (deleted.Count > 0)
        {
            BackupLog.Pruned(logger, deleted.Count);
        }

        return deleted;
    }
}

/// <summary>
/// Cached logging delegates. The analyzer requires these over
/// <c>logger.LogX(...)</c> calls because the message template is parsed once at
/// start-up rather than on every call.
/// </summary>
internal static class BackupLog
{
    private static readonly Action<ILogger, string, string, Exception?> BackingUpCore =
        LoggerMessage.Define<string, string>(
            LogLevel.Information,
            new EventId(2000, nameof(BackingUp)),
            "Backing up {Database} to {Path}.");

    private static readonly Action<ILogger, string, Exception?> StaleBackupLockedCore =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(2001, nameof(StaleBackupLocked)),
            "Could not delete stale backup {Path} because it is in use.");

    private static readonly Action<ILogger, string, Exception?> StaleBackupDeniedCore =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(2002, nameof(StaleBackupDenied)),
            "No permission to delete stale backup {Path}.");

    private static readonly Action<ILogger, int, Exception?> PrunedCore =
        LoggerMessage.Define<int>(
            LogLevel.Information,
            new EventId(2003, nameof(Pruned)),
            "Pruned {Count} stale backups.");

    public static void BackingUp(this ILogger logger, string database, string path)
        => BackingUpCore(logger, database, path, null);

    public static void StaleBackupLocked(this ILogger logger, string path, Exception ex)
        => StaleBackupLockedCore(logger, path, ex);

    public static void StaleBackupDenied(this ILogger logger, string path, Exception ex)
        => StaleBackupDeniedCore(logger, path, ex);

    public static void Pruned(this ILogger logger, int count)
        => PrunedCore(logger, count, null);
}
