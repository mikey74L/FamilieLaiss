using Microsoft.Extensions.Options;
using System.Reflection;
using Upload.API.Interfaces;
using Upload.API.Models;

namespace Upload.API.Services;

public class FolderHelperService(IOptions<AppSettings> appSettings) : IFolderHelperService
{
    #region Implementation of IFolderHelperService

    /// <inheritdoc />
    public string GetApplicationPath()
    {
        return System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "";
    }

    /// <inheritdoc />
    public string GetFolderNameForTempUploadPicture()
    {
        return System.IO.Path.Combine(GetApplicationPath(), appSettings.Value.TempDirectoryUploadPicture);
    }

    /// <inheritdoc />
    public string GetFullFilenameForTempUploadPicture(string filename)
    {
        return System.IO.Path.Combine(GetFolderNameForTempUploadPicture(), filename);
    }

    /// <inheritdoc />
    public string GetFolderNameForUploadPicture()
    {
        return System.IO.Path.Combine(GetApplicationPath(), appSettings.Value.DirectoryUploadPicture);
    }

    /// <inheritdoc />
    public string GetFullFilenameForUploadPicture(string filename)
    {
        return System.IO.Path.Combine(GetFolderNameForUploadPicture(), filename);
    }

    /// <inheritdoc />
    public string GetFullFilenameForUploadPictureCard(string originalFilename)
    {
        var filenameWithOutExtension = System.IO.Path.GetFileNameWithoutExtension(originalFilename);
        return System.IO.Path.Combine(GetFolderNameForUploadPicture(),
            $"{filenameWithOutExtension}_{appSettings.Value.CardSizeHeight}_{appSettings.Value.CardSizeWidth}.png");
    }

    /// <inheritdoc />
    public string GetFullFilenameForUploadPictureGallery(string originalFilename)
    {
        var filenameWithOutExtension = System.IO.Path.GetFileNameWithoutExtension(originalFilename);
        return System.IO.Path.Combine(GetFolderNameForUploadPicture(),
            $"{filenameWithOutExtension}_{appSettings.Value.GallerySizeHeight}_{appSettings.Value.GallerySizeWidth}.png");
    }

    /// <inheritdoc />
    public string GetFullFilenameForUploadPictureThumbnail(string originalFilename)
    {
        var filenameWithOutExtension = System.IO.Path.GetFileNameWithoutExtension(originalFilename);
        return System.IO.Path.Combine(GetFolderNameForUploadPicture(),
            $"{filenameWithOutExtension}_{appSettings.Value.ThumbnailSizeHeight}_{appSettings.Value.ThumbnailSizeWidth}.png");
    }

    #endregion
}