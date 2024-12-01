using Schnauz.Shared.Dtos.enums;

namespace Schnauz.Client.Services.AppState;

public class Profile
{
    public const string Guest = "Guest";
    private string UserName { get; set; } = Guest;
    
    private UserStateDto? UserState { get; set; }
    
    public bool IsLoggedIn => UserName != Guest;
    
    public event Action? OnChange;
    
    private void NotifyStateChanged() => OnChange?.Invoke();
    
    public void SetUserName(string userName)
    {
        UserName = userName;
        NotifyStateChanged();
    }
    
    public string GetUserName() => UserName;
    
    public void SetUserState(UserStateDto? userState)
    {
        UserState = userState;
        NotifyStateChanged();
    }
    
    public UserStateDto? GetUserState() => UserState;
}