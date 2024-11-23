using Schnauz.Shared;
using Schnauz.Shared.Interfaces;

namespace Schnauz.Client.Cqrs;

public class CommandExecutor(CqrsHttpClient httpClient) : ICommandExecutor
{
    private readonly CqrsHttpClient _httpClient = httpClient;

    public async Task<bool> Send(ICommand cqrsCommand)
    {
        return await _httpClient.PostCommandAsync(new CommandQueryContract(cqrsCommand));
    }
}
