using GymPro.Application.Abstractions;
using GymPro.Domain.Users;
using GymPro.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GymPro.Presentation.Composition;

/// <summary>
/// Prepares the store before the shell is shown: creates the database when it is
/// missing and seeds the built-in administrator on first run. Without this, a
/// fresh machine opens a login screen whose only account does not exist.
/// </summary>
internal static partial class DatabaseInitializer
{
    /// <summary>Built-in administrator credentials; the password must change on first sign-in.</summary>
    private const string AdminUserName = "admin";
    private const string AdminPassword = "admin";
    private const string AdminDisplayName = "مدیر سیستم";

    public static async Task InitializeAsync(IServiceProvider services, CancellationToken cancellationToken = default)
    {
        await using var scope = services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<GymDbContext>();
        var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DatabaseInitializer");

        await db.Database.EnsureCreatedAsync(cancellationToken);

        if (await db.Users.AnyAsync(u => u.Username == AdminUserName, cancellationToken))
        {
            return;
        }

        var admin = User.Register(
            AdminUserName,
            AdminDisplayName,
            hasher.Hash(AdminPassword),
            UserRole.Admin);

        db.Users.Add(admin);
        await db.SaveChangesAsync(cancellationToken);
        LogAdminSeeded(logger);
    }

    /// <summary>Source-generated log delegate, required by CA1848.</summary>
    [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "Seeded the built-in administrator account.")]
    private static partial void LogAdminSeeded(ILogger logger);
}
