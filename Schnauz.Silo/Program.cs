using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Schnauz.Grains.Services;
using StackExchange.Redis;

try
{
    var host = await StartSiloAsync();
    Console.WriteLine("\n\n Press Enter to terminate...\n\n");
    Console.ReadLine();

    await host.StopAsync();
    return 0;
}
catch (Exception ex)
{
    Console.WriteLine(ex);
    return 1;
}

static async Task<IHost> StartSiloAsync()
{
    var builder = new HostBuilder()
        .UseOrleans(c =>
        {
            c.UseLocalhostClustering()
                // Use in-memory storage
                // .AddMemoryGrainStorage("schnauz")
                .AddRedisGrainStorage(name: "schnauz", configureOptions: options =>
                {
                    options.ConfigurationOptions = new ConfigurationOptions();
                    options.ConfigurationOptions.EndPoints.Add("localhost", 6379);
                })
                .UseTransactions()
                .ConfigureLogging(logging => logging.AddConsole());
        });
    
    builder.ConfigureServices(services =>
    {
        services.AddHttpClient<SendGameMatchService>();
        services.AddTransient<SendGameMatchService>();
        services.AddOpenTelemetry()
            .ConfigureResource(r =>
            {
                r.AddService("Silo Server",
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
    });

    var host = builder.Build();
    await host.StartAsync();

    return host;
}