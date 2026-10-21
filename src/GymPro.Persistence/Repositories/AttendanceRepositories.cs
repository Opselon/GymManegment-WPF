using GymPro.Application.Abstractions;
using GymPro.Application.Attendance.Queries.ListCheckIns;
using GymPro.Application.Attendance.Queries.ListMeasurements;
using GymPro.Domain.Attendance;
using GymPro.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace GymPro.Persistence.Repositories;

/// <summary>Append-only writes to the attendance log.</summary>
public sealed class CheckInWriteRepository(GymDbContext context) : ICheckInWriteRepository
{
    public void Add(CheckIn checkIn) => context.CheckIns.Add(checkIn);

    public Task<int> CountForDateAsync(JalaliDate day, CancellationToken cancellationToken = default)
        => context.CheckIns.CountAsync(c => c.CheckedInOn == day, cancellationToken);
}

/// <summary>
/// Read projections for the attendance log. The member name is joined here so
/// the screen does not issue a query per row.
/// </summary>
public sealed class CheckInReadRepository(GymDbContext context) : ICheckInReadRepository
{
    public async Task<IReadOnlyList<CheckInListItem>> ListRecentAsync(
        int maxResults,
        CancellationToken cancellationToken = default)
        => await Project(
                context.CheckIns.OrderByDescending(c => c.CheckedInOn).ThenByDescending(c => c.Id),
                maxResults)
            .ToListAsync(cancellationToken);

    private IQueryable<CheckInListItem> Project(IQueryable<CheckIn> source, int maxResults)
        => source
            .Take(maxResults)
            .GroupJoin(
                context.Memberships,
                c => c.MembershipId,
                m => m.Id,
                (c, members) => new { CheckIn = c, Members = members })
            .SelectMany(
                x => x.Members.DefaultIfEmpty(),
                (x, member) => new CheckInListItem(
                    x.CheckIn.Id,
                    x.CheckIn.MembershipId,
                    member == null ? "—" : member.FullName,
                    x.CheckIn.CheckedInOn.ToString()));
}

/// <summary>Append-only writes to the measurement history.</summary>
public sealed class MeasurementWriteRepository(GymDbContext context) : IMeasurementWriteRepository
{
    public void Add(BodyMeasurement measurement) => context.BodyMeasurements.Add(measurement);
}

/// <summary>Read projections for the measurement history.</summary>
public sealed class MeasurementReadRepository(GymDbContext context) : IMeasurementReadRepository
{
    public async Task<IReadOnlyList<MeasurementListItem>> ListForMemberAsync(
        int membershipId,
        CancellationToken cancellationToken = default)
        => await context.BodyMeasurements
            .Where(b => b.MembershipId == membershipId)
            .OrderBy(b => b.MeasuredOn)
            .ThenBy(b => b.Id)
            .Select(b => new MeasurementListItem(
                b.Id,
                b.MembershipId,
                b.MeasuredOn.ToString(),
                b.ChestCm,
                b.ArmCm,
                b.WaistCm,
                b.HeightCm,
                b.WeightKg))
            .ToListAsync(cancellationToken);
}
