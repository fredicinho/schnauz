using Schnauz.Shared.Dtos.enums;

namespace Schnauz.Shared.Dtos;

public class MatchDto
{
    public List<PlayerDto> Players { get; set; } = [];
    
    public MatchStateDto MatchStateDto { get; set; }
    
    public RoundDto CurrentRound { get; set; } = null!;
    
    /**
     * Ranking of players in the match.
     * We don't care right now if a player has the same score as another player.
     */
    public List<string> RankPlayers { get; set; } = [];
    
    public List<string> PlayersWhoRequestedNewMatch { get; set; } = [];
}