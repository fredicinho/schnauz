using Schnauz.Shared.Dtos.enums;

namespace Schnauz.Shared.Dtos;

public class CardDto
{
    public SuitDto Suit { get; set; }
    
    public CardRankDto CardRank { get; set; }

    public override string ToString() => $"{CardRank} of {Suit}";
}