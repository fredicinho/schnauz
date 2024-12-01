using Microsoft.AspNetCore.SignalR;
using Schnauz.Shared.Constants;

namespace Schnauz.SignalR;

public class ProfileHub(UserConnectionService userConnectionService) : Hub
{
    public override Task OnConnectedAsync()
    {
        // Retrieve userId from the query string
        var userName = Context.GetHttpContext()?.Request.Query[ProfileHubApi.UserNameQueryParameter];
        if (!string.IsNullOrEmpty(userName))
        {
            userConnectionService.AddConnection(userName, Context.ConnectionId);
        }

        Console.WriteLine($"User connected: {userName} with ConnectionId: {Context.ConnectionId}");
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        userConnectionService.RemoveConnection(Context.ConnectionId);
        Console.WriteLine($"User disconnected with ConnectionId: {Context.ConnectionId}");
        return base.OnDisconnectedAsync(exception);
    }
}