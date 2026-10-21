using GymPro.Domain.Memberships;
using GymPro.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymPro.Persistence.Configurations;

public sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Amount).HasConversion<MoneyConverter>().HasPrecision(18, 0);
        builder.Property(p => p.PaidOn).HasConversion<Converters.JalaliDateConverter>();
    }
}
