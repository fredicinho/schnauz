using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Schnauz.Shared.Dtos;

namespace Schnauz.Grains.Services;

public class SendGameMatchService(HttpClient httpClient, ILogger<SendGameMatchService> logger)
{
    public async ValueTask SendGameMatch(GameMatchDto gameMatch)
    {
        logger.LogInformation("Sending game match to server");
        var content = new StringContent(JsonSerializer.Serialize(gameMatch), Encoding.UTF8, "application/json");
        await httpClient.PostAsync("http://localhost:5105/api/game-match", content);
    }
}