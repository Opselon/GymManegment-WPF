using GymPro.Domain.Memberships;

using GymPro.Domain.ValueObjects;

namespace GymPro.Application.Abstractions;

/// <summary>Write side of the membership aggregate.</summary>
public interface IMembershipWriteRepository : IWriteRepository<Membership>
{
    Task<bool> ExistsNationalCodeAsync(string nationalCode, CancellationToken cancellationToken = default);
    Task<Membership?> GetByNationalCodeAsync(string nationalCode, CancellationToken cancellationToken = default);
}

/// <summary>Read side of the membership aggregate.</summary>
public interface IMembershipReadRepository : IRepository<Membership>
{
    /// <summary>Lists memberships that are paid-up as of the given date.</summary>
    Task<IReadOnlyList<Membership>> ListActiveAsync(JalaliDate asOf, CancellationToken cancellationToken = default);

    /// <summary>Lists members whose subscription ends within the given window.</summary>
    Task<IReadOnlyList<Membership>> ListExpiringBetweenAsync(JalaliDate from, JalaliDate until, CancellationToken cancellationToken = default);
}
