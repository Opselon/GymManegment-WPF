using GymPro.Application.Abstractions;
using GymPro.Domain.Attendance;
using GymPro.Domain.Common;
using GymPro.Domain.Memberships;
using GymPro.Domain.ValueObjects;

namespace GymPro.Application.Tests.Fakes;

/// <summary>In-memory membership store covering every membership-facing port.</summary>
internal sealed class FakeMembershipRepository
    : IMembershipWriteRepository, IMembershipReadRepository, IMembershipAggregateLoader
{
    private readonly Dictionary<int, Membership> _items = [];

    public IReadOnlyCollection<Membership> All => _items.Values;

    public void Add(Membership aggregate)
    {
        // Mirror the database: the store allocates identity on insert.
        var id = _items.Count + 1;
        aggregate.SetIdForTest(id);
        _items[id] = aggregate;
    }

    public void Remove(Membership aggregate) => _items.Remove(aggregate.Id);

    public Task<Membership?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => Task.FromResult(_items.GetValueOrDefault(id));

    public Task<IReadOnlyList<Membership>> ListAsync(CancellationToken cancellationToken = default)
        => Task.FromResult<IReadOnlyList<Membership>>(_items.Values.ToList());

    public Task<bool> ExistsNationalCodeAsync(string nationalCode, CancellationToken cancellationToken = default)
        => Task.FromResult(_items.Values.Any(m => m.NationalCode == nationalCode));

    public Task<Membership?> GetByNationalCodeAsync(string nationalCode, CancellationToken cancellationToken = default)
        => Task.FromResult(_items.Values.FirstOrDefault(m => m.NationalCode == nationalCode));

    public Task<IReadOnlyList<Membership>> ListActiveAsync(JalaliDate asOf, CancellationToken cancellationToken = default)
        => Task.FromResult<IReadOnlyList<Membership>>(
            _items.Values.Where(m => m.Status == MemberStatus.Active && !m.IsExpired(asOf)).ToList());

    public Task<IReadOnlyList<Membership>> ListExpiringBetweenAsync(
        JalaliDate from,
        JalaliDate until,
        CancellationToken cancellationToken = default)
        => Task.FromResult<IReadOnlyList<Membership>>(
            _items.Values.Where(m => m.EndOfCoverage >= from && m.EndOfCoverage <= until).ToList());

    public Task<Membership?> LoadAsync(int id, CancellationToken cancellationToken = default)
        => GetByIdAsync(id, cancellationToken);
}

/// <summary>Counts commits, so tests can prove the handler is a transaction boundary.</summary>
internal sealed class FakeUnitOfWork : IUnitOfWork
{
    public int SaveCount { get; private set; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SaveCount++;
        return Task.FromResult(1);
    }
}

/// <summary>Deterministic clock, so date assertions never depend on wall time.</summary>
internal sealed class FixedClock : TimeProvider
{
    private readonly DateTimeOffset _now;

    public FixedClock(int year, int month, int day)
        => _now = new DateTimeOffset(year, month, day, 10, 0, 0, TimeSpan.FromHours(3.5));

    public override DateTimeOffset GetUtcNow() => _now;
}

/// <summary>
/// In-memory attendance log. Stands in for ICheckInWriteRepository's write face;
/// ListRecent projections are covered by the persistence tests against SQLite.
/// </summary>
internal sealed class FakeCheckInStore : ICheckInWriteRepository
{
    private readonly List<CheckIn> _items = [];

    public IReadOnlyList<CheckIn> All => _items;

    public void Add(CheckIn checkIn) => _items.Add(checkIn);

    public Task<int> CountForDateAsync(JalaliDate day, CancellationToken cancellationToken = default)
        => Task.FromResult(_items.Count(c => c.CheckedInOn == day));
}

/// <summary>In-memory measurement store.</summary>
internal sealed class FakeMeasurementStore : IMeasurementWriteRepository
{
    private readonly List<BodyMeasurement> _items = [];

    public IReadOnlyList<BodyMeasurement> All => _items;

    public void Add(BodyMeasurement measurement) => _items.Add(measurement);
}
