using Schnauz.Shared.Dtos;

namespace Schnauz.Client.Services.AppState;

public class AppStateService
{
    public Profile Profile { get; private set; } = new();

    public Match CurrentMatch { get; private set; } = new();

    public void SetProfile(ProfileDto profileDto)
    {
        Profile.SetUserName(profileDto.UserName);
        Profile.SetUserState(profileDto.UserState);
        CurrentMatch.SetMatchDto(profileDto.CurrentMatch);
    }

    // I am not sure if this should be the right approach.
    public void ResetProfile()
    {
        Profile.SetUserName(Profile.Guest);
        Profile.SetUserState(null);
        CurrentMatch = new Match();
    }
}