namespace GymPro.Domain.Common;

/// <summary>
/// Base type for every aggregate root and entity in the domain.
/// </summary>
/// <remarks>
/// Equality is identity-based: two entities are the same thing when they are
/// the same type and share the same identifier. Transient entities (not yet
/// persisted, <see cref="Id"/> == 0) fall back to reference equality so that a
/// collection never collapses two unsaved rows into one.
/// </remarks>
public abstract class Entity
{
    public int Id { get; protected set; }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity other || other.GetType() != GetType())
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        // Both transient: identity is not meaningful yet.
        return Id != 0 && Id == other.Id;
    }

    public override int GetHashCode() => Id == 0 ? base.GetHashCode() : HashCode.Combine(GetType(), Id);

    public static bool operator ==(Entity? left, Entity? right) => Equals(left, right);

    public static bool operator !=(Entity? left, Entity? right) => !Equals(left, right);
}

/// <summary>
/// Marker for aggregate roots: the only entities repositories are allowed to
/// load and save directly.
/// </summary>
public abstract class AggregateRoot : Entity;

/// <summary>
/// An event that already happened in the domain. Dispatched after the
/// transaction that produced it commits.
/// </summary>
public interface IDomainEvent
{
    DateTimeOffset OccurredOn { get; }
}

/// <summary>
/// Base class for entities that accumulate domain events.
/// </summary>
public abstract class HasDomainEvents : AggregateRoot
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void Raise(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();
}
