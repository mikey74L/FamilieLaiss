namespace Upload.API.Models;

/// <summary>
/// App-Settings - Class
/// </summary>
public class AppSettings
{
    #region RabbitMQ
    #region Endpoints

    /// <summary>
    /// Endpoint for Upload-Service
    /// </summary>
    public string EndpointUploadService { get; set; } = string.Empty;

    #endregion
    #endregion

    #region File-Upload

    public string TempDirectoryUploadPicture { get; set; } = string.Empty;
    public string TempDirectoryUploadVideo { get; set; } = string.Empty;
    public string DirectoryUploadPicture { get; set; } = string.Empty;
    public string DirectoryUploadVideo { get; set; } = string.Empty;

    #endregion

    #region Google
    public string GoogleMicroserviceVersion { get; set; } = string.Empty;
    #endregion
    
    #region UploadPicture

    public int CardSizeWidth { get; set; }
    public int CardSizeHeight { get; set; }

    public int GallerySizeWidth { get; set; }
    public int GallerySizeHeight { get; set; }
    
    public int ThumbnailSizeWidth { get; set; }
    public int ThumbnailSizeHeight { get; set; }
    #endregion
}