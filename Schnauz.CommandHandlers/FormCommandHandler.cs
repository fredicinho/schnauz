using Schnauz.CommandHandlers.Core;
using Schnauz.Shared.Commands;
using System.Text.Json;

namespace Schnauz.CommandHandlers;
public class FormCommandHandler : ICommandHandler<FormCommand>
{
    public Task Execute(FormCommand command)
    {
        Console.WriteLine("COMMAND EXECUTED!");
        Console.WriteLine("Content: " + JsonSerializer.Serialize(command));
        return Task.CompletedTask;
    }
}
