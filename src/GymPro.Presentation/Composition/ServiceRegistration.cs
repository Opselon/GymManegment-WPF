using GymPro.Application;
using GymPro.Infrastructure;
using GymPro.Persistence;
using GymPro.Presentation.Navigation;
using GymPro.Presentation.ViewModels;
using GymPro.Presentation.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GymPro.Presentation.Composition;

/// <summary>
/// The composition root: the only place that knows about every layer at once.
/// </summary>
public static class ServiceRegistration
{
    public static IServiceCollection Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplication();
        services.AddInfrastructure(configuration);

        var connectionString = configuration.GetConnectionString("GymPro")
            ?? "Server=(localdb)\\MSSQLLocalDB;Database=GymPro;Trusted_Connection=True;TrustServerCertificate=True";
        services.AddPersistence(connectionString);

        // Views and view-models
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<ShellWindow>();
        services.AddTransient<LoginView>();
        services.AddTransient<LoginViewModel>();
        services.AddTransient<DashboardView>();
        services.AddTransient<DashboardViewModel>();
        services.AddTransient<MembersView>();
        services.AddTransient<MembersViewModel>();

        return services;
    }
}
