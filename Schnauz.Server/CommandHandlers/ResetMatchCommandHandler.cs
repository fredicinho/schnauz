
using Schnauz.GrainInterfaces;
using Schnauz.Server.CommandHandlers.Core;
using Schnauz.Shared.Commands;

namespace Schnauz.Server.CommandHandlers;

public class ResetMatchCommandHandler(
    ILogger<ResetMatchCommandHandler> logger,
    IClusterClient clusterClient
    ) : ICommandHandler<ResetMatchCommand>
{
    public async Task Execute(ResetMatchCommand command)
    {
        var playerGrain = clusterClient.GetGrain<IPlayer>(command.Username);
        var matchId = await playerGrain.GetCurrentMatchId();
        if (!matchId.HasValue)
        {
            throw new InvalidOperationException("Player is not in a match.");
        }
        var matchGrain = clusterClient.GetGrain<IMatch>(matchId.Value);
        await matchGrain.Reset(command.Username);
        logger.LogInformation($"Match reset by {command.Username}");
    }
}
