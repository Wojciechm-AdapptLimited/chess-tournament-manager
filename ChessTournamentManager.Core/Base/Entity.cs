using System.ComponentModel.DataAnnotations;

namespace ChessTournamentManager.Core.Base;

public abstract class Entity(Guid? id) : IEquatable<Entity>
{
    [Key]
    public Guid Id { get; } = id ?? Guid.NewGuid();

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
}