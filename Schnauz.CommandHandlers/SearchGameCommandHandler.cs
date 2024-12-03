using Microsoft.AspNetCore.SignalR;
using Schnauz.CommandHandlers.Core;
using Schnauz.Shared.Commands;
using Schnauz.Shared.Constants;
using Schnauz.Shared.Dtos;
using Schnauz.Shared.Dtos.enums;
using Schnauz.SignalR;

namespace Schnauz.CommandHandlers;

public class SearchGameCommandHandler(IHubContext<ProfileHub> profileHubContext, UserConnectionService userConnectionService) : ICommandHandler<SearchGameCommand>
{
    public async Task Execute(SearchGameCommand command)
    {
        // TODO: This is only a testing implementation... 
        var matchDto = new MatchDto
        {
            Players = [
                new (){ UserName = "TestUser", NumberOfLifePoints = 3, IsOut = false, LastAction = PlayerActionDto.CHANGED_CARD },
                new (){ UserName = "TestUser2", NumberOfLifePoints = 3, IsOut = false, LastAction = PlayerActionDto.CHANGED_CARD },
                new () { UserName = "Fredi", NumberOfLifePoints = 3, IsOut = false, LastAction = PlayerActionDto.CHANGED_CARD },
            ],
            MatchStateDto = MatchStateDto.RUNNING,
            CurrentRound = new()
            {
                RoundState = RoundStateDto.RUNNING,
                NextTurnUserName = command.Username,
                CardsOnTable = [
                    new () { Suit = SuitDto.HEARTS, CardRank = CardRankDto.Eight },
                    new () { Suit = SuitDto.SPADES, CardRank = CardRankDto.King },
                    new () { Suit = SuitDto.DIAMONDS, CardRank = CardRankDto.Jack },
                ],
                CardsOnHand = [
                    new (){ Suit = SuitDto.CLUBS, CardRank = CardRankDto.Ace },
                    new (){ Suit = SuitDto.CLUBS, CardRank = CardRankDto.Seven },
                    new (){ Suit = SuitDto.CLUBS, CardRank = CardRankDto.Ten },
                ],
            },
            RankPlayers = [],
        };
        var profileDto = new ProfileDto
        {
            UserName = command.Username,
            UserState = UserStateDto.PARTICIPATING_IN_MATCH,
            CurrentMatch = matchDto,
        };
        
        if (userConnectionService.TryGetConnection(profileDto.UserName, out var connectionId))
        {
            Console.WriteLine($"Send message to user: {profileDto.UserName} with ConnectionId: {connectionId}");
            await profileHubContext.Clients.Client(connectionId).SendAsync(ProfileHubApi.ReceiveProfileMethod, profileDto);
        }
    }
}
