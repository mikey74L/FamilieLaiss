using FamilieLaissGraphQlClientLibrary;

namespace FamilieLaissInterfaces.Models.Data;

public interface IVideoConvertStatusModel : IBaseModel<IVideoConvertStatusModel>
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

    public string RemainingDuration { get; }

    public string ElapsedTime { get; }

    public string Resolution { get; }

    public string Duration { get; }

    public string VideoConvertStatusText { get; }
}