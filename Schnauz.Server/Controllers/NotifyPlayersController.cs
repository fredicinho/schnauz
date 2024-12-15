using Microsoft.AspNetCore.Mvc;
using Schnauz.Server.Mappers;
using Schnauz.Server.Websockets.Services;
using Schnauz.Shared.Dtos;

namespace Schnauz.Server.Controllers;

[ApiController]
[Route("api/game-match")]
public class NotifyPlayersController(ProfileService profileService, ILogger<NotifyPlayersController> logger) : ControllerBase
{
    [HttpPost]
    public async Task SendGameMatch([FromBody] GameMatchDto gameMatch)
    {
        logger.LogInformation("Notifying players: {players}", gameMatch.CurrentRound.Players.Select(x => x.UserName));
        foreach (var player in gameMatch.CurrentRound.Players)
        {
            await profileService.SendProfileToUser(ProfileDtoMapper.Map(player.UserName, gameMatch));
        }
    }
}