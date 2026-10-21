using GymPro.Application.Abstractions;
using GymPro.Application.Attendance.Queries.ListMeasurements;
using GymPro.Domain.Common;
using GymPro.Domain.ValueObjects;

namespace GymPro.Application.Attendance.Commands.RecordMeasurement;

/// <summary>Records a body measurement for a member.</summary>
public sealed record RecordMeasurementCommand(
    int MembershipId,
    int Year,
    int Month,
    int Day,
    int? ChestCm = null,
    int? ArmCm = null,
    int? WaistCm = null,
    int? HeightCm = null,
    int? WeightKg = null) : ICommand<Result<int>>;

public sealed class RecordMeasurementHandler(
    IMembershipAggregateLoader memberships,
    IMeasurementWriteRepository measurements,
    IUnitOfWork unitOfWork,
    TimeProvider clock)
    : ICommandHandler<RecordMeasurementCommand, Result<int>>
{
    public async Task<Result<int>> Handle(
        RecordMeasurementCommand request,
        CancellationToken cancellationToken)
    {
        var membership = await memberships.LoadAsync(request.MembershipId, cancellationToken);
        if (membership is null)
        {
            return Result.Failure<int>(DomainError.NotFound("membership", request.MembershipId));
        }

        if (!JalaliDate.TryCreate(request.Year, request.Month, request.Day, out var measuredOn, out var dateError))
        {
            return Result.Failure<int>(DomainError.Validation(
                "measuredOn", dateError ?? "The measurement date is not a valid Jalali date."));
        }

        if (measuredOn > JalaliDate.Today(clock))
        {
            return Result.Failure<int>(
                DomainError.Validation("measuredOn", "A measurement cannot be dated in the future."));
        }

        if (request is { ChestCm: null, ArmCm: null, WaistCm: null, HeightCm: null, WeightKg: null })
        {
            return Result.Failure<int>(
                DomainError.Validation("measurement", "Record at least one measurement value."));
        }

        var measurement = Domain.Attendance.BodyMeasurement.Record(
            request.MembershipId,
            measuredOn,
            request.ChestCm,
            request.ArmCm,
            request.WaistCm,
            request.HeightCm,
            request.WeightKg);

        measurements.Add(measurement);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(measurement.Id);
    }
}
