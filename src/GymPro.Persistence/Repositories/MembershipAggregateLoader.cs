using GymPro.Application.Abstractions;
using GymPro.Domain.Memberships;
using Microsoft.EntityFrameworkCore;

namespace GymPro.Persistence.Repositories;

/// <summary>
/// Loads the full membership aggregate (with payments) for command handlers.
/// Tracked, because the caller may mutate it and save.
/// </summary>
public sealed class MembershipAggregateLoader(GymDbContext context) : IMembershipAggregateLoader
{
    public async Task<Membership?> LoadAsync(int membershipId, CancellationToken cancellationToken = default)
        => await context.Memberships
            .Include(m => m.Payments)
            .FirstOrDefaultAsync(m => m.Id == membershipId, cancellationToken);
}
