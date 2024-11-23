namespace Schnauz.Shared.Interfaces;

public interface ICommandExecutor
{
    Task<bool> Send(ICommand cqrsCommand);
}
