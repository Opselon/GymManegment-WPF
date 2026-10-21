using System.Windows;
using GymPro.Presentation.Composition;
using GymPro.Presentation.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GymPro.Presentation;

/// <summary>
/// Application entry point. Builds the generic host, which owns configuration,
/// logging and the DI container, then shows the shell window.
/// </summary>
public partial class App : System.Windows.Application
{
    private IHost? _host;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
                ServiceRegistration.Register(services, context.Configuration))
            .Build();

        await _host.StartAsync();

        // The database must exist and have an account before the login screen
        // appears, otherwise a first run cannot be signed into at all.
        await DatabaseInitializer.InitializeAsync(_host.Services);

        var shell = _host.Services.GetRequiredService<ShellWindow>();
        MainWindow = shell;
        shell.Show();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        if (_host is not null)
        {
            await _host.StopAsync();
            _host.Dispose();
        }

        base.OnExit(e);
    }
}
