using Schnauz.Shared.Commands;
using Schnauz.GrainInterfaces;
using Schnauz.Server.CommandHandlers.Core;
using Schnauz.Server.Services;

namespace Schnauz.Server.CommandHandlers;
public class SignInCommandHandler(
    IClusterClient clusterClient) : ICommandHandler<SignInCommand>
{
    public Task Execute(SignInCommand command)
    {
        // Create the grain if it doesnt exist yet.
        var player = clusterClient.GetGrain<IPlayer>(command.Username);
        return Task.CompletedTask;
    }
}
