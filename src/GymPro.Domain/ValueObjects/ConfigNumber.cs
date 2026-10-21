using System.Globalization;

namespace GymPro.Domain.ValueObjects;

/// <summary>
/// A setting that holds a number (monthly fee, leap-year thresholds) with the
/// unit it was entered in, so configuration values survive culture changes.
/// </summary>
public readonly record struct ConfigNumber
{
    private ConfigNumber(decimal value, string unit)
    {
        Value = value;
        Unit = unit;
    }

    public decimal Value { get; }
    public string Unit { get; }

    public static ConfigNumber Plain(decimal value) => new(value, string.Empty);
    public static ConfigNumber WithUnit(decimal value, string unit) => new(value, unit);

    public override string ToString()
        => Unit.Length == 0
            ? Value.ToString(CultureInfo.InvariantCulture)
            : Value.ToString(CultureInfo.InvariantCulture) + " " + Unit;
}
