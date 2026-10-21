using GymPro.Application.Memberships.Commands.RegisterMember;
using GymPro.Domain.Memberships;
using Shouldly;
using Xunit;

namespace GymPro.Application.Tests;

public sealed class RegisterMemberValidatorTests
{
    private static readonly RegisterMemberCommandValidator Validator = new();

    private static RegisterMemberCommand Valid() => new(
        "Yara Kiani", (byte)Gender.Female, "1234567890", null,
        StartYear: 1403, StartMonth: 1, StartDay: 1, DurationMonths: 3, MonthlyFeeRial: 1000m);

    [Fact]
    public void A_valid_command_passes()
        => Validator.Validate(Valid()).IsValid.ShouldBeTrue();

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("A")]
    public void An_empty_or_too_short_name_fails(string name)
        => Validator.Validate(Valid() with { FullName = name }).IsValid.ShouldBeFalse();

    [Theory]
    [InlineData("12345")]
    [InlineData("12345678901")]
    [InlineData("")]
    public void A_national_code_that_is_not_ten_characters_fails(string code)
        => Validator.Validate(Valid() with { NationalCode = code }).IsValid.ShouldBeFalse();

    [Theory]
    [InlineData(0)]
    [InlineData(37)]
    [InlineData(-1)]
    public void An_out_of_range_duration_fails(int months)
        => Validator.Validate(Valid() with { DurationMonths = months }).IsValid.ShouldBeFalse();

    [Fact]
    public void A_negative_fee_fails()
        => Validator.Validate(Valid() with { MonthlyFeeRial = -1m }).IsValid.ShouldBeFalse();

    [Fact]
    public void A_zero_fee_is_allowed_for_a_complimentary_membership()
        => Validator.Validate(Valid() with { MonthlyFeeRial = 0m }).IsValid.ShouldBeTrue();

    [Fact]
    public void Every_failure_reports_its_field()
    {
        var result = Validator.Validate(Valid() with { FullName = "", NationalCode = "1", DurationMonths = 0 });

        result.Errors.Select(e => e.PropertyName)
            .ShouldContain(name => name == nameof(RegisterMemberCommand.FullName));
        result.Errors.Count.ShouldBeGreaterThanOrEqualTo(3);
    }
}
