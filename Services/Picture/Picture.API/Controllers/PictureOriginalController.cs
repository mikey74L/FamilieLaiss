using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using Picture.API.Models;

namespace Picture.API.Controllers;

/// <summary>
/// API-Controller for picture in original quality
/// </summary>
[ApiController]
[Route("[controller]")]
public class PictureOriginalController(IOptions<AppSettings> appSettings, ILogger<PictureOriginalController> logger)
    : ControllerBase
{
    #region Get

    [HttpGet("{folder}/{filename}")]
    public PhysicalFileResult Get(string folder, string filename)
    {
        logger.LogInformation("Action called with following parameters:");
        logger.LogDebug($"Folder  : {folder}");
        logger.LogDebug($"Filename: {filename}");

        try
        {
            logger.LogDebug("Create full filename");
            var fullFilename = Path.Combine(appSettings.Value.RootFolder, folder, filename);
            logger.LogDebug($"Full-Filename: {fullFilename}");

            string contentType;
            var provider = new FileExtensionContentTypeProvider();
            logger.LogDebug($"Get MIME-Type for: {fullFilename}");
            provider.TryGetContentType(fullFilename, out contentType);
            logger.LogDebug($"MIME-Type to use: {contentType}");

            logger.LogDebug($"Return physical file for {fullFilename}");
            return PhysicalFile(fullFilename, contentType ?? throw new InvalidOperationException());
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Unexpected error occurred");
            throw;
        }
    }

    #endregion
}