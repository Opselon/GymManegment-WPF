using GymPro.Domain.Memberships;
using GymPro.Domain.ValueObjects;
using Shouldly;
using Xunit;

namespace GymPro.Domain.Tests;

public sealed class MembershipTests
{
    private static readonly TimeProvider Clock = TimeProvider.System;

    private static Membership Register(string name = "Yara Kiani", int months = 3, int fee = 100)
        => Membership.RegisterNew(name, Gender.Female, JalaliDate.Create(1403, 1, 1), months,
            Money.FromRial(fee), "1234567890", "0215551111", Clock);

    [Fact]
    public void RegisterNew_creates_an_active_membership()
    {
        var member = Register();

        member.FullName.ShouldBe("Yara Kiani");
        member.Status.ShouldBe(MemberStatus.Active);
        member.StartDate.ShouldBe(JalaliDate.Create(1403, 1, 1));
        member.EndOfCoverage.ShouldBe(JalaliDate.Create(1403, 4, 1));
        member.NationalCode.ShouldBe("1234567890");
    }

    [Fact]
    public void RegisterNew_raises_a_member_registered_event()
    {
        var member = Register();

        member.DomainEvents.OfType<MemberRegistered>().ShouldHaveSingleItem().FullName.ShouldBe("Yara Kiani");
    }

    [Fact]
    public void A_name_is_required()
    {
        Should.Throw<ArgumentException>(() => Register(name: " "));
    }

    [Fact]
    public void A_national_code_must_be_ten_digits()
    {
        Should.Throw<ArgumentException>(() => Membership.RegisterNew(
            "Yara Kiani", Gender.Female, JalaliDate.Create(1403, 1, 1), 3, Money.FromRial(100),
            "12345", null, Clock));
    }

    [Fact]
    public void An_out_of_range_duration_is_rejected()
    {
        Should.Throw<ArgumentOutOfRangeException>(() => Register(months: 0));
        Should.Throw<ArgumentOutOfRangeException>(() => Register(name: "X", months: 99));
    }

    [Fact]
    public void The_outstanding_balance_is_total_fees_less_payments()
    {
        var member = Register(months: 3, fee: 200);

        member.OutstandingBalance.RialAmount.ShouldBe(600m);
    }

    [Fact]
    public void RecordPayment_increases_settlement_and_captures_the_date()
    {
        var member = Register();

        member.RecordPayment(Money.FromRial(100), JalaliDate.Create(1403, 1, 5), Clock);

        member.Payments.ShouldHaveSingleItem().Amount.RialAmount.ShouldBe(100m);
        member.OutstandingBalance.RialAmount.ShouldBe(200m);
    }

    [Fact]
    public void RecordPayment_raises_a_payment_event()
    {
        var member = Register();

        member.RecordPayment(Money.FromRial(100), JalaliDate.Create(1403, 1, 5), Clock);

        member.DomainEvents.OfType<PaymentRecorded>().ShouldHaveSingleItem().AmountRial.ShouldBe(100m);
    }

    [Fact]
    public void A_negative_payment_is_rejected()
    {
        var member = Register();

        Should.Throw<ArgumentException>(() =>
            member.RecordPayment(Money.FromRial(-1), JalaliDate.Create(1403, 1, 5), Clock));
    }

    [Fact]
    public void A_future_dated_payment_is_rejected()
    {
        var member = Register();
        var nextYear = JalaliDate.Today(Clock).AddYears(1);

        Should.Throw<ArgumentException>(() => member.RecordPayment(Money.FromRial(100), nextYear, Clock));
    }

    [Fact]
    public void Renew_extends_the_coverage_period_and_raises()
    {
        var member = Register();

        member.Renew(2, Money.FromRial(100), Clock);

        member.DurationMonths.ShouldBe(5);
        member.EndOfCoverage.ShouldBe(JalaliDate.Create(1403, 1, 1).AddMonths(5));
        member.DomainEvents.OfType<MembershipRenewed>().ShouldHaveSingleItem().ExtraMonths.ShouldBe(2);
    }

    [Fact]
    public void ExpireIfLapsed_marks_a_lapsed_membership_expired()
    {
        var member = Register();

        member.ExpireIfLapsed(member.EndOfCoverage.AddDays(1)).ShouldBeTrue();

        member.Status.ShouldBe(MemberStatus.Expired);
        member.DomainEvents.OfType<MembershipExpired>().ShouldHaveSingleItem();
    }

    [Fact]
    public void ExpireIfLapsed_leaves_a_current_membership_alone()
    {
        var member = Register();

        member.ExpireIfLapsed(member.EndOfCoverage).ShouldBeFalse();

        member.Status.ShouldBe(MemberStatus.Active);
    }

    [Fact]
    public void ExpireIfLapsed_is_idempotent()
    {
        var member = Register();
        member.ExpireIfLapsed(member.EndOfCoverage.AddDays(5));

        member.ExpireIfLapsed(member.EndOfCoverage.AddDays(6)).ShouldBeFalse();
    }

    [Fact]
    public void Renew_reactivates_an_expired_membership()
    {
        var member = Register();
        member.ExpireIfLapsed(member.EndOfCoverage.AddDays(1));

        member.Renew(1, Money.FromRial(100), Clock);

        member.Status.ShouldBe(MemberStatus.Active);
    }

    [Fact]
    public void Renew_rejects_an_invalid_month_count()
    {
        var member = Register();

        Should.Throw<ArgumentOutOfRangeException>(() => member.Renew(0, Money.FromRial(100), Clock));
    }

    [Fact]
    public void Deactivate_sets_inactive_and_raises_once_only()
    {
        var member = Register();
        member.Deactivate("left");

        member.Status.ShouldBe(MemberStatus.Inactive);
        member.DomainEvents.OfType<MembershipDeactivated>().ShouldHaveSingleItem().Reason.ShouldBe("left");

        member.ClearDomainEvents();
        member.Deactivate("again");

        member.DomainEvents.ShouldBeEmpty();
    }
}
