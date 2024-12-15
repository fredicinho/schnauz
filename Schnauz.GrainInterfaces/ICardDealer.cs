using Schnauz.Grains.CardDealer;
using Schnauz.Shared.Dtos;

namespace Schnauz.GrainInterfaces;

public interface ICardDealer: IGrainWithGuidKey
{
    [Transaction(TransactionOption.Join)]
    ValueTask<CardDealerRoundDto> InitiateRound(List<string> players, int startingPlayerIndex);
    
    [Transaction(TransactionOption.Create)]
    ValueTask Shove(string userName);
    
    ValueTask<CardDealerRoundDto> GetRound();
    
    [Transaction(TransactionOption.Create)]
    ValueTask Close(string userName);
    
    [Transaction(TransactionOption.Create)]
    ValueTask ChangeCard(string userName, CardDto cardInHand, CardDto cardOnTable);

    [Transaction(TransactionOption.Create)]
    ValueTask ChangeAllCards(string userName);
}