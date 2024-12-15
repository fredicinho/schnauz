
using Schnauz.GrainInterfaces;
using Schnauz.Server.CommandHandlers.Core;
using Schnauz.Shared.Commands;

namespace Schnauz.Server.CommandHandlers;

public class NextRoundCommandHandler(
    ILogger<NextRoundCommandHandler> logger,
    IClusterClient clusterClient
    ) : ICommandHandler<NextRoundCommand>
{
    public async Task Execute(NextRoundCommand command)
    {
        var playerGrain = clusterClient.GetGrain<IPlayer>(command.Username);
        var matchId = await playerGrain.GetCurrentMatchId();
        if (!matchId.HasValue)
        {
            throw new InvalidOperationException("Player is not in a match.");
        }
        var matchGrain = clusterClient.GetGrain<IMatch>(matchId.Value);
        await matchGrain.NextRound();
        logger.LogInformation("Next round started for match {MatchId}", matchId);
    }
}
