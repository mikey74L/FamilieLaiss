using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace InfrastructureHelper.EventDispatchHandler;

/// <summary>
/// Background Service for dispatching domain events over mediator
/// </summary>
/// <remarks>
/// C'tor
/// </remarks>
/// <param name="scopeFactory">Service Scope Factory. Will be injected by DI-Container</param>
public class EventDispatcherBackgroundService(IServiceScopeFactory scopeFactory, iEventStore eventStore)
    : BackgroundService
{
    #region Protected Overrides

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //Solange kein Beenden der Applikation ansteht, wird die Abfrage im Loop ausgeführt
        while (!stoppingToken.IsCancellationRequested)
        {
            //Erstellen des Scopes
            using (var scope = scopeFactory.CreateScope())
            {
                //Ermitteln des Service für den Mediator über den Scope
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                //Schleife für die Abarbeitung der Events
                while (eventStore.Store.TryTake(out var currentItem))
                {
                    try
                    {
                        //Abarbeiten des Events über Mediator
                        await mediator.Publish(currentItem, stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        eventStore.Store.Add(currentItem);
                        Debug.WriteLine(ex.Message);
                    }
                }
            }

            //500 Milli-Sekunden bis zur nächsten Abfrage warten
            await Task.Delay(100, stoppingToken);
        }
    }

    #endregion
}