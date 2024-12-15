using Schnauz.Shared.Dtos;
using Schnauz.Shared.Dtos.enums;

namespace Schnauz.Server.Mappers;

public static class MatchDtoMapper
{
    public static MatchDto Map(string userName, GameMatchDto gameMatchDto)
    {
        var profile = gameMatchDto.CurrentRound.Players.FirstOrDefault(x => x.UserName == userName)!;
        return new MatchDto 
        {
            CurrentRound = new RoundDto
            {
                CardsOnHand = profile.CardsInHand,
                CardsOnTable = gameMatchDto.CurrentRound.CardsOnTable,
                RoundState = gameMatchDto.CurrentRound.RoundState,
                ActivePlayer = gameMatchDto.CurrentRound.ActivePlayer,
                LostPlayers = gameMatchDto.CurrentRound.PlayersLost
            },
            MatchStateDto = gameMatchDto.MatchState,
            Players = gameMatchDto.CurrentRound.Players
                .Select(player => new PlayerDto
                {
                    UserName = player.UserName,
                    NumberOfLifePoints = player.NumberOfLifePoints,
                    IsOut = player.IsOut,
                    LastAction = player.LastAction,
                    NumberOfCardPoints = gameMatchDto.CurrentRound.RoundState == RoundStateDto.FINISHED ? player.NumberOfCardPoints : null,
                    CardsInHand = gameMatchDto.CurrentRound.RoundState == RoundStateDto.FINISHED ? player.CardsInHand : []
                })
                .ToList(),
            RankPlayers = gameMatchDto.RankedPlayers,
            PlayersWhoRequestedNewMatch = gameMatchDto.PlayersWhoRequestedNewMatch
        };
    }
}