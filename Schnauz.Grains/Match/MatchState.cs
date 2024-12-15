using Schnauz.Shared.Dtos.enums;

namespace Schnauz.Grains.Match;

[Serializable]
public class MatchState
{
    public List<string> Players = [];

    public MatchStateDto MatchStatus = MatchStateDto.RUNNING;
    
    public List<string> RankPlayers = [];
    
    public int CurrentRoundNumber = 0;
    
    public List<string> PlayersWhoRequestedNewMatch = [];
}