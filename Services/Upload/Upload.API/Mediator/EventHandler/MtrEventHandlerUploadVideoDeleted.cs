using FamilieLaissMassTransitDefinitions.Contracts.Events;
using FamilieLaissMassTransitDefinitions.Events;
using FamilieLaissSharedObjects.Enums;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Options;
using Upload.API.Models;
using Upload.Domain.DomainEvents.UploadVideo;

namespace Upload.API.Mediator.EventHandler;

/// <summary>
/// Event handler for upload video deleted
/// </summary>
public class MtrEventUploadVideoDeletedHandler(
    IBus serviceBus,
    IOptions<AppSettings> appSettings,
    ILogger<MtrEventUploadVideoDeletedHandler> logger) : INotificationHandler<DomainEventUploadVideoDeleted>
{
    #region Mediatr-Handler

    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="notification">The notification data</param>
    /// <param name="cancellationToken">The cancelation token</param>
    /// <returns>Task</returns>
    public async Task Handle(DomainEventUploadVideoDeleted notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for upload video deleted was called with following parameters:");

        //Zusammensetzen des Dateinamens für das Sprite-Image
        logger.LogDebug("Construct filename for sprite image file");
        string filenameSprite =
            Path.Combine(appSettings.Value.Directory_Upload_Picture, notification.ID + "_Sprite.jpg");

        //Zusammensetzen des Dateinamens für das Poster-Image
        logger.LogDebug("Construct filename for poster image file");
        string filenamePoster = Path.Combine(appSettings.Value.Directory_Upload_Picture, notification.ID + ".jpg");

        //Zusammensetzen des Dateinamens für das VTT-File
        logger.LogDebug("Construct filename for VTT file");
        string filenameVTT = Path.Combine(appSettings.Value.Directory_Upload_Video, notification.ID + ".vtt");

        logger.LogDebug("Delete VTT mpd file");
        if (File.Exists(filenameVTT))
        {
            File.Delete(filenameVTT);
        }

        logger.LogDebug("Delete sprite image file");
        if (File.Exists(filenameSprite))
        {
            File.Delete(filenameSprite);
        }

        logger.LogDebug("Delete poster image file");
        if (File.Exists(filenamePoster))
        {
            File.Delete(filenamePoster);
        }

        logger.LogDebug("Delete converted pictures");
        var fileListPictures =
            Directory.GetFiles(appSettings.Value.Directory_Upload_Picture, notification.ID + "*.png");
        foreach (var file in fileListPictures)
        {
            logger.LogDebug($"Delete file \"{file}\"");
            File.Delete(file);
        }

        //Je nach dem ob es sich um ein DASH oder ein normales Video handelt muss unterschiedlich gelöscht werden
        switch (notification.VideoType)
        {
            case EnumVideoType.Hls:
                //Löschen der m3u8-Files
                var fileList = Directory.GetFiles(appSettings.Value.Directory_Upload_Video, notification.ID + "*.m3u8");
                foreach (var FileItem in fileList)
                {
                    File.Delete(FileItem);
                }

                //Löschen der TS-Files
                fileList = Directory.GetFiles(appSettings.Value.Directory_Upload_Video, notification.ID + "*.ts");
                foreach (var FileItem in fileList)
                {
                    File.Delete(FileItem);
                }

                break;
            case EnumVideoType.Progressive:
                //Zusammensetzen des Dateinamens für das 512 x 288 MP4 File
                logger.LogDebug("Construct filename for video file");
                string filenameVideo = Path.Combine(appSettings.Value.Directory_Upload_Video, notification.ID + ".mp4");

                logger.LogDebug("Delete video file");
                if (File.Exists(filenameVideo))
                {
                    File.Delete(filenameVideo);
                }

                break;
        }

        //Versenden des Events über den Service Bus
        logger.LogDebug("Sending event over service bus");
        var newEvent = new UploadVideoDeletedEvent(Convert.ToInt64(notification.ID));
        await serviceBus.Publish<iUploadVideoDeletedEvent>(newEvent);
    }

    #endregion
}