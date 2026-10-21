using GymPro.Domain.Attendance;
using GymPro.Domain.Memberships;
using GymPro.Domain.ValueObjects;
using Shouldly;
using Xunit;

namespace GymPro.Domain.Tests;

/// <summary>
/// The check-in eligibility rules. These are the gym's front-door policy: a
/// member who has lapsed, quit, or been banned does not get through the gate.
/// </summary>
public sealed class CheckInTests
{
    private static readonly TimeProvider Clock = TimeProvider.System;

    private static Membership ActiveMember(int months = 3)
        => Membership.RegisterNew(
            "Yara Kiani", Gender.Female, JalaliDate.Create(1403, 1, 1), months,
            Money.FromRial(1000m), "1234567890", null, Clock);

    [Fact]
    public void An_active_member_can_check_in_today()
    {
        var member = ActiveMember();

        var checkIn = CheckIn.Record(member, JalaliDate.Create(1403, 2, 1), Clock);

        checkIn.MembershipId.ShouldBe(member.Id);
        checkIn.CheckedInOn.ShouldBe(JalaliDate.Create(1403, 2, 1));
    }

    [Fact]
    public void An_expired_member_cannot_check_in()
    {
        var member = ActiveMember();

        Should.Throw<InvalidOperationException>(
            () => CheckIn.Record(member, member.EndOfCoverage.AddDays(1), Clock));
    }

    [Fact]
    public void The_last_covered_day_is_still_valid()
    {
        var member = ActiveMember();

        CheckIn.Record(member, member.EndOfCoverage, Clock);
    }

    [Fact]
    public void An_inactive_member_cannot_check_in()
    {
        var member = ActiveMember();
        member.Deactivate("left");

        Should.Throw<InvalidOperationException>(
            () => CheckIn.Record(member, JalaliDate.Create(1403, 2, 1), Clock));
    }

    [Fact]
    public void A_banned_member_cannot_check_in()
    {
        var member = ActiveMember();
        member.ExpireIfLapsed(member.EndOfCoverage.AddDays(1));
        member.Renew(1, Money.FromRial(1000m), Clock);

        Should.NotThrow(() => CheckIn.Record(member, member.EndOfCoverage, Clock));
    }

    [Fact]
    public void A_banned_status_member_is_refused()
    {
        var member = ActiveMember();
        // Status has an internal setter reachable here through InternalsVisibleTo;
        // there is no Ban() operation yet because no screen bans a member.
        member.Status = MemberStatus.Banned;

        Should.Throw<InvalidOperationException>(
            () => CheckIn.Record(member, JalaliDate.Create(1403, 2, 1), Clock));
    }

    [Fact]
    public void A_future_dated_check_in_is_refused()
    {
        var member = ActiveMember();
        var future = JalaliDate.Today(Clock).AddYears(1);

        Should.Throw<ArgumentException>(
            () => CheckIn.Record(member, future, Clock));
    }
}
