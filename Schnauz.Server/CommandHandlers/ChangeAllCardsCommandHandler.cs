
using Schnauz.GrainInterfaces;
using Schnauz.Server.CommandHandlers.Core;
using Schnauz.Shared.Commands;

namespace Schnauz.Server.CommandHandlers;

public class ChangeAllCardsCommandHandler(IClusterClient clusterClient) : ICommandHandler<ChangeAllCardsCommand>
{
    public async Task Execute(ChangeAllCardsCommand command)
    {
        var playerGrain = clusterClient.GetGrain<IPlayer>(command.Username);
        var matchId = await playerGrain.GetCurrentMatchId();
        if (!matchId.HasValue)
        {
            throw new InvalidOperationException("Player is not in a match.");
        }
        var cardDealerGrain = clusterClient.GetGrain<ICardDealer>(matchId.Value);
        await cardDealerGrain.ChangeAllCards(playerGrain.GetGrainId().Key.ToString()!);
    }
}
