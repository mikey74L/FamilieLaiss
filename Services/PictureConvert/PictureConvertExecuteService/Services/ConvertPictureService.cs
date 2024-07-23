using System.IO;
using System.Threading.Tasks;
using FamilieLaissMassTransitDefinitions.Contracts.Commands.UploadPicture;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PictureConvertExecuteService.Interfaces;
using PictureConvertExecuteService.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace PictureConvertExecuteService.Services;

public class ConvertPictureService(
    ILogger<ConvertPictureService> logger,
    IDatabaseOperations databaseOperations,
    IOptions<AppSettings> appSettings) : IConvertPicture
{
    #region Private Methods

    private void DoConvertion(string folder, string filename, string mode, int width, int height)
    {
        string fullFilenameTarget;
        string modeOriginal = "";
        string extensionTarget = "";

        var fullFilename = Path.Combine(folder, Path.GetFileNameWithoutExtension(filename));
        var extensionOriginal = Path.GetExtension(filename);
        var fullFilenameOriginal = fullFilename + extensionOriginal;
        if (extensionOriginal.ToUpper() == ".JPG" || extensionOriginal.ToUpper() == ".JPEG")
        {
            modeOriginal = "jpeg";
        }

        if (extensionOriginal.ToUpper() == ".PNG")
        {
            modeOriginal = "png";
        }

        if (extensionOriginal.ToUpper() == ".GIF")
        {
            modeOriginal = "gif";
        }

        if (extensionOriginal.ToUpper() == ".BMP")
        {
            modeOriginal = "bmp";
        }

        if (height > 0 || width > 0)
        {
            fullFilenameTarget = fullFilename + "_" + height.ToString() + "_" + width.ToString();
        }
        else
        {
            fullFilenameTarget = fullFilename;
        }

        switch (mode)
        {
            case "png":
                extensionTarget = ".png";
                break;
            case "jpeg":
                extensionTarget = ".jpg";
                break;
            case "bmp":
                extensionTarget = ".bmp";
                break;
        }

        if (mode.ToUpper() != modeOriginal.ToUpper())
        {
            fullFilenameTarget = fullFilenameTarget + extensionTarget;
        }
        else
        {
            fullFilenameTarget = fullFilenameTarget + extensionOriginal;
        }

        if (!File.Exists(fullFilenameTarget))
        {
            Image originalImage = Image.Load(fullFilenameOriginal);

            originalImage.Mutate(x => x.AutoOrient());

            if (height > 0 || width > 0)
            {
                ResizeOptions options = new()
                {
                    Size = new Size(width, height),
                    Mode = ResizeMode.Max
                };

                originalImage.Mutate(x => x.Resize(options));
            }

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
    }

    #endregion

    #region iConvertPicture

    public async Task ConvertPictureAsync(ConsumeContext<IMassConvertPictureCmd> consumerContext, string filename)
    {
        logger.LogInformation("Set status for convert picture begin");
        await databaseOperations.SetStatusConvertBeginAsync(consumerContext.Message.ConvertStatusId);

        logger.LogInformation(
            $"Converting picture to card format {appSettings.Value.CardSizeWidth} x {appSettings.Value.CardSizeHeight}");
        DoConvertion(appSettings.Value.DirectoryUploadPicture, filename, "png",
            appSettings.Value.CardSizeWidth, appSettings.Value.CardSizeHeight);

        logger.LogInformation(
            $"Converting picture to gallery format {appSettings.Value.GallerySizeWidth} x {appSettings.Value.GallerySizeHeight}");
        DoConvertion(appSettings.Value.DirectoryUploadPicture, filename, "png",
            appSettings.Value.GallerySizeWidth, appSettings.Value.GallerySizeHeight);

        logger.LogInformation(
            $"Converting picture to thumbnail gallery format {appSettings.Value.ThumbnailSizeWidth} x {appSettings.Value.ThumbnailSizeHeight}");
        DoConvertion(appSettings.Value.DirectoryUploadPicture, filename, "png",
            appSettings.Value.ThumbnailSizeWidth, appSettings.Value.ThumbnailSizeHeight);

        logger.LogInformation("Set status for convert picture end");
        await databaseOperations.SetStatusConvertEndAsync(consumerContext.Message.ConvertStatusId);
    }

    #endregion
}