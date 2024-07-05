using MassTransit;
using Microsoft.Extensions.Hosting;
using Polly;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExecuterLayerHelper.Helper
{
    public class MassTransitConsoleHostedService: IHostedService
    {
        #region Private Members
        private readonly IBusControl _BusControl;
        #endregion

        #region C'tor
        public MassTransitConsoleHostedService(IBusControl busControl)
        {
            _BusControl = busControl;
        }
        #endregion

        #region Interface IHostedService
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
                _BusControl.Start();
            });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _BusControl.StopAsync(cancellationToken);
        }
        #endregion
    }
}
