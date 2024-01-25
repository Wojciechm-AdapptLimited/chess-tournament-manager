using Ardalis.GuardClauses;

namespace ChessTournamentManager.Core.User;

public class Referee(string licenseNumber) : ApplicationUser
{
    private string _licenseNumber = Guard.Against.NullOrWhiteSpace(licenseNumber, nameof(licenseNumber));
    
    public string LicenseNumber
    {
        get => _licenseNumber;
        set => _licenseNumber = Guard.Against.NullOrWhiteSpace(value, nameof(value));
    }
}