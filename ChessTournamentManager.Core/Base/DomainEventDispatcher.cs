using MediatR;

namespace ChessTournamentManager.Core.Base;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IEnumerable<Entity> entities);
}

public class DomainEventDispatcher(IPublisher mediator) : IDomainEventDispatcher
{
    public async Task DispatchAsync(IEnumerable<Entity> entities)
    {
        foreach (var entity in entities)
        {
            var events = entity.DomainEvents.ToArray();
            entity.ClearDomainEvents();
            
            foreach (var @event in events)
            {
                await mediator.Publish(@event);
            }
        }
    }
}