using GymPro.Domain.Attendance;
using GymPro.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymPro.Persistence.Configurations;

public sealed class CheckInConfiguration : IEntityTypeConfiguration<CheckIn>
{
    public void Configure(EntityTypeBuilder<CheckIn> builder)
    {
        builder.ToTable("CheckIns");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.CheckedInOn).HasConversion<JalaliDateConverter>();
        builder.HasIndex(c => new { c.MembershipId, c.CheckedInOn });
    }
}

public sealed class BodyMeasurementConfiguration : IEntityTypeConfiguration<BodyMeasurement>
{
    public void Configure(EntityTypeBuilder<BodyMeasurement> builder)
    {
        builder.ToTable("BodyMeasurements");
        builder.HasKey(b => b.Id);

        // The aggregate's constructor takes the whole measurement in one call,
        // so bind it explicitly rather than relying on the convention, which
        // cannot bind nullable value-type parameters to properties.
        builder.Property(b => b.ChestCm);
        builder.Property(b => b.ArmCm);
        builder.Property(b => b.WaistCm);
        builder.Property(b => b.HeightCm);
        builder.Property(b => b.WeightKg);
        builder.Property(b => b.MeasuredOn).HasConversion<JalaliDateConverter>();

        builder.HasIndex(b => new { b.MembershipId, b.MeasuredOn });
    }
}
