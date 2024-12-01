namespace Schnauz.Shared.Dtos;

public class MatchDto
{
    public List<PlayerDto> Players { get; set; } = [];
    
    public string NextTurnUserName { get; set; } = String.Empty;
    
    public List<CardDto> CardsOnHand { get; set; } = [];
    
    public List<CardDto> CardsOnTable { get; set; } = [];

    public bool IsRoundOver { get; set; } = false;

    public bool IsMatchOver { get; set; } = false;
}