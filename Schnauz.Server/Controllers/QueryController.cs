using Schnauz.Shared;
using Schnauz.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Schnauz.Server.Controllers;

[ApiController]
public class QueryController(IQueryExecutorServer queryExecuterExecutorServer/*, IAuthorizationProvider authorizationProvider*/) : ControllerBase
{
    private readonly IQueryExecutorServer _queryExecuterExecutorServer = queryExecuterExecutorServer;
    //private readonly IAuthorizationProvider _authorizationProvider = authorizationProvider;

    [HttpPost(ApiPaths.Query)]
    public async Task<JsonResult> Post(CommandQueryContract commandQuery)
    {
        var query = (IQuery)commandQuery.GetObject();
        //_authorizationProvider.Authorize(query.GetType());
        return new JsonResult(await _queryExecuterExecutorServer.Send(query));
    }
}
