using Ardalis.GuardClauses;
using ChessTournamentManager.Core.Base;
using ChessTournamentManager.Core.User;

namespace ChessTournamentManager.Core.Game;

public class Game(Guid? id, Tournament.Tournament tournament, int round, Player player1, Player player2) : Entity(id), IAuditable, ISoftDeletable
{
    private Tournament.Tournament _tournament = Guard.Against.Null(tournament, nameof(tournament));
    private Player _player1 = Guard.Against.Null(player1, nameof(player1));
    private Player _player2 = Guard.Against.Null(player2, nameof(player2));
    private GameResult _result = GameResult.NotPlayed;
    
    public Player Player1
    {
        get => _player1;
        set => _player1 = Guard.Against.Null(value, nameof(value));
    }
    
    public Player Player2
    {
        get => _player2;
        set => _player2 = Guard.Against.Null(value, nameof(value));
    }
    
    public GameResult Result
    {
        get => _result;
        set => _result = Guard.Against.EnumOutOfRange(value, nameof(value));
    }
    
    public int Round { get; } = Guard.Against.OutOfRange(round, nameof(round), 1, tournament.RoundCount);
    
    public bool IsDeleted { get; set; }
}