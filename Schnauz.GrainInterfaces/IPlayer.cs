using Schnauz.Shared.Dtos;
using Schnauz.Shared.Dtos.enums;

namespace Schnauz.GrainInterfaces;

public interface IPlayer: IGrainWithStringKey
{
    ValueTask<int> GetNumberOfLives();
    
    [Transaction(TransactionOption.Join)]
    ValueTask Reset();

    [Transaction(TransactionOption.Join)]
    ValueTask RemoveLife();
    
    ValueTask<bool> GetIsOut();
    
    [Transaction(TransactionOption.Join)]
    ValueTask SetCardsOnHand(List<CardDto> cards);
    
    ValueTask<List<CardDto>> GetCardsOnHand();
    
    ValueTask<UserStateDto> GetUserState();
    
    [Transaction(TransactionOption.Create)]
    ValueTask SearchMatch(RegionDto region);
    
    [Transaction(TransactionOption.Create)]
    ValueTask CancelSearch();
    
    [Transaction(TransactionOption.Join)]
    ValueTask SetUserState(UserStateDto userState);
    
    [Transaction(TransactionOption.Join)]
    ValueTask SetCurrentMatchId(Guid matchId);

    ValueTask<Guid?> GetCurrentMatchId();
    
    ValueTask<PlayerActionDto> GetLastAction();
    
    [Transaction(TransactionOption.Join)]
    ValueTask SetLastAction(PlayerActionDto playerActionDto);
}