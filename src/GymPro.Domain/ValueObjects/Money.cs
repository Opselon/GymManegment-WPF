using System.Globalization;

namespace GymPro.Domain.ValueObjects;

/// <summary>
/// A monetary amount in Iranian Rial.
/// </summary>
/// <remarks>
/// <para>
/// The original application stored money in <c>long</c>/<c>decimal</c> columns
/// with no unit convention and formatted it with the current culture, which
/// produced "۱۲٫۵" style output that could not be re-parsed. This type makes
/// the unit explicit and keeps the invariant: non-negative for balances that
/// cannot go negative, exact to the rial (Iran has no circulating sub-rial
/// unit).
/// </para>
/// <para>
/// Deliberately a <c>readonly record struct</c>: money is compared by value,
/// copies are cheap, and there is no identity to confuse.
/// </para>
/// </remarks>
public readonly record struct Money : IComparable<Money>
{
    private Money(decimal rialAmount) => RialAmount = decimal.Round(rialAmount, 0, MidpointRounding.ToEven);

    /// <summary>Amount in Rial. Always rounded to whole rial.</summary>
    public decimal RialAmount { get; }

    public static Money Zero => new(0m);

    public static Money FromRial(decimal rial) => new(rial);

    /// <summary>Convenience for the "Toman" unit commonly used at the counter (1 Toman = 10 Rial).</summary>
    public static Money FromToman(decimal toman) => new(toman * 10m);

    public decimal TomanAmount => RialAmount / 10m;

    public bool IsZero => RialAmount == 0m;

    public bool IsNegative => RialAmount < 0m;

    public Money Add(Money other) => new(RialAmount + other.RialAmount);

    public Money Subtract(Money other) => new(RialAmount - other.RialAmount);

    /// <summary>Multiplies by a whole number of units (months, sessions, quantity).</summary>
    public Money MultiplyBy(int quantity) => new(RialAmount * quantity);

    public static Money operator +(Money left, Money right) => left.Add(right);

    public static Money operator -(Money left, Money right) => left.Subtract(right);

    public static Money operator *(Money money, int quantity) => money.MultiplyBy(quantity);

    public int CompareTo(Money other) => RialAmount.CompareTo(other.RialAmount);

    public static bool operator <(Money left, Money right) => left.CompareTo(right) < 0;

    public static bool operator >(Money left, Money right) => left.CompareTo(right) > 0;

    public static bool operator <=(Money left, Money right) => left.CompareTo(right) <= 0;

    public static bool operator >=(Money left, Money right) => left.CompareTo(right) >= 0;

    /// <summary>
    /// Renders the invariant "12345 Rial" form. Presentation-level Persian
    /// formatting belongs to the UI layer, not here.
    /// </summary>
    public override string ToString() =>
        string.Create(CultureInfo.InvariantCulture, $"{RialAmount:N0} IRR");
}
