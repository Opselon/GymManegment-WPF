using System.Globalization;

namespace GymPro.Domain.ValueObjects;

/// <summary>
/// A calendar date in the Solar Hijri (Jalali) calendar, the civil calendar of
/// Iran and the one every membership card, receipt and report here uses.
/// </summary>
/// <remarks>
/// The original schema stored dates as free-text columns, which turned ordering
/// and expiry checks into string comparisons. This type keeps an absolute
/// Gregorian date for storage and comparison, and projects Jalali components
/// for display. Conversion delegates to the BCL PersianCalendar rather than a
/// hand-rolled leap-cycle, which is a classic source of off-by-one-day bugs.
/// </remarks>
public readonly record struct JalaliDate : IComparable<JalaliDate>
{
    private static readonly PersianCalendar Calendar = new();

    private const int MinYear = 1300;
    private const int MaxYear = 1500;

    private JalaliDate(int year, int month, int day, DateOnly gregorian)
    {
        Year = year;
        Month = month;
        Day = day;
        Gregorian = gregorian;
    }

    public int Year { get; }
    public int Month { get; }
    public int Day { get; }

    /// <summary>The equivalent Gregorian date, used for storage and comparison.</summary>
    public DateOnly Gregorian { get; }

    public static JalaliDate FromGregorian(DateOnly gregorian)
    {
        var dateTime = gregorian.ToDateTime(TimeOnly.MinValue);
        var year = Calendar.GetYear(dateTime);
        if (year is < MinYear or > MaxYear)
        {
            throw new ArgumentOutOfRangeException(
                nameof(gregorian),
                "Jalali year " + year + " is outside the supported range " + MinYear + "-" + MaxYear + ".");
        }

        return new JalaliDate(year, Calendar.GetMonth(dateTime), Calendar.GetDayOfMonth(dateTime), gregorian);
    }

    public static JalaliDate FromGregorian(DateTime dateTime)
        => FromGregorian(DateOnly.FromDateTime(dateTime));

    public static JalaliDate Today(TimeProvider timeProvider)
        => FromGregorian(DateOnly.FromDateTime(timeProvider.GetLocalNow().DateTime));

    public static bool TryCreate(int year, int month, int day, out JalaliDate result, out string? error)
    {
        result = default;
        error = null;

        if (year is < MinYear or > MaxYear)
        {
            error = "Year must be between " + MinYear + " and " + MaxYear + ".";
            return false;
        }

        if (month < 1 || month > 12)
        {
            error = "Month must be between 1 and 12.";
            return false;
        }

        var monthLength = DaysInMonth(year, month);
        if (day < 1 || day > monthLength)
        {
            error = "Day must be between 1 and " + monthLength + " for month " + month + " of " + year + ".";
            return false;
        }

        var gregorian = DateOnly.FromDateTime(Calendar.ToDateTime(year, month, day, 0, 0, 0, 0));
        result = new JalaliDate(year, month, day, gregorian);
        return true;
    }

    public static JalaliDate Create(int year, int month, int day)
    {
        if (!TryCreate(year, month, day, out var date, out var error))
        {
            throw new ArgumentOutOfRangeException(nameof(day), error);
        }

        return date;
    }

    /// <summary>Number of days in a Jalali month; the final month depends on the leap cycle.</summary>
    public static int DaysInMonth(int year, int month) => Calendar.GetDaysInMonth(year, month);

    /// <summary>True when the Jalali year is a leap year (its final month has 30 days).</summary>
    public static bool IsLeapYear(int year) => Calendar.IsLeapYear(year);

    public JalaliDate AddMonths(int months)
    {
        var totalMonths = (Year * 12) + (Month - 1) + months;
        var newYear = totalMonths / 12;
        var newMonth = (totalMonths % 12) + 1;
        var newDay = Math.Min(Day, DaysInMonth(newYear, newMonth));
        return Create(newYear, newMonth, newDay);
    }

    public JalaliDate AddDays(int days) => FromGregorian(Gregorian.AddDays(days));

    /// <summary>First day of the containing Jalali month.</summary>
    public JalaliDate StartOfMonth => Create(Year, Month, 1);

    /// <summary>Last day of the containing Jalali month.</summary>
    public JalaliDate EndOfMonth => Create(Year, Month, MonthLength);

    /// <summary>Number of days in the containing Jalali month.</summary>
    public int MonthLength => DaysInMonth(Year, Month);

    public JalaliDate AddYears(int years) => AddMonths(years * 12);

    /// <summary>Days elapsed from the other date to this one (negative when earlier).</summary>
    public int DaysSince(JalaliDate other) => Gregorian.DayNumber - other.Gregorian.DayNumber;

    public int CompareTo(JalaliDate other) => Gregorian.CompareTo(other.Gregorian);

    public static bool operator <(JalaliDate left, JalaliDate right) => left.CompareTo(right) < 0;
    public static bool operator >(JalaliDate left, JalaliDate right) => left.CompareTo(right) > 0;
    public static bool operator <=(JalaliDate left, JalaliDate right) => left.CompareTo(right) <= 0;
    public static bool operator >=(JalaliDate left, JalaliDate right) => left.CompareTo(right) >= 0;

    /// <summary>Zero-padded year-month-day in Latin digits; sorts chronologically as text.</summary>
    public override string ToString()
        => string.Format(CultureInfo.InvariantCulture, "{0:D4}-{1:D2}-{2:D2}", Year, Month, Day);

    /// <summary>Parses the zero-padded Jalali form produced by ToString.</summary>
    public static bool TryParse(string? text, out JalaliDate result)
    {
        result = default;
        if (string.IsNullOrWhiteSpace(text))
        {
            return false;
        }

        var parts = text.Trim().Split('-');
        if (parts.Length != 3)
        {
            return false;
        }

        if (!int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out var year)
            || !int.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out var month)
            || !int.TryParse(parts[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out var day))
        {
            return false;
        }

        return TryCreate(year, month, day, out result, out _);
    }
}
