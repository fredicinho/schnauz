using Microsoft.AspNetCore.SignalR;
using Schnauz.GrainInterfaces;
using Schnauz.Server.Services;
using Schnauz.Server.Websockets.Services;
using Schnauz.Shared.Constants;
using Schnauz.Shared.Dtos;

namespace Schnauz.Server.Websockets.Hubs;

public class ProfileHub(
    UserConnectionService userConnectionService,
    ProfileService profileService,
    IClusterClient clusterClient
    ) : Hub
{
    public override async Task OnConnectedAsync()
    {
        // Retrieve userId from the query string
        var userName = Context.GetHttpContext()?.Request.Query[ProfileHubApi.UserNameQueryParameter];
        if (!string.IsNullOrEmpty(userName))
        {
            userConnectionService.AddConnection(userName!, Context.ConnectionId);
        }

        Console.WriteLine($"User connected: {userName} with ConnectionId: {Context.ConnectionId}");
        await profileService.SendProfileToUser(userName!);
        await base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        userConnectionService.RemoveConnection(Context.ConnectionId);
        Console.WriteLine($"User disconnected with ConnectionId: {Context.ConnectionId}");
        return base.OnDisconnectedAsync(exception);
    }
}