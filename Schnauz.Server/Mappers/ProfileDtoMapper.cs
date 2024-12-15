using Schnauz.Shared.Dtos;

namespace Schnauz.Server.Mappers;

public static class ProfileDtoMapper
{
    public static ProfileDto Map(string userName, GameMatchDto gameMatchDto)
    {
        var profile = gameMatchDto.CurrentRound.Players.FirstOrDefault(x => x.UserName == userName)!;
        return new ProfileDto
        {
            UserName = userName,
            UserState = profile.UserState,
            CurrentMatch = MatchDtoMapper.Map(userName, gameMatchDto),
        };
    }
}