using DomainHelper.DomainEvents;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace InfrastructureHelper.EventDispatchHandler
{
    /// <summary>
    /// Background Service for dispatching domain events over mediator
    /// </summary>
    public class EventDispatcherBackgroundService : BackgroundService
    {
        #region Private Members
        private readonly IServiceScopeFactory _ScopeFactory;
        private readonly iEventStore _EventStore;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="scopeFactory">Service Scope Factory. Will be injected by DI-Container</param>
        public EventDispatcherBackgroundService(IServiceScopeFactory scopeFactory, iEventStore eventStore)
        {
            _ScopeFactory = scopeFactory;
            _EventStore = eventStore;
        }
        #endregion

        #region Protected Overrides
        /// <inheritdoc/>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //Solange kein beenden der Applikation ansteht, wird die Abfrage im Loop ausgeführt
            while (!stoppingToken.IsCancellationRequested)
            {
                //Erstellen des Scopes
                using (var scope = _ScopeFactory.CreateScope())
                {
                    //Ermitteln des Service für den Mediator über den Scope
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                    //Schleife für die Abarbeitung der Events
                    DomainEventBase CurrentItem;
                    while (_EventStore.Store.TryTake(out CurrentItem))
                    {
                        try
                        {
                            //Abarbeiten des Events über Mediator
                            await mediator.Publish(CurrentItem);
                        }
                        catch (Exception ex)
                        {
                            _EventStore.Store.Add(CurrentItem);
                            Debug.WriteLine(ex.Message);
                        }
                    }
                }

                //500 Milli-Sekunden bis zur nächsten Abfrage warten
                await Task.Delay(100);
            }
        }
        #endregion
    }
}