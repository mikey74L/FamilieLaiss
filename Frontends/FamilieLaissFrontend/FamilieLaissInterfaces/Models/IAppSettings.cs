namespace FamilieLaissInterfaces.Models;

public interface IAppSettings
{
    string UrlGraphQlHttp { get; set; }
    string UrlGraphQlWs { get; set; }
    string UrlPicture { get; set; }
    string UrlVideo { get; set; }
    string UrlVideoVtt { get; set; }
    int? PictureUploadWidthCard { get; set; }

    int? PictureUploadHeightCard { get; set; }

    int? PictureUploadWidthThumbnail { get; set; }

    int? PictureUploadHeightThumbnail { get; set; }

    int? PictureUploadWidthGallery { get; set; }

    int? PictureUploadHeightGallery { get; set; }
}