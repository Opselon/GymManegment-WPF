using GymPro.Application.Attendance.Commands.RecordMeasurement;
using GymPro.Application.Tests.Fakes;
using GymPro.Domain.Common;
using GymPro.Domain.Memberships;
using GymPro.Domain.ValueObjects;
using Shouldly;
using Xunit;

namespace GymPro.Application.Tests;

public sealed class RecordMeasurementHandlerTests
{
    private static readonly FixedClock Clock = new(2024, 3, 31);

    private static (RecordMeasurementHandler Handler, FakeMembershipRepository Repo, FakeMeasurementStore Store, int Id)
        Arrange()
    {
        var repo = new FakeMembershipRepository();
        var store = new FakeMeasurementStore();
        var uow = new FakeUnitOfWork();
        var member = Membership.RegisterNew(
            "Yara Kiani", Gender.Female, JalaliDate.Create(1403, 1, 1), 3,
            Money.FromRial(1000m), "1234567890", null, Clock);
        repo.Add(member);

        return (new RecordMeasurementHandler(repo, store, uow, Clock), repo, store, member.Id);
    }

    [Fact]
    public async Task A_valid_measurement_is_recorded()
    {
        var (handler, _, store, id) = Arrange();

        var result = await handler.Handle(new RecordMeasurementCommand(
            id, Year: 1403, Month: 1, Day: 5, WeightKg: 68, HeightCm: 172), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue(result.Error?.Message);
        store.All.ShouldHaveSingleItem().WeightKg.ShouldBe(68);
    }

    [Fact]
    public async Task All_fields_may_be_left_null_only_if_at_least_one_is_set()
    {
        var (handler, _, _, id) = Arrange();

        var result = await handler.Handle(
            new RecordMeasurementCommand(id, 1403, 2, 15), CancellationToken.None);

        result.IsFailure.ShouldBeTrue();
        result.Error!.Kind.ShouldBe(ErrorKind.Validation);
    }

    [Fact]
    public async Task An_impossible_date_is_rejected()
    {
        var (handler, _, _, id) = Arrange();

        var result = await handler.Handle(
            new RecordMeasurementCommand(id, 1402, 12, 30, WeightKg: 68), CancellationToken.None);

        result.IsFailure.ShouldBeTrue();
        result.Error!.Kind.ShouldBe(ErrorKind.Validation);
    }

    [Fact]
    public async Task A_future_dated_measurement_is_rejected()
    {
        var (handler, _, _, id) = Arrange();

        var result = await handler.Handle(
            new RecordMeasurementCommand(id, 1410, 1, 1, WeightKg: 68), CancellationToken.None);

        result.IsFailure.ShouldBeTrue();
        result.Error!.Kind.ShouldBe(ErrorKind.Validation);
    }

    [Fact]
    public async Task An_unknown_member_is_reported_as_not_found()
    {
        var (handler, _, _, _) = Arrange();

        var result = await handler.Handle(
            new RecordMeasurementCommand(777, 1403, 2, 15, WeightKg: 68), CancellationToken.None);

        result.IsFailure.ShouldBeTrue();
        result.Error!.Kind.ShouldBe(ErrorKind.NotFound);
    }
}
