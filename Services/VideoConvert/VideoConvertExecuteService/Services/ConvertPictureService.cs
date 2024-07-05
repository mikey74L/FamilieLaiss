using System.IO;
using System.Threading.Tasks;
using FamilieLaissMassTransitDefinitions.Contracts.Commands;
using FamilieLaissMassTransitDefinitions.Contracts.Events;
using FamilieLaissMassTransitDefinitions.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using VideoConvertExecuteService.Interfaces;
using VideoConvertExecuteService.Models;

namespace VideoConvertExecuteService.Services;

public class ConvertPictureService(
    ILogger<ConvertPictureService> logger,
    IDatabaseOperations databaseOperations,
    IOptions<AppSettings> appSettings) : IConvertPicture
{
    #region Private Methods

    private void DoConvertion(string folder, string filename, string mode, int width, int height)
    {
        var fullFilenameTarget = "";
        var modeOriginal = "";
        var extensionTarget = "";

        //Build Full-Filename for Picture
        var fullFilename = Path.Combine(folder, Path.GetFileNameWithoutExtension(filename));
        var extensionOriginal = Path.GetExtension(filename);
        var fullFilenameOriginal = fullFilename + extensionOriginal;
        switch (extensionOriginal.ToUpper())
        {
            case ".JPG":
            case ".JPEG":
                modeOriginal = "jpeg";
                break;
            case ".PNG":
                modeOriginal = "png";
                break;
            case ".GIF":
                modeOriginal = "gif";
                break;
            case ".BMP":
                modeOriginal = "bmp";
                break;
        }

        if (height > 0 || width > 0)
        {
            fullFilenameTarget = fullFilename + "_" + height.ToString() + "_" + width.ToString();
        }
        else
        {
            fullFilenameTarget = fullFilename;
        }

        extensionTarget = mode switch
        {
            "png" => ".png",
            "jpeg" => ".jpg",
            "bmp" => ".bmp",
            _ => extensionTarget
        };

        if (mode.ToUpper() != modeOriginal.ToUpper())
        {
            fullFilenameTarget = fullFilenameTarget + extensionTarget;
        }
        else
        {
            fullFilenameTarget = fullFilenameTarget + extensionOriginal;
        }

        if (File.Exists(fullFilenameTarget)) return;
        var originalImage = Image.Load(fullFilenameOriginal);

        if (height > 0 || width > 0)
        {
            ResizeOptions options = new()
            {
                Size = new Size(width, height),
                Mode = ResizeMode.Max
            };

            originalImage.Mutate(x => x.Resize(options));
        }

        originalImage.Mutate(x => x.AutoOrient());

        switch (mode)
        {
            case "png":
                using (var ms = new FileStream(fullFilenameTarget, FileMode.OpenOrCreate))
                {
                    originalImage.SaveAsPng(ms);
                }

                break;
            case "jpeg":
                using (var ms = new FileStream(fullFilenameTarget, FileMode.OpenOrCreate))
                {
                    originalImage.SaveAsJpeg(ms);
                }

                break;
            case "bmp":
                using (var ms = new FileStream(fullFilenameTarget, FileMode.OpenOrCreate))
                {
                    originalImage.SaveAsBmp(ms);
                }

                break;
            default:
                using (var ms = new FileStream(fullFilenameTarget, FileMode.OpenOrCreate))
                {
                    originalImage.SaveAsJpeg(ms);
                }

                break;
        }
    }

    #endregion

    #region IConvertPicture

    public async Task ConvertPicture(long id, string filename, ConsumeContext<IConvertVideoCmd> consumerContext)
    {
        logger.LogInformation("Set status for convert picture begin");
        await databaseOperations.SetStatusConvertPictureBeginAsync(id);

        logger.LogInformation("Send event over mass transit");
        var @event = new VideoConvertProgressEvent()
        {
            ConvertStatusId = consumerContext.Message.ConvertStatusId,
            UploadVideoId = consumerContext.Message.Id
        };
        await consumerContext.Publish<IVideoConvertProgressEvent>(@event);

        var filenamePreviewPicture = Path.GetFileNameWithoutExtension(filename) + ".jpg";

        logger.LogInformation("Converting picture to format 298 x 170");
        DoConvertion(appSettings.Value.DirectoryUploadPicture, filenamePreviewPicture, "png", 298, 170);

        logger.LogInformation("Set status for convert picture end");
        await databaseOperations.SetStatusConvertPictureEndAsync(id);

        logger.LogInformation("Send event over mass transit");
        await consumerContext.Publish<IVideoConvertProgressEvent>(@event);
    }

    #endregion
}