using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using Picture.API.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Picture.API.Controllers;

/// <summary>
/// API-Controller for user settings
/// </summary>
[ApiController]
[Route("[controller]")]
public class PictureConvertController(IOptions<AppSettings> appSettings, ILogger<PictureConvertController> logger)
    : ControllerBase
{
    #region API

    /// <summary>
    /// Get Picture with conversion
    /// </summary>
    /// <param name="folder">The physical folder where the picture is stored</param>
    /// <param name="filename">The filename for the picture</param>
    /// <param name="mode">The mode for the picture</param>
    /// <param name="height">The height for the picture</param>
    /// <param name="width">The width for the picture</param>
    /// <returns>The resized and formatted picture</returns>
    [HttpGet("{folder}/{filename}")]
    public PhysicalFileResult Get(string folder, string filename, [FromQuery] string mode = "jpeg",
        [FromQuery] int height = 0, [FromQuery] int width = 0)
    {
        logger.LogInformation("Action called with following parameters: ");
        logger.LogDebug($"Folder  : {folder}");
        logger.LogDebug($"Filename: {filename}");
        logger.LogDebug($"Mode    : {mode}");
        logger.LogDebug($"Height  : {height}");
        logger.LogDebug($"Width   : {width}");

        var modeOriginal = "";
        var extensionTarget = "";

        //Build Full-Filename for Picture
        try
        {
            logger.LogDebug("Generate full filename");
            var fullFilename = System.IO.Path.Combine(appSettings.Value.RootFolder, folder,
                System.IO.Path.GetFileNameWithoutExtension(filename));
            logger.LogDebug($"Full-Filename: {fullFilename}");
            var extensionOriginal = System.IO.Path.GetExtension(filename);
            logger.LogDebug($"ExtensionOriginal: {extensionOriginal}");
            var fullFilenameOriginal = fullFilename + extensionOriginal;
            logger.LogDebug($"FullFilenameOriginal: {fullFilenameOriginal}");
            switch (extensionOriginal.ToString().ToUpper())
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

            logger.LogDebug($"ModeOriginal: {modeOriginal}");
            string fullFilenameTarget;
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

            logger.LogDebug($"ExtensionTarget: {extensionTarget}");
            if (mode.ToUpper() != modeOriginal.ToUpper())
            {
                fullFilenameTarget = fullFilenameTarget + extensionTarget;
            }
            else
            {
                fullFilenameTarget = fullFilenameTarget + extensionOriginal;
            }

            logger.LogDebug($"FullFilenameTarget: {fullFilenameTarget}");

            logger.LogDebug($"Check if filename \"{fullFilenameTarget}\" already exists");
            if (!System.IO.File.Exists(fullFilenameTarget))
            {
                logger.LogDebug("Target file not exist. Loading original image");
                var originalImage = Image.Load(fullFilenameOriginal);

                // Perform auto orientation for the image based on the EXIF information
                // Must be done before resizing, otherwise rotated images won't match the maximum height and width specifications
                logger.LogDebug("Make auto orientation from EXIF-Info");
                originalImage.Mutate(x => x.AutoOrient());

                // Perform resizing for the image if a height and width have been specified
                if (height > 0 || width > 0)
                {
                    var options = new ResizeOptions
                    {
                        Size = new Size(width, height),
                        Mode = ResizeMode.Max
                    };

                    logger.LogDebug($"Making Resize for width = {width} and height = {height}");
                    originalImage.Mutate(x => x.Resize(options));
                }
                else
                {
                    logger.LogDebug("Resize not necessary");
                }

                switch (mode)
                {
                    case "png":
                        logger.LogDebug("Transform picture to png");
                        using (var ms = new FileStream(fullFilenameTarget, FileMode.OpenOrCreate))
                        {
                            originalImage.SaveAsPng(ms);
                        }

                        break;
                    case "jpeg":
                        logger.LogDebug("Transform picture to jpeg");
                        using (var ms = new FileStream(fullFilenameTarget, FileMode.OpenOrCreate))
                        {
                            originalImage.SaveAsJpeg(ms);
                        }

                        break;
                    case "bmp":
                        logger.LogDebug("Transform picture to bmp");
                        using (var ms = new FileStream(fullFilenameTarget, FileMode.OpenOrCreate))
                        {
                            originalImage.SaveAsBmp(ms);
                        }

                        break;
                    default:
                        logger.LogDebug("Transform picture to jpeg because of default");
                        using (var ms = new FileStream(fullFilenameTarget, FileMode.OpenOrCreate))
                        {
                            originalImage.SaveAsJpeg(ms);
                        }

                        break;
                }
            }
            else
            {
                logger.LogDebug("Target file already exist. Serving existing file.");
            }

            string contentType;
            var provider = new FileExtensionContentTypeProvider();
            logger.LogDebug($"Get MIME-Type for {fullFilenameTarget}");
            provider.TryGetContentType(fullFilenameTarget, out contentType);
            logger.LogDebug($"MIME-Type is {contentType}");

            logger.LogDebug($"Return physical file for {fullFilenameTarget}");
            return PhysicalFile(fullFilenameTarget, contentType);
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Unexpected error occurred");
            throw;
        }
    }

    #endregion
}