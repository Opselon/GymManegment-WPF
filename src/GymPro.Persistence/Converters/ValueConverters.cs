using GymPro.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GymPro.Persistence.Converters;

/// <summary>
/// Persists a <see cref="JalaliDate"/> as the equivalent Gregorian DateOnly.
/// Storing the absolute date (rather than the original free-text Jalali string)
/// is what makes ordering and range queries correct.
/// </summary>
public sealed class JalaliDateConverter : ValueConverter<JalaliDate, DateOnly>
{
    public JalaliDateConverter()
        : base(v => v.Gregorian, v => JalaliDate.FromGregorian(v))
    {
    }
}

/// <summary>
/// Persists a <see cref="Money"/> as a whole-rial decimal. Using a numeric
/// column keeps SUM/ORDER BY in the database instead of in memory.
/// </summary>
public sealed class MoneyConverter : ValueConverter<Money, decimal>
{
    public MoneyConverter()
        : base(v => v.RialAmount, v => Money.FromRial(v))
    {
    }
}
