using GymPro.Application.Abstractions;
using GymPro.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GymPro.Persistence;

public static class DependencyInjection
{
    /// <summary>
    /// Registers EF Core and the repositories. SQL Server is the production
    /// database; the connection string name defaults to "GymPro".
    /// </summary>
    public static IServiceCollection AddPersistence(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<GymDbContext>(options =>
            options.UseSqlServer(connectionString, sql => sql.EnableRetryOnFailure(3)));

        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        services.AddScoped<IMembershipWriteRepository, MembershipWriteRepository>();
        services.AddScoped<IMembershipReadRepository, MembershipReadRepository>();
        services.AddScoped<IMembershipAggregateLoader, MembershipAggregateLoader>();
        services.AddScoped<IUserWriteRepository, UserRepository>();
        services.AddScoped<IUserReadRepository>(sp => (UserRepository)sp.GetRequiredService<IUserWriteRepository>());

        services.AddScoped<ICheckInWriteRepository, CheckInWriteRepository>();
        services.AddScoped<ICheckInReadRepository, CheckInReadRepository>();
        services.AddScoped<IMeasurementWriteRepository, MeasurementWriteRepository>();
        services.AddScoped<IMeasurementReadRepository, MeasurementReadRepository>();

        return services;
    }
}
