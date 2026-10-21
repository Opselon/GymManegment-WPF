using System.Threading;
using GymPro.Application.Memberships.Commands.ChargeMembership;
using GymPro.Application.Tests.Fakes;
using GymPro.Domain.Common;
using GymPro.Domain.Memberships;
using GymPro.Domain.ValueObjects;
using Shouldly;
using Xunit;

namespace GymPro.Application.Tests;

public sealed class ChargeMembershipHandlerTests
{
    private static readonly FixedClock Clock = new(2024, 3, 31);

    private static (ChargeMembershipHandler Handler, FakeMembershipRepository Repo, FakeUnitOfWork Uow, int Id)
        Arrange(int months = 3, decimal fee = 1000m)
    {
        var repo = new FakeMembershipRepository();
        var uow = new FakeUnitOfWork();
        var member = Membership.RegisterNew(
            "Yara Kiani", Gender.Female, JalaliDate.Create(1403, 1, 1), months,
            Money.FromRial(fee), "1234567890", null, Clock);
        repo.Add(member);

        return (new ChargeMembershipHandler(repo, uow, Clock), repo, uow, member.Id);
    }

    [Fact]
    public async Task A_payment_is_recorded_and_reported()
    {
        var (handler, repo, _, id) = Arrange(fee: 1000m);

        var result = await handler.Handle(new ChargeMembershipCommand(id, 1000m), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value!.PaidRial.ShouldBe(1000m);
        result.Value.OutstandingRial.ShouldBe(2000m);
        repo.All.Single().Payments.ShouldHaveSingleItem();
    }

    [Fact]
    public async Task A_legacy_id_returns_not_found_rather_than_throwing()
    {
        var (handler, _, uow, _) = Arrange();

        var result = await handler.Handle(new ChargeMembershipCommand(9999, 100m), CancellationToken.None);

        result.IsFailure.ShouldBeTrue();
        result.Error!.Kind.ShouldBe(ErrorKind.NotFound);
        uow.SaveCount.ShouldBe(0);
    }

    [Fact]
    public async Task A_negative_payment_is_rejected()
    {
        var (handler, _, uow, id) = Arrange();

        var result = await handler.Handle(new ChargeMembershipCommand(id, -5m), CancellationToken.None);

        result.IsFailure.ShouldBeTrue();
        result.Error!.Kind.ShouldBe(ErrorKind.Validation);
        uow.SaveCount.ShouldBe(0);
    }

    [Fact]
    public async Task Paying_the_full_balance_clears_the_debt()
    {
        var (handler, repo, _, id) = Arrange(months: 3, fee: 1000m);

        var result = await handler.Handle(new ChargeMembershipCommand(id, 3000m), CancellationToken.None);

        result.Value!.OutstandingRial.ShouldBe(0m);
        repo.All.Single().OutstandingBalance.RialAmount.ShouldBe(0m);
    }
}
