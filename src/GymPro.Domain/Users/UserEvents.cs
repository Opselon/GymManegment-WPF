using GymPro.Domain.Common;

namespace GymPro.Domain.Users;

/// <summary>Raised when a new user account is created.</summary>
public sealed record UserRegistered(string Username) : IDomainEvent
{
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
}

/// <summary>Raised when a user account is suspended.</summary>
public sealed record UserSuspended(string Username) : IDomainEvent
{
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
}

/// <summary>Raised when a user account is reinstated.</summary>
public sealed record UserReinstated(string Username) : IDomainEvent
{
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
}
