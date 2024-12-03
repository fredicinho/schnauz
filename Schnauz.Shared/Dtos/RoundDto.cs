using Schnauz.Shared.Dtos.enums;

namespace Schnauz.Shared.Dtos;

public class RoundDto
{
    public RoundStateDto RoundState { get; set; }
    
    public string NextTurnUserName { get; set; } = String.Empty;
    
    /**
     * Players who lost that round
     */
    public List<string> LostPlayers { get; set; } = [];
    
    public List<CardDto> CardsOnHand { get; set; } = [];
    
    public List<CardDto> CardsOnTable { get; set; } = [];
}