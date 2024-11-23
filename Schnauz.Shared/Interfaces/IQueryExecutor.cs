namespace Schnauz.Shared.Interfaces;

public interface IQueryExecutor
{
    Task<TResult> Send<TResult>(IQuery<TResult> query);
}

public interface IQueryExecutorServer
{
    Task<object?> Send(IQuery query);
}
