using Schnauz.Shared.Dtos.enums;

namespace Schnauz.Shared.Dtos;

public class PlayerDto
{
    public string UserName { get; set; } = String.Empty;
    
    public int NumberOfLifePoints { get; set; }
    
    /**
     * If the round is over, the number of card points will be shown on each player
     */
    public int? NumberOfCardPoints { get; set; }
    
    public bool IsOut { get; set; }
    
    public PlayerActionDto LastAction { get; set; }
}