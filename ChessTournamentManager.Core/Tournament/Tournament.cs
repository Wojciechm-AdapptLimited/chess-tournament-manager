using Ardalis.GuardClauses;
using ChessTournamentManager.Core.Base;
using ChessTournamentManager.Core.User;

namespace ChessTournamentManager.Core.Tournament;

public class Tournament(Guid? id, Event.Event @event, TournamentType type, TournamentFormat format, bool isOpen, int maxPlayers, int requiredReferees, DateTime deadline, int roundCount)
    : Entity(id), IAuditable, ISoftDeletable
{
    private Event.Event _event = Guard.Against.Null(@event, nameof(@event));
    private TournamentType _type = Guard.Against.EnumOutOfRange(type, nameof(type));
    private TournamentFormat _format = Guard.Against.EnumOutOfRange(format, nameof(format));
    private int _maxPlayers = Guard.Against.OutOfRange(maxPlayers, nameof(maxPlayers), 0, int.MaxValue);
    private int _requiredReferees = Guard.Against.OutOfRange(requiredReferees, nameof(requiredReferees), 0, int.MaxValue);
    private DateTime _deadline = Guard.Against.OutOfRange(deadline, nameof(deadline), DateTime.Now, @event.TimeInformation.StartDate);
    private int _roundCount = Guard.Against.OutOfRange(roundCount, nameof(roundCount), 0, int.MaxValue);
    private int _currentRound;
    
    private readonly List<Player> _players = [];
    private readonly List<Referee> _referees = [];
    private readonly List<Game.Game> _games = [];
    private readonly List<Sponsor.Sponsor> _sponsors = [];
    
    public Event.Event Event 
    {
        get => _event;
        set => _event = Guard.Against.Null(value, nameof(value));
    }
    
    public TournamentType Type 
    {
        get => _type;
        set => _type = Guard.Against.EnumOutOfRange(value, nameof(value)); 
    }
    
    public TournamentFormat Format 
    {
        get => _format;
        set => _format = Guard.Against.EnumOutOfRange(value, nameof(value));
    }
    
    public DateTime Deadline 
    {
        get => _deadline;
        set => _deadline = Guard.Against.OutOfRange(value, nameof(value), DateTime.Now, Event.TimeInformation.StartDate);
    }
    
    public int RoundCount 
    {
        get => _roundCount;
        set => _roundCount = Guard.Against.OutOfRange(value, nameof(value), 0, int.MaxValue);
    }
    
    public int CurrentRound 
    {
        get => _currentRound;
        set => _currentRound = Guard.Against.OutOfRange(value, nameof(value), _currentRound + 1, RoundCount);
    }
    
    public IReadOnlyCollection<Player> Players => _players.AsReadOnly();
    public IReadOnlyCollection<Referee> Referees => _referees.AsReadOnly();
    public IReadOnlyCollection<Game.Game> Games => _games.AsReadOnly();
    public IReadOnlyCollection<Sponsor.Sponsor> Sponsors => _sponsors.AsReadOnly();
    
    public bool IsOpen { get; set; } = isOpen;
    
    public int MaxPlayers 
    {
        get => _maxPlayers;
        set => _maxPlayers = Guard.Against.OutOfRange(value, nameof(value), 0, int.MaxValue);
    }
    
    public int RequiredReferees 
    {
        get => _requiredReferees;
        set => _requiredReferees = Guard.Against.OutOfRange(value, nameof(value), 0, int.MaxValue);
    }
    
    public bool IsDeleted { get; set; }
    
    public void AddPlayer(Player player)
    {
        Guard.Against.Null(player, nameof(player));
        
        if (_players.Count >= _maxPlayers)
        {
            throw new InvalidOperationException("The tournament is full.");
        }
        
        if (_players.Contains(player))
        {
            throw new InvalidOperationException("The player is already registered.");
        }
        
        if (!IsOpen && string.IsNullOrEmpty(player.FideId))
        {
            throw new InvalidOperationException("The tournament is restricted to FIDE players only.");
        }
        
        _players.Add(player);
    }
    
    public void RemovePlayer(Player player)
    {
        Guard.Against.Null(player, nameof(player));
        
        if (!_players.Contains(player))
        {
            throw new InvalidOperationException("The player is not registered.");
        }
        
        _players.Remove(player);
    }
    
    public void AddReferee(Referee referee)
    {
        Guard.Against.Null(referee, nameof(referee));
        
        if (_referees.Count >= _requiredReferees)
        {
            throw new InvalidOperationException("The tournament has enough referees.");
        }
        
        if (_referees.Contains(referee))
        {
            throw new InvalidOperationException("The referee is already registered.");
        }
        
        _referees.Add(referee);
    }
    
    public void RemoveReferee(Referee referee)
    {
        Guard.Against.Null(referee, nameof(referee));
        
        if (!_referees.Contains(referee))
        {
            throw new InvalidOperationException("The referee is not registered.");
        }
        
        _referees.Remove(referee);
    }
    
    public void AddGame(Game.Game game)
    {
        Guard.Against.Null(game, nameof(game));
        
        if (_games.Contains(game))
        {
            throw new InvalidOperationException("The game is already registered.");
        }
        
        _games.Add(game);
    }
    
    public void RemoveGame(Game.Game game)
    {
        Guard.Against.Null(game, nameof(game));
        
        if (!_games.Contains(game))
        {
            throw new InvalidOperationException("The game is not registered.");
        }
        
        _games.Remove(game);
    }
    
    public void AddSponsor(Sponsor.Sponsor sponsor)
    {
        Guard.Against.Null(sponsor, nameof(sponsor));
        
        if (_sponsors.Contains(sponsor))
        {
            throw new InvalidOperationException("The sponsor is already registered.");
        }
        
        _sponsors.Add(sponsor);
    }
    
    public void RemoveSponsor(Sponsor.Sponsor sponsor)
    {
        Guard.Against.Null(sponsor, nameof(sponsor));
        
        if (!_sponsors.Contains(sponsor))
        {
            throw new InvalidOperationException("The sponsor is not registered.");
        }
        
        _sponsors.Remove(sponsor);
    }
    
    public bool CanRegister(Player player)
    {
        Guard.Against.Null(player, nameof(player));
        
        if (_players.Count == _maxPlayers)
        {
            return false;
        }
        
        if (_players.Contains(player))
        {
            return false;
        }
        
        return IsOpen || !string.IsNullOrEmpty(player.FideId);
    }
    
    public bool CanRegister(Referee referee)
    {
        Guard.Against.Null(referee, nameof(referee));
        
        if (_referees.Count == _requiredReferees)
        {
            return false;
        }
        
        return !_referees.Contains(referee);
    }
    
    public bool CanUnregister(Player player)
    {
        Guard.Against.Null(player, nameof(player));
        
        return _players.Contains(player);
    }
    
    public bool CanUnregister(Referee referee)
    {
        Guard.Against.Null(referee, nameof(referee));
        
        return _referees.Contains(referee);
    }
    
    public bool CanStart()
    {
        return _players.Count == _maxPlayers && _referees.Count == _requiredReferees;
    }
}
