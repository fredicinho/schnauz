using Microsoft.Extensions.Logging;
using Schnauz.GrainInterfaces;
using Schnauz.Grains.MatchMaker;
using Schnauz.Shared.Dtos;
using Schnauz.Shared.Dtos.enums;

namespace Schnauz.Grains.Player;

public class PlayerGrain(
    ILogger<IPlayer> logger, 
    [PersistentState("player", "schnauz")] IPersistentState<PlayerState> playerState, 
    IGrainFactory grainFactory
    ) : Grain, IPlayer
{
    public async ValueTask Reset()
    {
        playerState.State.CardsOnHand = new();
        playerState.State.NumberOfLifes = 3;
        playerState.State.IsOut = false;
        playerState.State.LastAction = PlayerActionDto.NO_ACTION;
        await playerState.WriteStateAsync();
        logger.LogInformation("Player {userName} has been reset", this.GetGrainId().Key.ToString());
    }

    public async ValueTask RemoveLife()
    {
        playerState.State.NumberOfLifes--;
        if (playerState.State.NumberOfLifes == -1)
        {
            playerState.State.IsOut = true;
            logger.LogInformation("Player {userName} is out", this.GetGrainId().Key.ToString());
        }

        await playerState.WriteStateAsync();
        logger.LogInformation("Player {userName} has lost a life", this.GetGrainId().Key.ToString());
    }

    public ValueTask<int> GetNumberOfLives()
    {
        return ValueTask.FromResult(playerState.State.NumberOfLifes);
    }

    public ValueTask<bool> GetIsOut()
    {
        return ValueTask.FromResult(playerState.State.IsOut);
    }

    public async ValueTask SetCardsOnHand(List<CardDto> cards)
    {
        playerState.State.CardsOnHand = cards;
        await playerState.WriteStateAsync();
        logger.LogInformation("Player {userName} has received new cards", this.GetGrainId().Key.ToString());
    }

    public ValueTask<List<CardDto>> GetCardsOnHand()
    {
        return ValueTask.FromResult(playerState.State.CardsOnHand);
    }

    public ValueTask<UserStateDto> GetUserState()
    {
        return ValueTask.FromResult(playerState.State.UserState);
    }

    public async ValueTask SearchMatch(RegionDto region)
    {
        if (playerState.State.UserState != UserStateDto.SIGNED_IN)
        {
            throw new InvalidOperationException("Player can't search for a match if he is not in the signed in state");
        }
        playerState.State.UserState = UserStateDto.SEARCHING_A_MATCH;
        playerState.State.SelectedRegion = region;
        await playerState.WriteStateAsync();
        var matchMaker = grainFactory.GetGrain<IMatchMaker>(MatchMakerKey.GetKey(region));
        await matchMaker.SearchMatch(this.GetGrainId().Key.ToString()!);
    }

    public async ValueTask CancelSearch()
    {
        if (playerState.State.UserState != UserStateDto.SEARCHING_A_MATCH)
        {
            throw new InvalidOperationException("Player can't cancel a search for a match if he is not in the corresponding state");
        }
        playerState.State.UserState = UserStateDto.SIGNED_IN;
        await playerState.WriteStateAsync();
        var matchMaker = grainFactory.GetGrain<IMatchMaker>(MatchMakerKey.GetKey(playerState.State.SelectedRegion));
        await matchMaker.CancelSearch(this.GetGrainId().ToString());
    }

    public async ValueTask SetUserState(UserStateDto userState)
    {
        playerState.State.UserState = userState;
        await playerState.WriteStateAsync();
    }
    
    public async ValueTask SetCurrentMatchId(Guid matchId)
    {
        playerState.State.CurrentMatchId = matchId;
        await playerState.WriteStateAsync();
    }
    
    public ValueTask<Guid?> GetCurrentMatchId()
    {
        return ValueTask.FromResult(playerState.State.CurrentMatchId);
    }

    public ValueTask<PlayerActionDto> GetLastAction()
    {
        return ValueTask.FromResult(playerState.State.LastAction);
    }

    public async ValueTask SetLastAction(PlayerActionDto playerActionDto)
    {
        playerState.State.LastAction = playerActionDto;
        await playerState.WriteStateAsync();
    }
}