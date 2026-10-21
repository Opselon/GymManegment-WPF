namespace GymPro.Application.Abstractions;

/// <summary>
/// Takes and prunes database backups.
/// </summary>
/// <remarks>
/// This lives in the Application layer rather than beside its implementation so
/// the backup screen depends on an abstraction, not on SQL Server. A deployment
/// that stores its data elsewhere can provide a different adapter without the UI
/// changing.
/// </remarks>
public interface IDatabaseBackupService
{
    /// <summary>Writes one backup and returns the full path of the file.</summary>
    Task<string> CreateBackupAsync(CancellationToken cancellationToken = default);

    /// <summary>Deletes all but the newest backups, returning the paths removed.</summary>
    IReadOnlyList<string> PruneBackups(int keepNewest);

    /// <summary>
    /// The directory backups are written to, so a screen can list what exists
    /// without duplicating the configuration lookup.
    /// </summary>
    string BackupDirectory { get; }
}
