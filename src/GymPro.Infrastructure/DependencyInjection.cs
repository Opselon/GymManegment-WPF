using GymPro.Application.Abstractions;
using GymPro.Infrastructure.Backup;
using GymPro.Infrastructure.Qr;
using GymPro.Infrastructure.Reporting;
using GymPro.Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GymPro.Infrastructure;

/// <summary>
/// Composition root for the adapters. Everything registered here implements an
/// interface owned by <c>GymPro.Application</c>, so the layers above never see a
/// concrete infrastructure type.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers password hashing, QR generation, Excel reporting and backups.
    /// </summary>
    /// <param name="services">The container being built.</param>
    /// <param name="configuration">Configuration supplying the hashing and backup settings.</param>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        AddPasswordHashing(services, configuration);
        AddBackups(services, configuration);

        // The QR renderer owns native buffers and implements IDisposable, so it
        // is scoped to the container's lifetime rather than shared freely; the
        // Excel exporter holds no state between calls.
        services.AddSingleton<IQrCodeRenderer>(_ => new QrCodeRenderer());
        services.AddSingleton<IExcelExporter, ExcelExporter>();

        return services;
    }

    private static void AddPasswordHashing(IServiceCollection services, IConfiguration configuration)
    {
        var hashingSection = configuration.GetSection("PasswordHashing");
        services.Configure<PasswordHasherOptions>(options =>
        {
            options.SaltSize = hashingSection.GetValue("SaltSize", options.SaltSize);
            options.HashSize = hashingSection.GetValue("HashSize", options.HashSize);
            options.Iterations = hashingSection.GetValue("Iterations", options.Iterations);
        });

        services.AddSingleton<IPasswordHasher, Pbkdf2PasswordHasher>();
    }

    private static void AddBackups(IServiceCollection services, IConfiguration configuration)
    {
        // Backups are optional: a machine with no backup directory configured
        // should still run the application, just without them.
        var connectionString = configuration.GetConnectionString("GymPro");
        var backupDirectory = configuration.GetSection("Backup").GetValue<string>("Directory");

        if (string.IsNullOrWhiteSpace(connectionString) || string.IsNullOrWhiteSpace(backupDirectory))
        {
            return;
        }

        services.AddSingleton<IDatabaseBackupService>(sp =>
            new SqlServerBackupService(
                connectionString,
                // Anchored to the app directory, not the process working
                // directory: a shortcut, a scheduled task or a shell started
                // elsewhere must still write backups to the same place.
                Path.Combine(AppContext.BaseDirectory, backupDirectory),
                sp.GetRequiredService<ILogger<SqlServerBackupService>>()));
    }
}
