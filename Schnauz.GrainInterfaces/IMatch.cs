using Schnauz.Grains.CardDealer;
using Schnauz.Shared.Dtos;

namespace Schnauz.GrainInterfaces;

public interface IMatch: IGrainWithGuidKey
{
    [Transaction(TransactionOption.Join)]
    ValueTask Create(List<string> players);

    [Transaction(TransactionOption.Join)]
    ValueTask OnUpdateOfRoundState(CardDealerRoundDto roundDto);
    
    ValueTask<GameMatchDto> GetMatch();
    
    [Transaction(TransactionOption.Create)]
    ValueTask NextRound();
    
    [Transaction(TransactionOption.Create)]
    ValueTask Reset(string userName);
}