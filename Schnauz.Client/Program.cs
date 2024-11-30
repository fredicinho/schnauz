using Schnauz.Client.Cqrs;
using Schnauz.Client.Services;
using Schnauz.Shared.Dtos;
using Schnauz.Shared.Interfaces;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Schnauz.Shared;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<ToastService>();

// CQRS
builder.Services.AddTransient(sp =>
{
    var logger = sp.GetService<ILogger<CqrsHttpClient>>();
    var messageService = sp.GetService<ToastService>();
    var httpClient = sp.GetService<HttpClient>();
    return new CqrsHttpClient(messageService!, httpClient!, logger!);
});
builder.Services.AddTransient<IQueryExecutor, QueryExecutor>();
builder.Services.AddTransient<ICommandExecutor, CommandExecutor>();

builder.Services.AddFluentValidations(typeof(ValidationErrorDto));
builder.Services.AddMudServices();

await builder.Build().RunAsync();
