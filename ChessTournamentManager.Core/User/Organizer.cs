using Ardalis.GuardClauses;

namespace ChessTournamentManager.Core.User;

public class Organizer : ApplicationUser
{
    private readonly List<Organization.Organization> _organizations = [];
    
    public IReadOnlyList<Organization.Organization> Organizations => _organizations.AsReadOnly();
    
    public void JoinOrganization(Organization.Organization organization)
    {
        Guard.Against.Null(organization, nameof(organization)); 

        if (_organizations.Contains(organization))
        {
            throw new InvalidOperationException($"{FullName} is already a member of this organization.");
        }

        _organizations.Add(organization);
    }
    
    public void LeaveOrganization(Organization.Organization organization)
    {
        Guard.Against.Null(organization, nameof(organization)); 

        if (!_organizations.Contains(organization))
        {
            throw new InvalidOperationException($"{FullName} is not a member of this organization.");
        }

        _organizations.Remove(organization);
    }
}