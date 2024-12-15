using Orleans;
using Schnauz.Shared.Dtos;
using Schnauz.Shared.Dtos.enums;

namespace Schnauz.Grains.CardDealer;

[GenerateSerializer]
public class CardDealerRoundDto
{
    [Id(0)]
    public List<CardDto> CardsOnTable { get; set; } = [];
    
    [Id(1)]
    public RoundStateDto RoundState { get; set; }
    
    [Id(2)]
    public string ActivePlayer { get; set; }
    
    [Id(3)]
    public List<string> PlayersLost { get; set; } = [];
}