using MediatR;
using Microsoft.Extensions.Options;
using Upload.API.Models;
using Upload.Domain.DomainEvents.UploadPortrait;

namespace Upload.API.Mediator.EventHandler;

/// <summary>
/// Event handler for upload portrait deleted
/// </summary>
public class MtrEventUploadPortraitDeletedHandler(IOptions<AppSettings> appSettings, ILogger<MtrEventUploadPortraitDeletedHandler> logger) : INotificationHandler<DomainEventUploadPortraitDeleted>
{
    #region Mediatr-Handler
    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="notification">The notification data</param>
    /// <param name="cancellationToken">The cancelation token</param>
    /// <returns>Task</returns>
    public Task Handle(DomainEventUploadPortraitDeleted notification, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Mediatr-Handler for upload portrait deleted was called for ID = {notification.ID}");

        //Ermitteln der Liste der Dateien
        logger.LogDebug("Get list for all formats of upload portrait");
        var FileList = Directory.GetFiles(appSettings.Value.Directory_Upload_Portrait, notification.ID + "*.*");

        //Löschen der Bilder von der Platte
        foreach (var Item in FileList)
        {
            logger.LogDebug("Delete file \"{0}\" from disk", Item);
            File.Delete(Item);
        }

        //Funktionsergebnis
        return Task.CompletedTask;
    }
    #endregion
}
