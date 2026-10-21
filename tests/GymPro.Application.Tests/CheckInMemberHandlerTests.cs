using GymPro.Application.Attendance.Commands.CheckInMember;
using GymPro.Application.Tests.Fakes;
using GymPro.Domain.Common;
using GymPro.Domain.Memberships;
using GymPro.Domain.ValueObjects;
using Shouldly;
using Xunit;

namespace GymPro.Application.Tests;

public sealed class CheckInMemberHandlerTests
{
    private static readonly FixedClock Clock = new(2024, 3, 31);

    private static (CheckInMemberHandler Handler, FakeMembershipRepository Repo, FakeCheckInStore CheckIns, FakeUnitOfWork Uow, int Id)
        Arrange()
    {
        var repo = new FakeMembershipRepository();
        var checkIns = new FakeCheckInStore();
        var uow = new FakeUnitOfWork();
        var member = Membership.RegisterNew(
            "Yara Kiani", Gender.Female, JalaliDate.Create(1403, 1, 1), 3,
            Money.FromRial(1000m), "1234567890", null, Clock);
        repo.Add(member);

        var handler = new CheckInMemberHandler(repo, checkIns, uow, Clock);
        return (handler, repo, checkIns, uow, member.Id);
    }

    [Fact]
    public async Task An_active_member_can_check_in()
    {
        var (handler, _, checkIns, _, id) = Arrange();

        var result = await handler.Handle(new CheckInMemberCommand(id), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value!.MemberName.ShouldBe("Yara Kiani");
        checkIns.All.ShouldHaveSingleItem().MembershipId.ShouldBe(id);
    }

    [Fact]
    public async Task The_receipt_reports_how_many_people_came_in_today()
    {
        var (handler, _, _, _, id) = Arrange();

        await handler.Handle(new CheckInMemberCommand(id), CancellationToken.None);
        var second = await handler.Handle(new CheckInMemberCommand(id), CancellationToken.None);

        second.Value!.TotalToday.ShouldBe(2);
    }

    [Fact]
    public async Task An_expired_membership_is_refused_at_the_desk()
    {
        var (handler, repo, checkIns, _, id) = Arrange();
        // Coverage runs 1403-01-01 to 1403-04-01, so just past that has lapsed.
        repo.All.Single().ExpireIfLapsed(JalaliDate.Create(1403, 1, 1).AddMonths(3).AddDays(1));

        var result = await handler.Handle(new CheckInMemberCommand(id), CancellationToken.None);

        result.IsFailure.ShouldBeTrue();
        result.Error!.Kind.ShouldBe(ErrorKind.Validation);
        checkIns.All.ShouldBeEmpty();
    }

    [Fact]
    public async Task An_inactive_membership_is_refused()
    {
        var (handler, repo, checkIns, _, id) = Arrange();
        repo.All.Single().Deactivate("left");

        var result = await handler.Handle(new CheckInMemberCommand(id), CancellationToken.None);

        result.IsFailure.ShouldBeTrue();
        checkIns.All.ShouldBeEmpty();
    }

    [Fact]
    public async Task An_unknown_membership_id_reports_not_found()
    {
        var (handler, _, _, _, _) = Arrange();

        var result = await handler.Handle(new CheckInMemberCommand(4242), CancellationToken.None);

        result.IsFailure.ShouldBeTrue();
        result.Error!.Kind.ShouldBe(ErrorKind.NotFound);
    }

    [Fact]
    public async Task The_write_happens_exactly_once_per_check_in()
    {
        var (_, repo, checkIns, uow, id) = Arrange();
        var handler = new CheckInMemberHandler(repo, checkIns, uow, Clock);

        await handler.Handle(new CheckInMemberCommand(id), CancellationToken.None);

        uow.SaveCount.ShouldBe(1);
    }
}
