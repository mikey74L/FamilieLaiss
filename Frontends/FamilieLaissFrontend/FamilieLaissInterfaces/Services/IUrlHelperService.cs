using FamilieLaissEnums;
using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissInterfaces.Services;

public interface IUrlHelperService
{
    string GetUrlForUploadPictureInfo(IUploadPictureModel model);

    string GetUrlForUploadPictureCard(IUploadPictureModel model);

    string GetUrlForUploadPictureThumbnail(IUploadPictureModel model);

    string GetUrlForUploadPictureGallery(IUploadPictureModel model);

    string GetUrlForUploadVideoCard(IUploadVideoModel model);

    string GetUrlForVideoResolutionIcon(EnumVideoResolutionIcon videoResolutionSize);
}