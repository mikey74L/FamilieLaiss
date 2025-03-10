namespace Upload.API.Interfaces;

public interface IFolderHelperService
{
    string GetApplicationPath();

    string GetFolderNameForTempUploadPicture();
    string GetFullFilenameForTempUploadPicture(string filename);

    string GetFolderNameForUploadPicture();
    string GetFullFilenameForUploadPicture(string filename);

    string GetFullFilenameForUploadPictureCard(string originalFilename);
    string GetFullFilenameForUploadPictureGallery(string originalFilename);
    string GetFullFilenameForUploadPictureThumbnail(string originalFilename);
}