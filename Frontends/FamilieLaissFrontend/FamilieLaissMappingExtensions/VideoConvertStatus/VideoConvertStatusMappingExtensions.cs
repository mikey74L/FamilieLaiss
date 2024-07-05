using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissMappingExtensions.UploadPicture;
using FamilieLaissMappingExtensions.UploadVideo;
using FamilieLaissModels.Models.VideoConvert;

namespace FamilieLaissMappingExtensions.VideoConvertStatus;

public static class VideoConvertStatusMappingExtensions
{
    public static IVideoConvertStatusModel Map(this IFrVideoConvertStatusError sourceItem)
    {
        var newItem = new VideoConvertStatusModel()
        {
            Id = sourceItem.Id,
            StartDateMediaInfo = sourceItem.StartDateMediaInfo,
            ErrorMessage = sourceItem.ErrorMessage,
            UploadVideo = sourceItem.UploadVideo.Map()
        };

        return newItem;
    }

    public static IVideoConvertStatusModel Map(this IFrVideoConvertStatusWaiting sourceItem)
    {
        var newItem = new VideoConvertStatusModel()
        {
            Id = sourceItem.Id,
            UploadVideo = sourceItem.UploadVideo.Map()
        };

        return newItem;
    }

    public static IVideoConvertStatusModel Map(this IFrVideoConvertStatusSuccess sourceItem)
    {
        var newItem = new VideoConvertStatusModel()
        {
            Id = sourceItem.Id,
            FinishDateConvertPicture = sourceItem.FinishDateConvertPicture,
            UploadVideo = sourceItem.UploadVideo.Map()
        };

        return newItem;
    }

    public static IVideoConvertStatusModel Map(this IFrVideoConvertStatusCurrent sourceItem)
    {
        var newItem = new VideoConvertStatusModel()
        {
            Id = sourceItem.Id,
            FinishDateConvertPicture = sourceItem.FinishDateConvertPicture,
            ConvertHour = sourceItem.ConvertHour,
            ConvertMinute = sourceItem.ConvertMinute,
            ConvertSecond = sourceItem.ConvertSecond,
            ConvertRestHour = sourceItem.ConvertRestHour,
            ConvertRestMinute = sourceItem.ConvertRestMinute,
            ConvertRestSecond = sourceItem.ConvertRestSecond,
            FinishDateCopyConverted = sourceItem.FinishDateCopyConverted,
            FinishDateDeleteOriginal = sourceItem.FinishDateDeleteOriginal,
            Status = sourceItem.Status,
            FinishDateDeleteTemp = sourceItem.FinishDateDeleteTemp,
            FinishDateHls = sourceItem.FinishDateHls,
            FinishDateMp4 = sourceItem.FinishDateMp4,
            FinishDateMediaInfo = sourceItem.FinishDateMediaInfo,
            FinishDateMp41080 = sourceItem.FinishDateMp41080,
            FinishDateMp4480 = sourceItem.FinishDateMp4480,
            FinishDateMp4720 = sourceItem.FinishDateMp4720,
            FinishDateThumbnail = sourceItem.FinishDateThumbnail,
            FinishDateVtt = sourceItem.FinishDateVtt,
            Progress = sourceItem.Progress,
            FinishDateMp4360 = sourceItem.FinishDateMp4360,
            StartDateConvertPicture = sourceItem.StartDateConvertPicture,
            StartDateThumbnail = sourceItem.StartDateThumbnail,
            VideoType = sourceItem.VideoType,
            StartDateVtt = sourceItem.StartDateVtt,
            StartDateMp4720 = sourceItem.StartDateMp4720,
            FinishDateMp42160 = sourceItem.FinishDateMp42160,
            StartDateCopyConverted = sourceItem.StartDateCopyConverted,
            StartDateDeleteOriginal = sourceItem.StartDateDeleteOriginal,
            StartDateDeleteTemp = sourceItem.StartDateDeleteTemp,
            StartDateMediaInfo = sourceItem.StartDateMediaInfo,
            StartDateHls = sourceItem.StartDateHls,
            StartDateMp4 = sourceItem.StartDateMp4,
            StartDateMp41080 = sourceItem.StartDateMp41080,
            StartDateMp42160 = sourceItem.StartDateMp42160,
            StartDateMp4360 = sourceItem.StartDateMp4360,
            StartDateMp4480 = sourceItem.StartDateMp4360,
            UploadVideo = sourceItem.UploadVideo.Map()
        };

        return newItem;
    }
}
