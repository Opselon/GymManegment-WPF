using GymPro.Application.Abstractions;

namespace GymPro.Application.Attendance.Queries.ListMeasurements;

/// <summary>Read model for one body-measurement record.</summary>
public sealed record MeasurementListItem(
    int Id,
    int MembershipId,
    string MeasuredOn,
    int? ChestCm,
    int? ArmCm,
    int? WaistCm,
    int? HeightCm,
    int? WeightKg);

/// <summary>Lists one member's measurement history, oldest first.</summary>
public sealed record ListMeasurementsQuery(int MembershipId)
    : IQuery<IReadOnlyList<MeasurementListItem>>;
