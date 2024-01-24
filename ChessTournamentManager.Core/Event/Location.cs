using Ardalis.GuardClauses;

namespace ChessTournamentManager.Core.Event;

public record Location(double Longitude, double Latitude)
{
    public double Longitude { get; init; } = Guard.Against.NullOrOutOfRange(Longitude, nameof(Longitude), -180, 180);
    public double Latitude { get; init; } = Guard.Against.NullOrOutOfRange(Latitude, nameof(Latitude), -90, 90);
}