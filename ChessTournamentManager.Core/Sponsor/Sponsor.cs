using Ardalis.GuardClauses;
using ChessTournamentManager.Core.Base;

namespace ChessTournamentManager.Core.Sponsor;

public class Sponsor(Guid? id, string name, string logoUrl) : Entity(id), IAuditable, ISoftDeletable
{
    private string _name = Guard.Against.NullOrWhiteSpace(name);
    private string _logoUrl = Guard.Against.NullOrWhiteSpace(logoUrl);
    
    public string Name
    {
        get => _name;
        set => _name = Guard.Against.NullOrWhiteSpace(value);
    }
    
    public string LogoUrl
    {
        get => _logoUrl;
        set => _logoUrl = Guard.Against.NullOrWhiteSpace(value);
    }
    
    public bool IsDeleted { get; set; }
}