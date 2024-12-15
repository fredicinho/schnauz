using Schnauz.Shared.Dtos.enums;

namespace Schnauz.Shared.Dtos;

public class PlayerDto
{
    public string UserName { get; set; } = String.Empty;
    
    public int NumberOfLifePoints { get; set; }
    
    public bool IsOut { get; set; }
    
    public PlayerActionDto LastAction { get; set; }
    
    /**
     * If the round is over, the number of card points will be shown on each player
     */
    public double? NumberOfCardPoints { get; set; }
    
    /**
     * The cards that the player has in his hand. This is only available
     * when the round is over.
     */
    public List<CardDto> CardsInHand { get; set; } = [];
}