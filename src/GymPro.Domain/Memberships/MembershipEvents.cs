using GymPro.Domain.Common;

using GymPro.Domain.ValueObjects;

namespace GymPro.Domain.Memberships;

/// <summary>Raised when a new member registers.</summary>
public sealed record MemberRegistered(string FullName) : IDomainEvent
{
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
}

/// <summary>Raised when a membership is extended.</summary>
public sealed record MembershipRenewed(int ExtraMonths) : IDomainEvent
{
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
}

/// <summary>Raised when a membership is deactivated.</summary>
public sealed record MembershipDeactivated(string Reason) : IDomainEvent
{
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
}

/// <summary>Raised when a payment is recorded against a membership.</summary>
public sealed record PaymentRecorded(decimal AmountRial) : IDomainEvent
{
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
}

/// <summary>Raised when a membership's coverage lapses.</summary>
/// <param name="CoverageEndedOn">The last day that was paid for.</param>
public sealed record MembershipExpired(JalaliDate CoverageEndedOn) : IDomainEvent
{
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
}
