using Schnauz.Shared.Interfaces;

namespace Schnauz.QueryHandlers.Core
{
    public class QueryExecuter : IQueryExecutor, IQueryExecutorServer
    {
        private readonly IServiceProvider _provider;

        public QueryExecuter(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task<TResult> Send<TResult>(IQuery<TResult> query)
        {
            var handlerType = typeof(IQueryHandlerQuery<>).MakeGenericType(query.GetType());
            var queryHandler = (IQueryHandlerResult<TResult>)_provider.GetService(handlerType)!;
            return await queryHandler.RunQuery(query);
        }

        public async Task<object?> Send(IQuery query)
        {
            var handlerType = typeof(IQueryHandlerQuery<>).MakeGenericType(query.GetType());
            var queryHandler = (IQueryHandler)_provider.GetService(handlerType)!;
            return await queryHandler.RunQueryGeneric(query);
        }
    }
}
