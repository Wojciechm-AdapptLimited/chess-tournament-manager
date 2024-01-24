using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChessTournamentManager.Core.Base;

public interface IAuditable;

public interface ISoftDeletable
{
    bool IsDeleted { get; set; } 
}

public abstract class Entity(Guid? id) : IEquatable<Entity>
{
    private readonly List<DomainEvent> _domainEvents = [];
    
    [Key]
    public Guid Id { get; } = id ?? Guid.NewGuid();
    
    [NotMapped]
    public IEnumerable<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public static bool operator ==(Entity? left, Entity? right) =>
        left switch
        {
            null when right is null => true,
            null => false,
            not null when right is null => false,
            _ => left.Equals(right)
        };

    public static bool operator !=(Entity? left, Entity? right) => !(left == right);

    public override bool Equals(object? obj) => Equals(obj as Entity);

    public override int GetHashCode() => Id.GetHashCode();

    public bool Equals(Entity? other) =>
        other is not null 
        && GetType() == other.GetType()
        && Id == other.Id;

    protected void RegisterDomainEvent(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    internal void ClearDomainEvents() => _domainEvents.Clear();
}