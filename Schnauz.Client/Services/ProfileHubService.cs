using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Schnauz.Client.Services.AppState;
using Schnauz.Shared.Constants;
using Schnauz.Shared.Dtos;
using Schnauz.Shared.Dtos.enums;

namespace Schnauz.Client.Services;

public class ProfileHubService(AppStateService appStateService, NavigationManager navigationManager)
{
    private HubConnection? _hubConnection;

    public async Task Connect()
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(navigationManager.ToAbsoluteUri($"{ProfileHubApi.ProfileHubUrl}?{ProfileHubApi.UserNameQueryParameter}={appStateService.Profile.GetUserName()}"))
            .Build();
        
        _hubConnection.On<ProfileDto>(ProfileHubApi.ReceiveProfileMethod, appStateService.SetProfile);
        
        await _hubConnection.StartAsync();
    }
    
    public async Task Disconnect()
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.StopAsync();
            appStateService.Profile.SetUserState(UserStateDto.SIGNED_IN);
        }
    }
}