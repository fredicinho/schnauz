using Schnauz.Shared;
using Schnauz.Shared.Dtos;
using System.Net.Http.Json;

namespace Schnauz.Client.Cqrs;

public class CqrsHttpClient(
    IInfoMessage messageService,
    HttpClient httpClient,
    ILogger<CqrsHttpClient> logger)
{
    public readonly IInfoMessage _messageService = messageService;
    public readonly HttpClient _httpClient = httpClient;
    public readonly ILogger<CqrsHttpClient> _logger = logger;

    public async Task<bool> PostCommandAsync(CommandQueryContract command, string url = ApiPaths.Command, CancellationToken token = default)
    {
        var response = await _httpClient.PostAsJsonAsync(url, command, token);
        if (response.IsSuccessStatusCode)
        {
            return true;
        }
        await ProcessServerError(command, response);
        return false;
    }

    public async Task<TResponse> PostQueryAsync<TResponse>(CommandQueryContract command, string url = ApiPaths.Query, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync(url, command, cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            try
            {
                return (await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken))!;
            }
            catch (Exception e)
            {
                await ProcessParsingError(command, response, e);
            }
        }
        else
        {
            await ProcessServerError(command, response);
        }
        return default!;
    }

    private async Task ProcessServerError(CommandQueryContract contract, HttpResponseMessage response)
    {
        try
        {
            var error = await response.Content.ReadFromJsonAsync<NegativeServerResponseDto>();
            if (error?.ValidationErrors?.Any() == true)
            {
                ProcessValidationError(contract, error.ValidationErrors);
            }
            _messageService.ShowError(contract.SimpleName + " Error: " + error?.Message, "Server Error");
            _logger.LogError($"Command '{contract.ObjectName}' thrown error: '{error?.Message}'");
        }
        catch (Exception e)
        {
            await ProcessParsingError(contract, response, e);
        }
    }

    private void ProcessValidationError(CommandQueryContract contract, IReadOnlyList<ValidationErrorDto> errors)
    {
        var logString = $"Command '{contract.ObjectName}' Validation Errors: ";
        var toasterString = string.Empty;
        foreach (var error in errors)
        {
            logString += $"{error.ErrorMessage}[{error.PropertyFullName}], ";
            toasterString += $"{error.ErrorMessage} ";
        }
        _messageService.ShowError(toasterString, "Validation Error");
        _logger.LogError(logString);
    }

    private async Task ProcessParsingError(CommandQueryContract contract, HttpResponseMessage response, Exception e)
    {
        _messageService.ShowError($"Error parsing for command '{contract.SimpleName}' failed with message: '{e.Message}'. Check console for more details.", "Command Server Error");
        _logger.LogError($"Command \"{contract.ObjectName}\" error deserialization failed: Message: \"{e.Message}\", Response as string: \"{await response.Content.ReadAsStringAsync()}\"");
    }
}
