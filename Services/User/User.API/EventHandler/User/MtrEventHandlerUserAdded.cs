using FamilieLaissMassTransitDefinitions.Contracts.Events;
using FamilieLaissMassTransitDefinitions.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using User.Domain.DomainEvents;

namespace Catalog.API.Events.MediaItem
{
    /// <summary>
    /// Event handler for domain event for user created
    /// </summary>
    public class MtrEventUserCreatedHandler : INotificationHandler<MtrEventUserCreated>
    {
        #region Private Members
        private readonly IBus _ServiceBus;
        private readonly ILogger<MtrEventUserCreatedHandler> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="serviceBus">Masstransit service bus. Will be injected by DI-Container</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public MtrEventUserCreatedHandler(IBus serviceBus, ILogger<MtrEventUserCreatedHandler> logger)
        {
            _ServiceBus = serviceBus;
            _Logger = logger;
        }
        #endregion

        #region Mediatr-Handler
        /// <summary>
        /// Will be called by Mediatr
        /// </summary>
        /// <param name="notification">The notification data</param>
        /// <param name="cancellationToken">The cancelation token</param>
        /// <returns>Task</returns>
        public async Task Handle(MtrEventUserCreated notification, CancellationToken cancellationToken)
        {
            //Logging ausgeben
            _Logger.LogInformation("Mediatr-Handler for user {User} created was called", notification);

            //Versenden des Events über den Service Bus
            _Logger.LogDebug("Sending event over service bus");
            var Event = new UserAccountCreatedEvent(notification.ID, notification.UserName, notification.EMail);
            await _ServiceBus.Publish<iUserAccountCreatedEvent>(Event);
        }
        #endregion
    }
}
