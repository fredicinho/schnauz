using Schnauz.Shared;
using Schnauz.Shared.Dtos;
using Schnauz.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Schnauz.Server.Controllers;

[ApiController]
public class CommandController : ControllerBase
{
    private readonly ICommandExecutor _commandExecutor;
    //private readonly IAuthorizationProvider _authorizationProvider;
    private readonly ValidationProvider _validationProvider;

    public CommandController(ICommandExecutor commandExecutor,
        //IAuthorizationProvider authorizationProvider,
        ValidationProvider validationProvider
        )
    {
        _commandExecutor = commandExecutor;
        //_authorizationProvider = authorizationProvider;
        _validationProvider = validationProvider;
    }

    [HttpPost(ApiPaths.Command)]
    public async Task<ActionResult> Post(CommandQueryContract commandQuery)
    {
        var command = (ICommand)commandQuery.GetObject();
        //_authorizationProvider.Authorize(command.GetType());
        var validationErrors = await _validationProvider.Validate(command);
        if (validationErrors.Any())
        {
            return new BadRequestObjectResult(new NegativeServerResponseDto { Message = "Validation Error", ValidationErrors = validationErrors });
        }

        await _commandExecutor.Send(command);
        return Ok();
    }
}
