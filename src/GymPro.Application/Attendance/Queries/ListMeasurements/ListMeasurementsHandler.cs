using GymPro.Application.Abstractions;
using GymPro.Application.Attendance.Queries.ListMeasurements;
using GymPro.Domain.Common;

namespace GymPro.Application.Attendance.Queries.ListMeasurements;

/// <summary>Lists one member's measurement history, oldest first.</summary>
public sealed class ListMeasurementsHandler(IMeasurementReadRepository measurements)
    : IQueryHandler<ListMeasurementsQuery, IReadOnlyList<MeasurementListItem>>
{
    public async Task<IReadOnlyList<MeasurementListItem>> Handle(
        ListMeasurementsQuery request,
        CancellationToken cancellationToken)
        => await measurements.ListForMemberAsync(request.MembershipId, cancellationToken);
}
