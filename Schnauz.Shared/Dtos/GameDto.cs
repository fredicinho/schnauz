using Orleans;
using Schnauz.Shared.Dtos.enums;

namespace Schnauz.Shared.Dtos;

[GenerateSerializer]
public class GameMatchDto
{
    [Id(0)]
    public MatchStateDto MatchState { get; set; }
    
    [Id(1)]
    public List<string> RankedPlayers { get; set; } = [];
    
    [Id(2)]
    public GameRoundDto CurrentRound { get; set; }
    
    [Id(3)]
    public List<string> PlayersWhoRequestedNewMatch { get; set; } = [];
}

[GenerateSerializer]
public class GameRoundDto
{
    [Id(0)]
    public List<CardDto> CardsOnTable { get; set; } = [];
    
    [Id(1)]
    public List<GamePlayerDto> Players { get; set; } = [];
    
    [Id(2)]
    public RoundStateDto RoundState { get; set; }
    
    [Id(3)]
    public string ActivePlayer { get; set; }
    
    [Id(4)]
    public List<string> PlayersLost { get; set; } = [];
}

[GenerateSerializer]
public class GamePlayerDto
{
    [Id(0)]
    public string UserName { get; set; } = String.Empty;
    
    [Id(1)]
    public UserStateDto UserState { get; set; }
    
    [Id(2)]
    public List<CardDto> CardsInHand { get; set; } = [];
    
    [Id(3)]
    public int NumberOfLifePoints { get; set; }
    
    [Id(4)]
    public double? NumberOfCardPoints { get; set; }
    
    [Id(5)]
    public bool IsOut { get; set; }
    
    [Id(6)]
    public PlayerActionDto LastAction { get; set; }
}