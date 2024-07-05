using FamilieLaissInterfaces.Models;

namespace FamilieLaissModels.Models;

public class AppSettings : IAppSettings
{
    public string UrlGraphQlHttp { get; set; } = string.Empty;
    public string UrlGraphQlWs { get; set; } = string.Empty;
    public string UrlPicture { get; set; } = string.Empty;
    public string UrlVideo { get; set; } = string.Empty;
    public string UrlVideoVtt { get; set; } = string.Empty;
    public int? PictureUploadWidthCard { get; set; }

    public int? PictureUploadHeightCard { get; set; }

    public int? PictureUploadWidthThumbnail { get; set; }

    public int? PictureUploadHeightThumbnail { get; set; }

    public int? PictureUploadWidthGallery { get; set; }

    public int? PictureUploadHeightGallery { get; set; }
}