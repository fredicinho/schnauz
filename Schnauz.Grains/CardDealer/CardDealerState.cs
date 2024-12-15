using Schnauz.Shared.Dtos;
using Schnauz.Shared.Dtos.enums;

namespace Schnauz.Grains.CardDealer;

[Serializable]
public class CardDealerState
{
    public List<CardDto> CardsOnTable = [];
    
    public List<string> Players = [];
    
    public int IndexOfCurrentPlayer = 0;
    
    public RoundStateDto RoundState = RoundStateDto.RUNNING;
    
    public List<string> PlayersLost = [];
}