using GymPro.Domain.Memberships;
using GymPro.Domain.ValueObjects;
using GymPro.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymPro.Persistence.Configurations;

/// <summary>
/// Table mapping for the membership aggregate.
/// </summary>
public sealed class MembershipConfiguration : IEntityTypeConfiguration<Membership>
{
    /// <summary>
    /// SQL Server's unrestricted binary column type. Global query filtering and
    /// provider selection happen in <see cref="GymDbContext"/>; here the type is
    /// only attached where the provider understands it.
    /// </summary>
    private const string SqlServerBinaryMax = "varbinary(max)";

    public void Configure(EntityTypeBuilder<Membership> builder)
    {
        builder.ToTable("Memberships");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.FullName).IsRequired().HasMaxLength(200);
        builder.Property(m => m.NationalCode).HasMaxLength(10);
        builder.Property(m => m.PhoneNumber).HasMaxLength(20);
        builder.Property(m => m.DurationMonths).IsRequired();

        builder.Property(m => m.Gender).HasConversion<byte>();
        builder.Property(m => m.Status).HasConversion<byte>();
        builder.Property(m => m.MonthlyFee).HasConversion<MoneyConverter>().HasPrecision(18, 0);
        builder.Property(m => m.StartDate).HasConversion<JalaliDateConverter>();
        builder.Property(m => m.RegisteredOn).HasConversion<JalaliDateConverter>();
        // The photo column is a blob. SQL Server wants varbinary(max); see
        // GymDbContext, which strips provider-specific column types when the
        // model is built against a different engine.
        builder.Property(m => m.Photo).HasColumnType(SqlServerBinaryMax);

        // A national code identifies a member: duplicates are a data-integrity
        // bug, so the database enforces it rather than a UI check.
        // The filtered index needs each provider's own quoting. SQLite is used by
        // the test harness, SQL Server in production.
        builder.HasIndex(m => m.NationalCode).IsUnique();

        builder.HasMany(m => m.Payments)
            .WithOne()
            .HasForeignKey("MembershipId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(m => m.Payments).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Ignore(m => m.DomainEvents);
        builder.Ignore(m => m.OutstandingBalance);
        builder.Ignore(m => m.StartOfCoverage);
        builder.Ignore(m => m.EndOfCoverage);
    }
}
