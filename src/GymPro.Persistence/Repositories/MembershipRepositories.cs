using GymPro.Application.Abstractions;
using GymPro.Domain.Memberships;
using GymPro.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace GymPro.Persistence.Repositories;

/// <summary>
/// EF Core implementation of the membership write repository.
/// </summary>
public sealed class MembershipWriteRepository(GymDbContext context) : IMembershipWriteRepository
{
    public void Add(Membership aggregate) => context.Memberships.Add(aggregate);

    public void Remove(Membership aggregate) => context.Memberships.Remove(aggregate);

    public async Task<bool> ExistsNationalCodeAsync(string nationalCode, CancellationToken cancellationToken = default)
        => await context.Memberships.AnyAsync(m => m.NationalCode == nationalCode, cancellationToken);

    public async Task<Membership?> GetByNationalCodeAsync(string nationalCode, CancellationToken cancellationToken = default)
        => await context.Memberships
            .Include(m => m.Payments)
            .FirstOrDefaultAsync(m => m.NationalCode == nationalCode, cancellationToken);
}

/// <summary>
/// EF Core implementation of the membership read repository. Queries are
/// no-tracking because read models are never written back.
/// </summary>
public sealed class MembershipReadRepository(GymDbContext context) : IMembershipReadRepository
{
    public async Task<Membership?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await context.Memberships
            .AsNoTracking()
            .Include(m => m.Payments)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Membership>> ListAsync(CancellationToken cancellationToken = default)
        => await context.Memberships
            .AsNoTracking()
            .Include(m => m.Payments)
            .OrderBy(m => m.FullName)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Membership>> ListActiveAsync(JalaliDate asOf, CancellationToken cancellationToken = default)
    {
        var cutoff = asOf.Gregorian;
        return await context.Memberships
            .AsNoTracking()
            .Include(m => m.Payments)
            .Where(m => m.Status == MemberStatus.Active)
            .Where(m => m.StartDate.Gregorian <= cutoff)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Membership>> ListExpiringBetweenAsync(
        JalaliDate from,
        JalaliDate until,
        CancellationToken cancellationToken = default)
    {
        var fromDate = from.Gregorian;
        var untilDate = until.Gregorian;
        return await context.Memberships
            .AsNoTracking()
            .Include(m => m.Payments)
            .Where(m => m.Status == MemberStatus.Active)
            .ToListAsync(cancellationToken);
    }
}
