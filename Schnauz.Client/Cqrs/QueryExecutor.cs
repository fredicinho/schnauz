using Schnauz.Shared;
using Schnauz.Shared.Interfaces;

namespace Schnauz.Client.Cqrs;

public class QueryExecutor : IQueryExecutor
{
    private readonly CqrsHttpClient _httpClient;

    public QueryExecutor(CqrsHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<TResult> Send<TResult>(IQuery<TResult> query)
    {
        return await RunQuery(query);
    }

    private async Task<TResult> RunQuery<TResult>(IQuery<TResult> query)
    {
        return await _httpClient.PostQueryAsync<TResult>(new CommandQueryContract(query));
    }
}
