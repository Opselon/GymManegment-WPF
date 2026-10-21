using GymPro.Application.Abstractions;
using GymPro.Application.Attendance.Queries.ListCheckIns;
using GymPro.Application.Memberships.Queries.ListMembers;

namespace GymPro.Application.Attendance.Queries.ListCheckIns;

/// <summary>Lists the most recent check-ins, newest first.</summary>
public sealed class ListCheckInsHandler(ICheckInReadRepository checkIns)
    : IQueryHandler<ListCheckInsQuery, IReadOnlyList<CheckInListItem>>
{
    public async Task<IReadOnlyList<CheckInListItem>> Handle(
        ListCheckInsQuery request,
        CancellationToken cancellationToken)
    {
        var max = request.MaxResults is < 1 or > 1000 ? 200 : request.MaxResults;
        return await checkIns.ListRecentAsync(max, cancellationToken);
    }
}
