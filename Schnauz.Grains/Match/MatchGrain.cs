using Microsoft.Extensions.Logging;
using Schnauz.GrainInterfaces;
using Schnauz.Grains.CardDealer;
using Schnauz.Grains.Services;
using Schnauz.Shared.Dtos;
using Schnauz.Shared.Dtos.enums;

namespace Schnauz.Grains.Match;

public class MatchGrain(
    ILogger<MatchGrain> logger,
    IGrainFactory grainFactory,
    [PersistentState("match", "schnauz")] IPersistentState<MatchState> matchState,
    SendGameMatchService sendGameMatchService
    ): Grain, IMatch
{
    public async ValueTask Create(List<string> players)
    {
        await InitiateNewMatch(players);
    }

    public async ValueTask OnUpdateOfRoundState(CardDealerRoundDto roundDto)
    {
        if (roundDto.RoundState == RoundStateDto.FINISHED)
        {
            await UpdateMatchAndPlayers(roundDto);
        }

        await NotifyPlayersWithMatchState(roundDto);
    }

    public async ValueTask<GameMatchDto> GetMatch()
    {
        var cardDealerGrain = grainFactory.GetGrain<ICardDealer>(this.GetGrainId().GetGuidKey());
        var roundDto = await cardDealerGrain.GetRound();
        return await CreateGameMatchDto(roundDto);
    }

    public async ValueTask NextRound()
    {
        matchState.State.CurrentRoundNumber++;
        await matchState.WriteStateAsync();
        var cardDealerGrain = grainFactory.GetGrain<ICardDealer>(this.GetGrainId().GetGuidKey());
        var gameRoundDto = await cardDealerGrain.InitiateRound(matchState.State.Players, matchState.State.CurrentRoundNumber % matchState.State.Players.Count);
        await NotifyPlayersWithMatchState(gameRoundDto);
    }

    public async ValueTask Reset(string userName)
    {
        ThrowIfPlayerIsNotInMatch(userName);
        if (matchState.State.PlayersWhoRequestedNewMatch.Contains(userName))
        {
            return;
        }
        matchState.State.PlayersWhoRequestedNewMatch.Add(userName);
        await matchState.WriteStateAsync();
        
        if (matchState.State.PlayersWhoRequestedNewMatch.Count == matchState.State.Players.Count)
        {
            await InitiateNewMatch(matchState.State.Players);
        }
        else
        {
            var cardDealerGrain = grainFactory.GetGrain<ICardDealer>(this.GetGrainId().GetGuidKey());
            var roundDto = await cardDealerGrain.GetRound();
            await NotifyPlayersWithMatchState(roundDto);
        }
    }
    
    private void ThrowIfPlayerIsNotInMatch(string userName)
    {
        if (!matchState.State.Players.Contains(userName))
        {
            throw new InvalidOperationException($"Player {userName} is not in the match");
        }
    }

    private async ValueTask UpdateMatchAndPlayers(CardDealerRoundDto roundDto)
    {
        await RemoveLifeFromLostPlayers(roundDto);
        if (await OnlyOnePlayerLeft())
        { 
            matchState.State.MatchStatus = MatchStateDto.FINISHED;
        }
        await matchState.WriteStateAsync();
    }

    private async ValueTask RemoveLifeFromLostPlayers(CardDealerRoundDto roundDto)
    {
        foreach (var lostPlayer in roundDto.PlayersLost)
        {
            var playerGrain = grainFactory.GetGrain<IPlayer>(lostPlayer);
            await playerGrain.RemoveLife();
        }
    }

    private async ValueTask<bool> OnlyOnePlayerLeft()
    {
        var playerStatuses = await Task.WhenAll(
            matchState.State.Players
                .Select(player => grainFactory.GetGrain<IPlayer>(player))
                .Select(async player => await player.GetIsOut())
        );

        return playerStatuses.Count(playerIsOut => !playerIsOut) == 1;
    }

    private async ValueTask NotifyPlayersWithMatchState(CardDealerRoundDto roundDto)
    {
        var gameMatchDto = await CreateGameMatchDto(roundDto);
        await sendGameMatchService.SendGameMatch(gameMatchDto);
    }

    private async ValueTask<GameMatchDto> CreateGameMatchDto(CardDealerRoundDto roundDto)
    {
        var players = await Task.WhenAll(
            matchState.State.Players
                .Select(player => grainFactory.GetGrain<IPlayer>(player))
                .Select(async player => new GamePlayerDto
                    {
                        UserName = player.GetGrainId().Key.ToString()!,
                        CardsInHand = await player.GetCardsOnHand(),
                        NumberOfLifePoints = await player.GetNumberOfLives(),
                        NumberOfCardPoints = CardHelper.CalculateCardPoints(await player.GetCardsOnHand()),
                        IsOut = await player.GetIsOut(),
                        LastAction = await player.GetLastAction(),
                        UserState = await player.GetUserState()
                    }
                )
        );
        
        return new GameMatchDto
        {
            MatchState = matchState.State.MatchStatus,
            RankedPlayers = matchState.State.RankPlayers,
            PlayersWhoRequestedNewMatch = matchState.State.PlayersWhoRequestedNewMatch,
            CurrentRound = new GameRoundDto
            {
                CardsOnTable = roundDto.CardsOnTable,
                Players = players.ToList(),
                PlayersLost = roundDto.PlayersLost,
                RoundState = roundDto.RoundState,
                ActivePlayer = roundDto.ActivePlayer
            },
        };
    }

    private async ValueTask InitiateNewMatch(List<string> players)
    {
        matchState.State.Players = players;
        matchState.State.RankPlayers = players.OrderBy(player => player).ToList();
        matchState.State.PlayersWhoRequestedNewMatch.Clear();
        matchState.State.MatchStatus = MatchStateDto.RUNNING;
        matchState.State.CurrentRoundNumber = 0;
        await matchState.WriteStateAsync();

        foreach (var player in players)
        {
            var playerGrain = grainFactory.GetGrain<IPlayer>(player);
            await playerGrain.Reset();
            await playerGrain.SetUserState(UserStateDto.PARTICIPATING_IN_MATCH);
            await playerGrain.SetCurrentMatchId(this.GetGrainId().GetGuidKey());
        }

        var cardDealer = grainFactory.GetGrain<ICardDealer>(this.GetGrainId().GetGuidKey());
        var roundDto = await cardDealer.InitiateRound(matchState.State.Players, matchState.State.CurrentRoundNumber);
        await NotifyPlayersWithMatchState(roundDto);
    }
}