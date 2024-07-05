using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.Extensions;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissSharedObjects.Extensions;

namespace FamilieLaissModels.Models.VideoConvert;

public class VideoConvertStatusModel : IVideoConvertStatusModel
{
    public long Id { get; set; }

    public EnumVideoType? VideoType { get; set; }

    public EnumVideoConvertStatus? Status { get; set; }

    public int? ConvertHour { get; set; }

    public int? ConvertMinute { get; set; }

    public int? ConvertSecond { get; set; }

    public int? ConvertRestHour { get; set; }

    public int? ConvertRestMinute { get; set; }

    public int? ConvertRestSecond { get; set; }

    public string? ErrorMessage { get; set; }

    public int? Progress { get; set; }

    public DateTimeOffset? StartDateMp4 { get; set; }

    public DateTimeOffset? FinishDateMp4 { get; set; }

    public DateTimeOffset? StartDateMp4360 { get; set; }

    public DateTimeOffset? FinishDateMp4360 { get; set; }

    public DateTimeOffset? StartDateMp4480 { get; set; }

    public DateTimeOffset? FinishDateMp4480 { get; set; }

    public DateTimeOffset? StartDateMp4720 { get; set; }

    public DateTimeOffset? FinishDateMp4720 { get; set; }

    public DateTimeOffset? StartDateMp41080 { get; set; }

    public DateTimeOffset? FinishDateMp41080 { get; set; }

    public DateTimeOffset? StartDateMp42160 { get; set; }

    public DateTimeOffset? FinishDateMp42160 { get; set; }

    public DateTimeOffset? StartDateHls { get; set; }

    public DateTimeOffset? FinishDateHls { get; set; }

    public DateTimeOffset? StartDateThumbnail { get; set; }

    public DateTimeOffset? FinishDateThumbnail { get; set; }

    public DateTimeOffset? StartDateMediaInfo { get; set; }

    public DateTimeOffset? FinishDateMediaInfo { get; set; }

    public DateTimeOffset? StartDateVtt { get; set; }

    public DateTimeOffset? FinishDateVtt { get; set; }

    public DateTimeOffset? StartDateCopyConverted { get; set; }

    public DateTimeOffset? FinishDateCopyConverted { get; set; }

    public DateTimeOffset? StartDateDeleteTemp { get; set; }

    public DateTimeOffset? FinishDateDeleteTemp { get; set; }

    public DateTimeOffset? StartDateDeleteOriginal { get; set; }

    public DateTimeOffset? FinishDateDeleteOriginal { get; set; }

    public DateTimeOffset? StartDateConvertPicture { get; set; }

    public DateTimeOffset? FinishDateConvertPicture { get; set; }

    public IUploadVideoModel? UploadVideo { get; set; }

    public string ElapsedTime
    {
        get
        {
            if (ConvertHour.HasValue && ConvertMinute.HasValue && ConvertSecond.HasValue)
            {
                return $"{ConvertHour.Value:00}:{ConvertMinute.Value:00}:{ConvertSecond.Value:00}";
            }

            return "";
        }
    }

    public string RemainingDuration
    {
        get
        {
            if (ConvertRestHour.HasValue && ConvertRestMinute.HasValue && ConvertRestSecond.HasValue)
            {
                return $"{ConvertRestHour.Value:00}:{ConvertRestMinute.Value:00}:{ConvertRestSecond.Value:00}";
            }

            return "";
        }
    }

    public string Resolution => UploadVideo is not null ? $"{UploadVideo.Width} x {UploadVideo.Height}" : "";

    public string Duration =>
        UploadVideo is { DurationHour: not null, DurationMinute: not null, DurationSecond: not null }
            ? $"{UploadVideo.DurationHour.Value:00}:{UploadVideo.DurationMinute.Value:00}:{UploadVideo.DurationSecond.Value:00}"
            : "";

    public string VideoConvertStatusText
    {
        get
        {
            var targetEnum =
                Status?.MapEnum<EnumVideoConvertStatus, FamilieLaissSharedObjects.Enums.EnumVideoConvertStatus>();

            if (targetEnum is not null)
            {
                return targetEnum.Description();
            }

            return "Missing Enum Description Attribute";
        }
    }

    public IVideoConvertStatusModel Clone()
    {
        throw new NotImplementedException("Cloning this model is not implemented.");
    }

    public void TakeOverValues(IVideoConvertStatusModel sourceModel)
    {
        throw new NotImplementedException("Take over values for this model is not implemented.");
    }
}