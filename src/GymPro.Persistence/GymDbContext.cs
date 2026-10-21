using GymPro.Domain.Attendance;
using GymPro.Domain.Memberships;
using GymPro.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace GymPro.Persistence;

/// <summary>
/// The EF Core unit of work. Replaces the EF6 EDMX model from the original
/// application; mappings live in this assembly (not as attributes on the
/// domain) so the domain stays persistence-ignorant.
/// </summary>
public sealed class GymDbContext(DbContextOptions<GymDbContext> options) : DbContext(options)
{
    public DbSet<Membership> Memberships => Set<Membership>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<User> Users => Set<User>();
    public DbSet<CheckIn> CheckIns => Set<CheckIn>();
    public DbSet<BodyMeasurement> BodyMeasurements => Set<BodyMeasurement>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GymDbContext).Assembly);
        RemoveProviderSpecificColumnTypes(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Column types such as <c>varbinary(max)</c> are SQL Server dialect. The
    /// mappings are shared with the SQLite test harness, so any type the current
    /// provider does not understand is dropped and the provider picks its own
    /// natural mapping for that CLR type.
    /// </summary>
    private void RemoveProviderSpecificColumnTypes(ModelBuilder modelBuilder)
    {
        if (Database.IsSqlServer())
        {
            return;
        }

        foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(t => t.GetProperties()))
        {
            if (property.GetColumnType() is not null)
            {
                property.SetColumnType(null);
            }
        }
    }
}
