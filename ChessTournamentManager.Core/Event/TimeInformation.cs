using Ardalis.GuardClauses;

namespace ChessTournamentManager.Core.Event;

public record TimeInformation(DateTime StartDate, DateTime EndDate)
{
    public DateTime StartDate { get; init; } = Guard.Against.NullOrOutOfRange(StartDate, nameof(StartDate), DateTime.Now, EndDate).ToUniversalTime();
    public DateTime EndDate { get; init; } = Guard.Against.NullOrOutOfRange(EndDate, nameof(EndDate), StartDate, DateTime.MaxValue).ToUniversalTime();
}