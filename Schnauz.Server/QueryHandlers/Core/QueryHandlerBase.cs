using Schnauz.Shared.Interfaces;

namespace Schnauz.Server.QueryHandlers.Core
{
    //public abstract class QueryHandlerDbBase<TQuery, TResult, TEntityRoot> : QueryHandlerBase<TQuery, TResult>
    //    where TQuery : IQuery<TResult>
    //    where TEntityRoot : class // TODO: Change to entity base if ever existing
    //{
    //    private readonly DbContext _context;

    //    protected QueryHandlerDbBase(DbContext context)
    //    {
    //        _context = context;
    //    }

    //    protected IQueryable<TEntityRoot> DbSet => _context.Set<TEntityRoot>();

    //    protected IQueryable<TEntity> GetSet<TEntity>() where TEntity : class => _context.Set<TEntity>();
    //}

    public abstract class QueryHandlerBase<TQuery, TResult> : IQueryHandlerQuery<TQuery>, IQueryHandlerResult<TResult>
        where TQuery : IQuery<TResult>
    {
        protected abstract Task<TResult> Execute(TQuery query);

        public async Task<TResult> RunQuery(IQuery query)
        {
            return await Execute((TQuery)query);
        }

        public async Task<object?> RunQueryGeneric(IQuery query)
        {
            return await Execute((TQuery)query);
        }
    }
}
