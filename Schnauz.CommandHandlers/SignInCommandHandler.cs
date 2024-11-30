using Schnauz.CommandHandlers.Core;
using Schnauz.Shared.Commands;
using System.Text.Json;

namespace Schnauz.CommandHandlers;
public class SignInCommandHandler : ICommandHandler<SignInCommand>
{
    public Task Execute(SignInCommand command)
    {
        Console.WriteLine("COMMAND EXECUTED!");
        Console.WriteLine("Content: " + JsonSerializer.Serialize(command));
        return Task.CompletedTask;
    }
}
