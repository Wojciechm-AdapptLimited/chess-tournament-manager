using Ardalis.GuardClauses;

namespace ChessTournamentManager.Core.User;

public class Player(string? fideId) : ApplicationUser
{
    private string? _fideId = fideId is null ? null : Guard.Against.NullOrWhiteSpace(fideId, nameof(fideId));
    
    public string? FideId
    {
        get => _fideId;
        set => _fideId = value is null ? null : Guard.Against.NullOrWhiteSpace(value, nameof(value));
    }
}