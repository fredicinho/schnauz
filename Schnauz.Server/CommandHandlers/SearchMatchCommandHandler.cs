
using Schnauz.GrainInterfaces;
using Schnauz.Server.CommandHandlers.Core;
using Schnauz.Server.Websockets.Services;
using Schnauz.Shared.Commands;
using Schnauz.Shared.Dtos;

namespace Schnauz.Server.CommandHandlers;

public class SearchMatchCommandHandler(
    ILogger<SearchMatchCommandHandler> logger,
    IClusterClient clusterClient,
    ProfileService profileService
    ) : ICommandHandler<SearchMatchCommand>
{
    public async Task Execute(SearchMatchCommand command)
    {
        var playerGrain = clusterClient.GetGrain<IPlayer>(command.Username);
        await playerGrain.SearchMatch(command.Region);
        await profileService.SendProfileToUser(command.Username);
    }
}
