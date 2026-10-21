using GymPro.Domain.ValueObjects;
using Shouldly;
using Xunit;

namespace GymPro.Domain.Tests;

public sealed class MoneyTests
{
    [Fact]
    public void Amounts_compare_by_value()
    {
        Money.FromRial(1000).ShouldBe(Money.FromRial(1000));
    }

    [Fact]
    public void Rial_rounds_to_the_nearest_whole_unit()
    {
        Money.FromRial(1000.6m).RialAmount.ShouldBe(1001m);
        Money.FromRial(1000.4m).RialAmount.ShouldBe(1000m);
    }

    [Fact]
    public void Toman_converts_at_ten_rial()
    {
        Money.FromToman(50).RialAmount.ShouldBe(500m);
        Money.FromRial(500).TomanAmount.ShouldBe(50m);
    }

    [Fact]
    public void Addition_and_subtraction_work()
    {
        (Money.FromRial(300) + Money.FromRial(200)).RialAmount.ShouldBe(500m);
        (Money.FromRial(300) - Money.FromRial(200)).RialAmount.ShouldBe(100m);
    }

    [Fact]
    public void Multiplication_scales_by_quantity()
    {
        (Money.FromRial(250) * 3).RialAmount.ShouldBe(750m);
    }

    [Fact]
    public void Ordering_operators_work()
    {
        (Money.FromRial(100) < Money.FromRial(200)).ShouldBeTrue();
        (Money.FromRial(300) > Money.FromRial(200)).ShouldBeTrue();
    }

    [Fact]
    public void Zero_is_detected()
    {
        Money.Zero.IsZero.ShouldBeTrue();
        Money.FromRial(1).IsZero.ShouldBeFalse();
    }
}
