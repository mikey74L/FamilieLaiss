using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissMappingExtensions.UploadPicture;
using FamilieLaissModels.Models.PictureConvert;

namespace FamilieLaissMappingExtensions.PictureConvertStatus;

public static class PictureConvertStatusMappingExtensions
{
    public static IPictureConvertStatusModel Map(this IFrPictureConvertStatusError sourceItem)
    {
        var newItem = new PictureConvertStatusModel()
        {
            Id = sourceItem.Id,
            StartDateInfo = sourceItem.StartDateInfo,
            ErrorMessage = sourceItem.ErrorMessage,
            UploadPicture = sourceItem.UploadPicture.Map()
        };

        return newItem;
    }

    public static IPictureConvertStatusModel Map(this IFrPictureConvertStatusWaiting sourceItem)
    {
        var newItem = new PictureConvertStatusModel()
        {
            Id = sourceItem.Id,
            UploadPicture = sourceItem.UploadPicture.Map()
        };

        return newItem;
    }

    public static IPictureConvertStatusModel Map(this IFrPictureConvertStatusSuccess sourceItem)
    {
        var newItem = new PictureConvertStatusModel()
        {
            Id = sourceItem.Id,
            FinishDateConvert = sourceItem.FinishDateConvert,
            UploadPicture = sourceItem.UploadPicture.Map()
        };

        return newItem;
    }

    public static IPictureConvertStatusModel Map(this IFrPictureConvertStatusCurrent sourceItem)
    {
        var newItem = new PictureConvertStatusModel()
        {
            Id = sourceItem.Id,
            FinishDateExif = sourceItem.FinishDateExif,
            FinishDateInfo = sourceItem.FinishDateInfo,
            StartDateConvert = sourceItem.StartDateConvert,
            StartDateExif = sourceItem.StartDateExif,
            StartDateInfo = sourceItem.StartDateInfo,
            Status = sourceItem.Status,
            FinishDateConvert = sourceItem.FinishDateConvert,
            UploadPicture = sourceItem.UploadPicture.Map()
        };

        return newItem;
    }
}
