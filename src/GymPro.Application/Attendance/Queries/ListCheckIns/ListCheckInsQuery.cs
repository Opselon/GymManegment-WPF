using GymPro.Application.Abstractions;
using GymPro.Domain.Attendance;

namespace GymPro.Application.Attendance.Queries.ListCheckIns;

/// <summary>Read model for one row of the attendance log.</summary>
public sealed record CheckInListItem(
    int Id,
    int MembershipId,
    string MemberName,
    string CheckedInOn);

/// <summary>Lists the most recent check-ins, newest first.</summary>
public sealed record ListCheckInsQuery(int MaxResults = 200) : IQuery<IReadOnlyList<CheckInListItem>>;
