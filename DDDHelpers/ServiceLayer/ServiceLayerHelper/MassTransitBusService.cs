using MassTransit;
using Microsoft.Extensions.Hosting;
using Polly;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceLayerHelper
{
    public class MassTransitBusService: IHostedService
    {
        private readonly IBusControl _busControl;

        public MassTransitBusService(IBusControl busControl)
        {
            _busControl = busControl;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            //Eine Retry-Policy mit Polly erstellen
            //Falls der Masstransit-Container beim Start des Service-Containers noch nicht bereit ist, wird 
            //ein mehrfacher Connection-Versuch unternommen
            var RetryPolicy = Policy.Handle<Exception>()
                .WaitAndRetry(10, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            //Ausführen der Policy um eine Verbindung mit Mass-Transit herzustellen
            RetryPolicy.ExecuteAndCapture(() =>
            {
                _busControl.Start();
            });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _busControl.StopAsync(cancellationToken);
        }
    }
}
