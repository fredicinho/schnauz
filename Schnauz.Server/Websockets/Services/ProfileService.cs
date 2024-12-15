using Microsoft.AspNetCore.SignalR;
using Schnauz.GrainInterfaces;
using Schnauz.Server.Mappers;
using Schnauz.Server.Websockets.Hubs;
using Schnauz.Shared.Constants;
using Schnauz.Shared.Dtos;

namespace Schnauz.Server.Websockets.Services;

public class ProfileService(
    IHubContext<ProfileHub> profileHubContext,
    UserConnectionService userConnectionService, 
    IClusterClient clusterClient,
    ILogger<ProfileService> logger)
{
    public async ValueTask SendProfileToUser(string username)
    {
        var profile = await CreateProfileDto(username);
        await SendProfileOnSocket(profile);
    }

    public async ValueTask SendProfileToUser(ProfileDto profileDto)
    {
        await SendProfileOnSocket(profileDto);
    }

    private async ValueTask SendProfileOnSocket(ProfileDto profileDto)
    {
        logger.LogInformation($"Send profile to user: {profileDto.UserName}");
        if (userConnectionService.TryGetConnection(profileDto.UserName, out var connectionId))
        {
            logger.LogInformation($"Send profile to user: {profileDto.UserName} with ConnectionId: {connectionId}");
            await profileHubContext.Clients.Client(connectionId).SendAsync(ProfileHubApi.ReceiveProfileMethod, profileDto);
        }
    }
    
    private async Task<ProfileDto> CreateProfileDto(string userName)
    {
        var playerGrain = clusterClient.GetGrain<IPlayer>(userName);
        var currentMatchId = await playerGrain.GetCurrentMatchId();
        return new ProfileDto
        {
            UserName = playerGrain.GetGrainId().Key.ToString()!,
            UserState = await playerGrain.GetUserState(),
            CurrentMatch = currentMatchId.HasValue ? await CreateMatchDto(userName, currentMatchId.Value) : null,
        };
    }

    private async Task<MatchDto> CreateMatchDto(string userName, Guid matchId)
    {
        var matchGrain = clusterClient.GetGrain<IMatch>(matchId);
        var gameMatchDto = await matchGrain.GetMatch();
        return MatchDtoMapper.Map(userName, gameMatchDto);
    }
}