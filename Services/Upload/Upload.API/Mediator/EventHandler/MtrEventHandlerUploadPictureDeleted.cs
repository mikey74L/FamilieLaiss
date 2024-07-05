using FamilieLaissMassTransitDefinitions.Contracts.Events;
using FamilieLaissMassTransitDefinitions.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Options;
using Upload.API.Models;
using Upload.Domain.DomainEvents.UploadPicture;

namespace Upload.API.Mediator.EventHandler;

/// <summary>
/// Event handler for upload picture deleted
/// </summary>
public class MtrEventUploadPictureDeletedHandler(IBus serviceBus, IOptions<AppSettings> appSettings,
        ILogger<MtrEventUploadPictureDeletedHandler> logger) : INotificationHandler<DomainEventUploadPictureDeleted>
{
    #region Mediatr-Handler
    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="notification">The notification data</param>
    /// <param name="cancellationToken">The cancelation token</param>
    /// <returns>Task</returns>
    public async Task Handle(DomainEventUploadPictureDeleted notification, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Mediatr-Handler for upload picture deleted was called for ID = {notification.ID}");

        //Ermitteln aller Dateien aus dem Verzeichnis
        logger.LogDebug("Construct filename");
        var filelist = Directory.GetFiles(appSettings.Value.Directory_Upload_Picture, notification.ID + "*.*");

        //Physisches Löschen des Bildes aus dem Verzeichnis
        logger.LogDebug("Delete all pictures from directory");
        foreach (var filename in filelist)
        {
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
        }

        //Versenden des Events über den Service Bus
        logger.LogDebug("Sending event over service bus");
        var newEvent = new UploadPictureDeletedEvent(Convert.ToInt64(notification.ID));
        await serviceBus.Publish<iUploadPictureDeletedEvent>(newEvent);
    }
    #endregion
}
