using Orleans;
using Schnauz.Shared.Dtos.enums;

namespace Schnauz.Shared.Dtos;

[GenerateSerializer]
public class CardDto
{
    [Id(0)]
    public SuitDto Suit { get; set; }
    
    [Id(1)]
    public CardRankDto CardRank { get; set; }

    public override string ToString() => $"{Suit}-{CardRank}";
}