using GymPro.Domain.Memberships;

namespace GymPro.Application.Abstractions;

/// <summary>
/// Loads a membership aggregate together with its payments. Kept separate from
/// the read repository because commands need the full object graph, not a
/// projected read model.
/// </summary>
public interface IMembershipAggregateLoader
{
    Task<Membership?> LoadAsync(int membershipId, CancellationToken cancellationToken = default);
}
