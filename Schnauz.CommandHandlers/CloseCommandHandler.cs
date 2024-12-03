using Microsoft.AspNetCore.SignalR;
using Schnauz.CommandHandlers.Core;
using Schnauz.Shared.Commands;
using Schnauz.Shared.Constants;
using Schnauz.Shared.Dtos;
using Schnauz.Shared.Dtos.enums;
using Schnauz.SignalR;

namespace Schnauz.CommandHandlers;

public class CloseCommandHandler() : ICommandHandler<CloseCommand>
{
    public async Task Execute(CloseCommand command)
    {

    }
}
