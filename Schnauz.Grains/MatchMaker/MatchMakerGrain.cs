using Microsoft.Extensions.Logging;
using Schnauz.GrainInterfaces;

namespace Schnauz.Grains.MatchMaker;

public class MatchMakerGrain(
    ILogger<IMatchMaker> logger,
    [PersistentState("matchMaker", "schnauz")] IPersistentState<MatchMakerState> matchMaker,
    IGrainFactory grainFactory)
    : Grain, IMatchMaker
{
    private const int MaxPlayersDoDeque = 7;

    public async ValueTask SearchMatch(string userName)
    {
        matchMaker.State.PlayersSearchingGame.Enqueue(userName);
        await matchMaker.WriteStateAsync();
        logger.LogInformation("Player {userName} is searching for a game", userName);
    }

    public async ValueTask CancelSearch(string userName)
    {
        matchMaker.State.PlayersSearchingGame = new(matchMaker.State.PlayersSearchingGame.Where(player => player != userName));
        await matchMaker.WriteStateAsync();
        logger.LogInformation("Player {userName} has canceled the search for a game", userName);
    }
    
    public async ValueTask MatchPlayers()
    {
        if (matchMaker.State.PlayersSearchingGame.Count >= 2)
        {
            logger.LogInformation("Enough Players to create a match. Dequeuing players");
            var numberOfPlayersToDeque = matchMaker.State.PlayersSearchingGame.Count > MaxPlayersDoDeque ? MaxPlayersDoDeque : matchMaker.State.PlayersSearchingGame.Count;
            var players = Enumerable
                .Range(0, numberOfPlayersToDeque)
                .Select(_ => matchMaker.State.PlayersSearchingGame.Dequeue())
                .ToList();
            await matchMaker.WriteStateAsync();
            
            var match = grainFactory.GetGrain<IMatch>(Guid.NewGuid());
            await match.Create(players);
            logger.LogInformation("Match created with players: {players}", players);
        }
    }
}