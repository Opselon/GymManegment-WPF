using System.Threading;
using GymPro.Application.Memberships.Commands.RegisterMember;
using GymPro.Application.Tests.Fakes;
using GymPro.Domain.Common;
using GymPro.Domain.Memberships;
using Shouldly;
using Xunit;

namespace GymPro.Application.Tests;

public sealed class RegisterMemberHandlerTests
{
    private static readonly FixedClock Clock = new(2024, 3, 31);

    private static RegisterMemberCommand Valid(
        string nationalCode = "1234567890",
        int durationMonths = 3,
        decimal fee = 300_000_000m)
        => new("Yara Kiani", (byte)Gender.Female, nationalCode, "0215551111",
            StartYear: 1403, StartMonth: 1, StartDay: 1,
            DurationMonths: durationMonths, MonthlyFeeRial: fee);

    [Fact]
    public async Task A_valid_command_registers_the_member()
    {
        var repo = new FakeMembershipRepository();
        var uow = new FakeUnitOfWork();
        var handler = new RegisterMemberHandler(repo, uow, Clock);

        var result = await handler.Handle(Valid(), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        repo.All.ShouldHaveSingleItem().FullName.ShouldBe("Yara Kiani");
    }

    [Fact]
    public async Task The_commit_happens_exactly_once()
    {
        var uow = new FakeUnitOfWork();
        var handler = new RegisterMemberHandler(new FakeMembershipRepository(), uow, Clock);

        await handler.Handle(Valid(), CancellationToken.None);

        uow.SaveCount.ShouldBe(1);
    }

    [Fact]
    public async Task A_duplicate_national_code_is_rejected_without_writing()
    {
        var repo = new FakeMembershipRepository();
        var uow = new FakeUnitOfWork();
        var handler = new RegisterMemberHandler(repo, uow, Clock);

        await handler.Handle(Valid(nationalCode: "9876543210"), CancellationToken.None);
        var second = await handler.Handle(Valid(nationalCode: "9876543210"), CancellationToken.None);

        second.IsFailure.ShouldBeTrue();
        second.Error!.Kind.ShouldBe(ErrorKind.Conflict);
        repo.All.Count.ShouldBe(1);
        uow.SaveCount.ShouldBe(1);
    }

    [Fact]
    public async Task An_impossible_Jalali_start_date_is_rejected()
    {
        var handler = new RegisterMemberHandler(new FakeMembershipRepository(), new FakeUnitOfWork(), Clock);
        var command = Valid() with { StartYear = 1402, StartMonth = 12, StartDay = 30 };

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsFailure.ShouldBeTrue();
        result.Error!.Kind.ShouldBe(ErrorKind.Validation);
    }

    [Fact]
    public async Task A_future_start_date_is_rejected()
    {
        var handler = new RegisterMemberHandler(new FakeMembershipRepository(), new FakeUnitOfWork(), Clock);
        var command = Valid() with { StartYear = 1405, StartMonth = 1, StartDay = 1 };

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsFailure.ShouldBeTrue();
        result.Error!.Kind.ShouldBe(ErrorKind.Validation);
    }
}
