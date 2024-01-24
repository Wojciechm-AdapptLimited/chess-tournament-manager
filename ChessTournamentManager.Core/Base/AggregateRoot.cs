using System.ComponentModel.DataAnnotations.Schema;

namespace ChessTournamentManager.Core.Base;

public abstract class AggregateRoot(Guid? id) : Entity(id)
{
    private readonly List<DomainEvent> _domainEvents = new();
    
    [NotMapped]
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    
    protected void RegisterDomainEvent(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    internal void ClearDomainEvents() => _domainEvents.Clear();
}