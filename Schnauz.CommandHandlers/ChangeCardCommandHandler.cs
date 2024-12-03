using Microsoft.AspNetCore.SignalR;
using Schnauz.CommandHandlers.Core;
using Schnauz.Shared.Commands;
using Schnauz.Shared.Constants;
using Schnauz.Shared.Dtos;
using Schnauz.Shared.Dtos.enums;
using Schnauz.SignalR;

namespace Schnauz.CommandHandlers;

public class ChangeCardCommandHandler() : ICommandHandler<ChangeCardCommand>
{
    public async Task Execute(ChangeCardCommand command)
    {
    }
}
