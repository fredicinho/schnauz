using Schnauz.Shared.Interfaces;

namespace Schnauz.QueryHandlers.Core;

public interface IQueryHandler
{
    Task<object?> RunQueryGeneric(IQuery query);
}

public interface IQueryHandlerQuery<TQuery> where TQuery : IQuery
{
}


public interface IQueryHandlerResult<TResult> : IQueryHandler
{
    Task<TResult> RunQuery(IQuery query);
}
