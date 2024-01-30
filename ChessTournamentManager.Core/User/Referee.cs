using Ardalis.GuardClauses;

namespace ChessTournamentManager.Core.User;

public class Referee(string? licenseNumber) : ApplicationUser
{
    private string? _licenseNumber = licenseNumber is null ? null : Guard.Against.NullOrWhiteSpace(licenseNumber, nameof(licenseNumber));
    
    public string? LicenseNumber
    {
        get => _licenseNumber;
        set => _licenseNumber = value is null ? null :  Guard.Against.NullOrWhiteSpace(value, nameof(value));
    }
}