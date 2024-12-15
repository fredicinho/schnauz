using Schnauz.GrainInterfaces;
using Schnauz.Server.CommandHandlers.Core;
using Schnauz.Server.Websockets.Services;
using Schnauz.Shared.Commands;

namespace Schnauz.Server.CommandHandlers;

public class CancelSearchMatchCommandHandler(
    ILogger<CancelSearchMatchCommandHandler> logger,
    IClusterClient clusterClient,
    ProfileService profileService
    ) : ICommandHandler<CancelSearchMatchCommand>
{
    public async Task Execute(CancelSearchMatchCommand command)
    {
        var playerGrain = clusterClient.GetGrain<IPlayer>(command.Username);
        await playerGrain.CancelSearch();
        await profileService.SendProfileToUser(command.Username);
    }
}
