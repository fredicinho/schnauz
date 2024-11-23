using Schnauz.Shared.Interfaces;

namespace Schnauz.CommandHandlers.Core;

public interface ICommandHandler
{
}

public interface ICommandHandler<TCommand> : ICommandHandler where TCommand : ICommand
{
    Task Execute(TCommand command/*, DbContext dbContext*/);
}
