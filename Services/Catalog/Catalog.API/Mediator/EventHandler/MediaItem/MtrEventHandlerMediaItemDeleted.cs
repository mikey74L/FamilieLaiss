using Catalog.Domain.DomainEvents;
using FamilieLaissMassTransitDefinitions.Contracts.Events;
using FamilieLaissMassTransitDefinitions.Events;
using FamilieLaissSharedObjects.Enums;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.API.Mediator.EventHandler.MediaItem
{
    /// <summary>
    /// Event handler for media item deleted
    /// </summary>
    public class MtrEventMediaItemDeletedHandler : INotificationHandler<MtrEventMediaItemDeleted>
    {
        #region Private Members
        private readonly IBus _ServiceBus;
        private readonly ILogger<MtrEventMediaItemDeletedHandler> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="serviceBus">Masstransit service bus. Will be injected by DI-Container</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public MtrEventMediaItemDeletedHandler(IBus serviceBus, ILogger<MtrEventMediaItemDeletedHandler> logger)
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
        public async Task Handle(MtrEventMediaItemDeleted notification, CancellationToken cancellationToken)
        {
            //Logging ausgeben
            _Logger.LogInformation("Mediatr-Handler for media item deleted was called with following parameters:");
            _Logger.LogDebug("ID              : ", notification.ID);
            _Logger.LogDebug("Media-Type      : ", notification.MediaType);
            _Logger.LogDebug("UploadItemID    : ", notification.UploadItemID);
            _Logger.LogDebug("DeleteUploadItem: ", notification.DeleteUploadItem);

            //Versenden des Events über den Service Bus
            _Logger.LogDebug("Sending event over service bus");
            var Event = new MediaItemDeletedEvent(Convert.ToInt64(notification.ID), notification.UploadItemID, notification.MediaType == EnumMediaType.Picture,
                notification.DeleteUploadItem);
            await _ServiceBus.Publish<iMediaItemDeletedEvent>(Event);
        }
        #endregion
    }
}
