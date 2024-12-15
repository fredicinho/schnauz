using Schnauz.GrainInterfaces;
using Schnauz.Server.CommandHandlers.Core;
using Schnauz.Shared.Commands;

namespace Schnauz.Server.CommandHandlers;

public class CloseCommandHandler(IClusterClient clusterClient) : ICommandHandler<CloseCommand>
{
    public async Task Execute(CloseCommand command)
    {
        var player = clusterClient.GetGrain<IPlayer>(command.Username);
        var matchId = await player.GetCurrentMatchId();
        if (!matchId.HasValue)
        {
            throw new InvalidOperationException("Player is not in a match.");
        }
        
        var cardDealerGrain = clusterClient.GetGrain<ICardDealer>(matchId.Value);
        await cardDealerGrain.Close(player.GetGrainId().Key.ToString()!);
    }
}
