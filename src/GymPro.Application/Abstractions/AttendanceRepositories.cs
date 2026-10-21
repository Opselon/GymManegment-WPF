using GymPro.Application.Attendance.Queries.ListCheckIns;
using GymPro.Application.Attendance.Queries.ListMeasurements;
using GymPro.Domain.Attendance;
using GymPro.Domain.ValueObjects;

namespace GymPro.Application.Abstractions;

/// <summary>
/// Write side of the attendance log. Check-ins are append-only: there is no
/// update and no delete, only a new row.
/// </summary>
public interface ICheckInWriteRepository
{
    /// <summary>Counts the check-ins recorded on the given day.</summary>
    Task<int> CountForDateAsync(JalaliDate day, CancellationToken cancellationToken = default);

    void Add(CheckIn checkIn);
}

/// <summary>
/// Read side of the attendance log. The projection joins the member name in the
/// database, so a reception screen needs one query rather than one per row.
/// </summary>
public interface ICheckInReadRepository
{
    /// <summary>Returns the most recent check-ins, newest first.</summary>
    Task<IReadOnlyList<CheckInListItem>> ListRecentAsync(
        int maxResults,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Write side of the body-measurement history. Like check-ins, measurements are
/// timestamped records, not an editable current-value column.
/// </summary>
public interface IMeasurementWriteRepository
{
    void Add(BodyMeasurement measurement);
}

/// <summary>Read side of the body-measurement history.</summary>
public interface IMeasurementReadRepository
{
    /// <summary>Returns one member's measurement history, oldest first.</summary>
    Task<IReadOnlyList<MeasurementListItem>> ListForMemberAsync(
        int membershipId,
        CancellationToken cancellationToken = default);
}
