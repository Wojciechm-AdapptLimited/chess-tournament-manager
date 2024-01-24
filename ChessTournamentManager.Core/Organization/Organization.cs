using Ardalis.GuardClauses;
using ChessTournamentManager.Core.Base;
using ChessTournamentManager.Core.User;

namespace ChessTournamentManager.Core.Organization;

public class Organization(Guid? id, string name, string? description, string email, Organizer owner) : AggregateRoot(id), IAuditable, ISoftDeletable
{
    private string _name = Guard.Against.NullOrWhiteSpace(name);
    private string _email = Guard.Against.NullOrWhiteSpace(email);
    private Organizer _owner = Guard.Against.Null(owner, nameof(owner));
    
    public string Name
    {
        get => _name;
        set => _name = Guard.Against.NullOrWhiteSpace(value);
    }
    
    public string? Description { get; set; } = description;

    public string Email
    {
        get => _email;
        set => _email = Guard.Against.NullOrWhiteSpace(value);
    }
    
    public Organizer Owner
    {
        get => _owner;
        set => _owner = Guard.Against.Null(value, nameof(value));
    }
    
    public bool IsDeleted { get; set; }
    
    public virtual IReadOnlyCollection<Organizer> Members { get; set; } = new List<Organizer>();
    public virtual IReadOnlyCollection<Event.Event> Events { get; set; } = new List<Event.Event>();
}