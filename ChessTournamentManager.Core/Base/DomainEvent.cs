using MediatR;

namespace ChessTournamentManager.Core.Base;

public abstract class DomainEvent : INotification
{
    public DateTime OccurredOn { get; protected set; } = DateTime.UtcNow;
}