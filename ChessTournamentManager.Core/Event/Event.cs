using Ardalis.GuardClauses;
using ChessTournamentManager.Core.Base;

namespace ChessTournamentManager.Core.Event;

public class Event(Guid? id, Organization.Organization organization, string name, string? description, Location location, TimeInformation timeInformation)
    : Entity(id), IAuditable, ISoftDeletable
{
    private string _name = Guard.Against.NullOrWhiteSpace(name);
    private Location _location = Guard.Against.Null(location, nameof(location));
    private TimeInformation _timeInformation = Guard.Against.Null(timeInformation, nameof(timeInformation));
    
    public Organization.Organization Organization { get; } = Guard.Against.Null(organization, nameof(organization));

    public string Name
    {
        get => _name;
        set => _name = Guard.Against.NullOrWhiteSpace(value);
    }

    public string? Description { get; set; } = description;
    
    public Location Location
    {
        get => _location;
        set => _location = Guard.Against.Null(value, nameof(value));
    }
    
    public TimeInformation TimeInformation
    {
        get => _timeInformation;
        set => _timeInformation = Guard.Against.Null(value, nameof(value));
    }
    
    public bool IsDeleted { get; set; }
}