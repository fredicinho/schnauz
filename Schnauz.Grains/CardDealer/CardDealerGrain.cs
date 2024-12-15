using Microsoft.Extensions.Logging;
using Schnauz.GrainInterfaces;
using Schnauz.Shared.Dtos;
using Schnauz.Shared.Dtos.enums;

namespace Schnauz.Grains.CardDealer;

public class CardDealerGrain(
    ILogger<CardDealerGrain> logger,
    IGrainFactory grainFactory,
    [PersistentState("cardDealer", "schnauz")] IPersistentState<CardDealerState> cardDealerState
): Grain, ICardDealer
{
    public async ValueTask<CardDealerRoundDto> InitiateRound(List<string> players, int startingPlayerIndex)
    {
        await InitiateState(players, startingPlayerIndex);
        await DealCards();
        logger.LogInformation("Round initiated and cards distributed");
        return await CreateRoundDto();
    }

    public async ValueTask Shove(string userName)
    {
        ThrowIfActionIsNotValid(userName);
        var playerGrain = grainFactory.GetGrain<IPlayer>(userName);
        await playerGrain.SetLastAction(PlayerActionDto.SHOVE);
        await ShiftNext();
    }

    public async ValueTask<CardDealerRoundDto> GetRound()
    {
        return await CreateRoundDto();
    }

    public async ValueTask ChangeCard(string userName, CardDto cardInHand, CardDto cardOnTable)
    {
        ThrowIfActionIsNotValid(userName);
        ThrowIfCardChangeIsNotValid(userName, cardInHand, cardOnTable);
        
        var newCardsOnTable = cardDealerState.State.CardsOnTable
            .Where(card => card.ToString() != cardOnTable.ToString())
            .ToList();
        newCardsOnTable.Add(cardInHand);
        cardDealerState.State.CardsOnTable = newCardsOnTable;
        await cardDealerState.WriteStateAsync();
        
        var playerGrain = grainFactory.GetGrain<IPlayer>(userName);
        var cardsOnHand = (await playerGrain.GetCardsOnHand())
            .Where(card => card.ToString() != cardInHand.ToString())
            .ToList();
        cardsOnHand.Add(cardOnTable);
        await playerGrain.SetCardsOnHand(cardsOnHand);
        
        await playerGrain.SetLastAction(GetPlayerActionForCardChange(cardsOnHand));
        await ShiftNext();
    }

    public async ValueTask ChangeAllCards(string userName)
    {
        ThrowIfActionIsNotValid(userName);
        var playerGrain = grainFactory.GetGrain<IPlayer>(userName);
        var cardsOnTable = cardDealerState.State.CardsOnTable;
        cardDealerState.State.CardsOnTable = await playerGrain.GetCardsOnHand();
        await playerGrain.SetCardsOnHand(cardsOnTable);
        await playerGrain.SetLastAction(GetPlayerActionForAllCardChange(cardsOnTable));
        await ShiftNext();
    }
    
    public async ValueTask Close(string userName)
    {
        ThrowIfActionIsNotValid(userName);
        var playerGrain = grainFactory.GetGrain<IPlayer>(userName);
        await playerGrain.SetLastAction(PlayerActionDto.CLOSING);
        await ShiftNext();
    }
    
    public ValueTask<string> GetActivePlayer()
    {
        return ValueTask.FromResult(cardDealerState.State.Players[cardDealerState.State.IndexOfCurrentPlayer]);
    }

    private async ValueTask InitiateState(List<string> players, int startingPlayerIndex)
    {
        cardDealerState.State.CardsOnTable = [];
        cardDealerState.State.Players = players;
        cardDealerState.State.IndexOfCurrentPlayer = startingPlayerIndex;
        cardDealerState.State.RoundState = RoundStateDto.RUNNING;
        cardDealerState.State.PlayersLost = [];
        cardDealerState.State.Players.ForEach(player =>
        {
            var playerGrain = grainFactory.GetGrain<IPlayer>(player);
            playerGrain.SetLastAction(PlayerActionDto.NO_ACTION);
        });
        await cardDealerState.WriteStateAsync();
    }

    private async ValueTask DealCards()
    {
        var shuffledDeck = CardHelper.ShuffleDeck(CardHelper.GenerateDeck());
        var playerGrains = cardDealerState.State.Players
            .Select(player => grainFactory.GetGrain<IPlayer>(player));
        foreach (var playerGrain in playerGrains)
        {
            (shuffledDeck, var cards) = CardHelper.DealCards(shuffledDeck);
            await playerGrain.SetCardsOnHand(cards);
        }
        
        var (_, cardsForTable) = CardHelper.DealCards(shuffledDeck);
        cardDealerState.State.CardsOnTable = cardsForTable;
        await cardDealerState.WriteStateAsync();
    }
    
    private async ValueTask ShiftNext()
    {
        if (await RoundIsOver())
        {
            cardDealerState.State.RoundState = RoundStateDto.FINISHED;
            cardDealerState.State.PlayersLost = await GetPlayersWhoLostRound();
            await cardDealerState.WriteStateAsync();
        }
        else
        {
            cardDealerState.State.IndexOfCurrentPlayer = GetIndexOfNextPlayer();
            await cardDealerState.WriteStateAsync();
        }
        var gameRoundDto = await CreateRoundDto();
        var matchGrain = grainFactory.GetGrain<IMatch>(this.GetGrainId().GetGuidKey());
        await matchGrain.OnUpdateOfRoundState(gameRoundDto);
    }
    
    private async ValueTask<CardDealerRoundDto> CreateRoundDto()
    {
        return new CardDealerRoundDto
        {
            ActivePlayer = cardDealerState.State.Players[cardDealerState.State.IndexOfCurrentPlayer],
            CardsOnTable = cardDealerState.State.CardsOnTable,
            PlayersLost = await GetPlayersWhoLostRound(),
            RoundState = cardDealerState.State.RoundState
        };
    }
    
    private void ThrowIfActionIsNotValid(string userName)
    {
        if (cardDealerState.State.Players[cardDealerState.State.IndexOfCurrentPlayer] != userName)
        {
            throw new InvalidOperationException(string.Concat("User ", userName, " is not the active player"));
        }
        
        if (cardDealerState.State.RoundState != RoundStateDto.RUNNING)
        {
            throw new InvalidOperationException("Round is not running");
        }
    }

    private async void ThrowIfCardChangeIsNotValid(string userName, CardDto cardInHand, CardDto cardOnTable)
    {
        if (cardDealerState.State.CardsOnTable.Count(card => card.ToString() == cardOnTable.ToString()) != 1)
        {
            throw new InvalidOperationException("Card is not on the table");
        }
        
        var playerGrain = grainFactory.GetGrain<IPlayer>(userName);
        if ((await playerGrain.GetCardsOnHand()).Count(card => card.ToString() == cardInHand.ToString()) != 1)
        {
            throw new InvalidOperationException("Card is not in players hand");
        }
    }
    
    /**
     * Round is over when:
     * - current player has Schnauz (31 points) or Fire (33 Points)
     * - next players last action is closing
     */
    private async ValueTask<bool> RoundIsOver()
    {
        var playerGrain = grainFactory.GetGrain<IPlayer>(cardDealerState.State.Players[cardDealerState.State.IndexOfCurrentPlayer]);
        if (CardHelper.CalculateCardPoints(await playerGrain.GetCardsOnHand()) >= 31)
        {
            return true;
        }
        
        var nextPlayerGrain = grainFactory.GetGrain<IPlayer>(cardDealerState.State.Players[GetIndexOfNextPlayer()]);
        
        return await nextPlayerGrain.GetLastAction() == PlayerActionDto.CLOSING;
    }
    
    private int GetIndexOfNextPlayer()
    {
        return (cardDealerState.State.IndexOfCurrentPlayer + 1) % cardDealerState.State.Players.Count;
    }
    
    private async ValueTask<List<string>> GetPlayersWhoLostRound()
    {
        var playersWithCardPoints = new Dictionary<string, double>();
        foreach (var player in cardDealerState.State.Players)
        {
            var playerGrain = grainFactory.GetGrain<IPlayer>(player);
            if (await playerGrain.GetIsOut())
            {
                continue;
            }
            
            var cardPoints = CardHelper.CalculateCardPoints(await playerGrain.GetCardsOnHand());
            playersWithCardPoints.Add(player, cardPoints);
        }
        
        var winners = playersWithCardPoints
            .GroupBy(player => player.Value)
            .OrderByDescending(group => group.Key)
            .First()
            .Select(player => player.Key)
            .ToList();
        
        // If all players have the same amount of points, all lost
        if (winners.Count == cardDealerState.State.Players.Count)
        {
            return cardDealerState.State.Players;
        }
        
        return cardDealerState.State.Players
            .Where(player => !winners.Contains(player))
            .ToList();
    }
    
    private PlayerActionDto GetPlayerActionForCardChange(List<CardDto> cardsOnHand)
    {
        switch (CardHelper.CalculateCardPoints(cardsOnHand))
        {
            case 31:
                return PlayerActionDto.SCHNAUZ;
            case 33:
                return PlayerActionDto.FIRE;
            default:
                return PlayerActionDto.CHANGED_CARD;
        }
    }
    
    private PlayerActionDto GetPlayerActionForAllCardChange(List<CardDto> cardsOnHand)
    {
        switch (CardHelper.CalculateCardPoints(cardsOnHand))
        {
            case 31:
                return PlayerActionDto.SCHNAUZ;
            case 33:
                return PlayerActionDto.FIRE;
            default:
                return PlayerActionDto.CHANGED_ALL_CARDS;
        }
    }
}