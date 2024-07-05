using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServiceHelper.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PictureConvertExecuteService;

internal sealed class ConsoleHostedService(
    ILogger<ConsoleHostedService> logger,
    IHostApplicationLifetime appLifetime,
    IPreconditions preConditions)
    : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug($"Starting with arguments: {string.Join(" ", Environment.GetCommandLineArgs())}");

        try
        {
            preConditions.CheckPreconditions();
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Service could not be started");
            appLifetime.StopApplication();
        }
        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}