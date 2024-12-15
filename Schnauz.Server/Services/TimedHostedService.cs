using Schnauz.GrainInterfaces;
using Schnauz.Grains.MatchMaker;
using Schnauz.Shared.Dtos.enums;

namespace Schnauz.Server.Services;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

public class TimedHostedService(ILogger<TimedHostedService> logger, IClusterClient clusterClient)
    : IHostedService, IDisposable
{
    private Timer timer;


    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Timed Hosted Service running.");

        timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

        return Task.CompletedTask;
    }

    private void DoWork(object state)
    {
        logger.LogInformation("Match players if there are any.");
        foreach (var region in Enum.GetValues<RegionDto>())
        {
            var matchMakerGrain = clusterClient.GetGrain<IMatchMaker>(MatchMakerKey.GetKey(region));
            matchMakerGrain.MatchPlayers();
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Timed Hosted Service is stopping.");

        timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        timer?.Dispose();
    }
}