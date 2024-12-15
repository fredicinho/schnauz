using Microsoft.AspNetCore.ResponseCompression;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Schnauz.Server;
using Schnauz.Server.Blazor;
using Schnauz.Server.Services;
using Schnauz.Server.Websockets.Hubs;
using Schnauz.Server.Websockets.Services;
using Schnauz.Shared.Constants;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddControllers();

// Custom services
builder.Services
    .AddCqrs()
    .AddValidations();

// Add the Orleans Cluster Client
builder.Host.UseOrleansClient(clientBuilder =>
{
    clientBuilder
        .UseLocalhostClustering()
        .UseTransactions();
});

builder.Services.AddSignalR();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        ["application/octet-stream"]);
});
builder.Services.AddSingleton<UserConnectionService>();
builder.Services.AddTransient<ProfileService>();
builder.Services.AddHostedService<TimedHostedService>();

builder.Services.AddOpenTelemetry()
    .ConfigureResource(r =>
    {
        r.AddService("Server",
            serviceVersion: "MyVersion",
            serviceInstanceId: Environment.MachineName);
    })
    .WithTracing(builder => builder
        .SetSampler(new AlwaysOnSampler())
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddSource("Microsoft.Orleans.Runtime")
        .AddSource("Microsoft.Orleans.Application")
        .AddOtlpExporter(opt =>
        {
            opt.Endpoint = new Uri("http://localhost:4317");
        })
    )
    .WithMetrics(builder => builder
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddOtlpExporter(opt =>
        {
            opt.Endpoint = new Uri("http://localhost:4317");
        })
    );


var app = builder.Build();
app.UseResponseCompression();
app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Schnauz.Client._Imports).Assembly);

app.MapHub<ProfileHub>(ProfileHubApi.ProfileHubUrl);

app.Run();
