using GymPro.Domain.ValueObjects;
using Shouldly;
using Xunit;

namespace GymPro.Domain.Tests;

public sealed class JalaliDateTests
{
    [Theory]
    [InlineData(2024, 3, 20, 1403, 1, 1)]   // Nowruz 1403
    [InlineData(2025, 3, 21, 1404, 1, 1)]   // Nowruz 1404
    [InlineData(2021, 3, 21, 1400, 1, 1)]   // Nowruz 1400
    [InlineData(2016, 3, 20, 1395, 1, 1)]   // Nowruz 1395
    [InlineData(2016, 9, 22, 1395, 7, 1)]   // 1 Mehr 1395
    public void FromGregorian_maps_known_Nowruz_dates(int gy, int gm, int gd, int jy, int jm, int jd)
    {
        var jalali = JalaliDate.FromGregorian(new DateOnly(gy, gm, gd));

        jalali.Year.ShouldBe(jy);
        jalali.Month.ShouldBe(jm);
        jalali.Day.ShouldBe(jd);
    }

    [Theory]
    [InlineData(1403, 1, 1, 2024, 3, 20)]
    [InlineData(1404, 1, 1, 2025, 3, 21)]
    [InlineData(1400, 1, 1, 2021, 3, 21)]
    [InlineData(1395, 7, 1, 2016, 9, 22)]
    public void Create_maps_back_to_the_expected_Gregorian_date(int jy, int jm, int jd, int gy, int gm, int gd)
    {
        JalaliDate.Create(jy, jm, jd).Gregorian.ShouldBe(new DateOnly(gy, gm, gd));
    }

    [Fact]
    public void The_first_six_months_have_31_days()
    {
        for (var month = 1; month <= 6; month++)
        {
            JalaliDate.DaysInMonth(1403, month).ShouldBe(31);
        }
    }

    [Fact]
    public void Months_seven_to_eleven_have_30_days()
    {
        for (var month = 7; month <= 11; month++)
        {
            JalaliDate.DaysInMonth(1403, month).ShouldBe(30);
        }
    }

    [Theory]
    [InlineData(1403, 30)]  // 1403 is a leap year
    [InlineData(1402, 29)]  // 1402 is not
    public void The_last_month_length_follows_the_leap_cycle(int year, int expectedDays)
    {
        JalaliDate.DaysInMonth(year, 12).ShouldBe(expectedDays);
    }

    [Fact]
    public void An_invalid_day_is_rejected_rather_than_silently_shifted()
    {
        // 1402 is not a leap year, so the 30th of Esfand does not exist.
        JalaliDate.TryCreate(1402, 12, 30, out _, out var realError).ShouldBeFalse();
        realError.ShouldNotBeNull();
    }

    [Fact]
    public void Month_13_is_rejected()
    {
        JalaliDate.TryCreate(1403, 13, 1, out _, out var error).ShouldBeFalse();
        error.ShouldNotBeNull();
    }

    [Fact]
    public void ToString_round_trips_through_TryParse()
    {
        var original = JalaliDate.Create(1403, 7, 15);

        JalaliDate.TryParse(original.ToString(), out var parsed).ShouldBeTrue();

        parsed.ShouldBe(original);
    }

    [Fact]
    public void ToString_sorts_chronologically_as_text()
    {
        var earlier = JalaliDate.Create(1403, 6, 31).ToString();
        var later = JalaliDate.Create(1403, 7, 1).ToString();

        string.CompareOrdinal(earlier, later).ShouldBeLessThan(0);
    }

    [Fact]
    public void AddMonths_clamps_to_the_end_of_a_shorter_month()
    {
        // 31 Shahrivar + 1 month -> Mehr only has 30 days.
        var result = JalaliDate.Create(1403, 6, 31).AddMonths(1);

        result.Month.ShouldBe(7);
        result.Day.ShouldBe(30);
    }

    [Fact]
    public void AddYears_handles_the_year_boundary()
    {
        JalaliDate.Create(1403, 12, 29).AddYears(1).Year.ShouldBe(1404);
    }

    [Fact]
    public void Dates_compare_chronologically()
    {
        var a = JalaliDate.Create(1403, 1, 1);
        var b = JalaliDate.Create(1403, 12, 29);

        (a < b).ShouldBeTrue();
        (b > a).ShouldBeTrue();
        var same = JalaliDate.Create(1403, 1, 1);
        (a <= same).ShouldBeTrue();
    }

    [Fact]
    public void StartOfMonth_and_EndOfMonth_frame_the_month()
    {
        var date = JalaliDate.Create(1403, 7, 15);

        date.StartOfMonth.Day.ShouldBe(1);
        date.EndOfMonth.Day.ShouldBe(30);
    }

    [Fact]
    public void Every_supported_date_round_trips()
    {
        var start = new DateOnly(1921, 3, 21);  // 1300-01-01
        for (var i = 0; i < 20_000; i++)
        {
            var gregorian = start.AddDays(i);
            var jalali = JalaliDate.FromGregorian(gregorian);
            jalali.Gregorian.ShouldBe(gregorian);
        }
    }
}
