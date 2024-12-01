using Microsoft.AspNetCore.ResponseCompression;
using Schnauz.Server;
using Schnauz.Server.Blazor;
using Schnauz.Shared.Constants;
using Schnauz.SignalR;

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
    clientBuilder.UseLocalhostClustering();
});

builder.Services.AddSignalR();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        ["application/octet-stream"]);
});
builder.Services.AddSingleton<UserConnectionService>();


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

app.MapHub<HelloWorldHub>(HelloWorldHub.HelloWorldUrl);
app.MapHub<ProfileHub>(ProfileHubApi.ProfileHubUrl);

app.Run();
