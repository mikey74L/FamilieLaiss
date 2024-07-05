using DomainHelper.AbstractClasses;
using DomainHelper.Exceptions;
using FamilieLaissSharedObjects.Enums;
using HotChocolate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace FLBackEnd.Domain.Entities;

/// <summary>
/// Entity for representing the convert status of an upload video
/// </summary>
[GraphQLDescription("Video convert status")]
public class VideoConvertStatus : EntityBase<long>
{
    #region Properties

    /// <summary>
    /// The type of video for the converted video
    /// </summary>
    [GraphQLDescription("The type of video for the converted video")]
    public EnumVideoType VideoType { get; private set; }

    /// <summary>
    /// Current status of conversion
    /// </summary>
    [GraphQLDescription("The current state for the conversion")]
    public EnumVideoConvertStatus Status { get; private set; }

    /// <summary>
    /// Hour part for the convert time
    /// </summary>
    [GraphQLDescription("Hour part for the convert time")]
    public int? ConvertHour { get; private set; }

    /// <summary>
    /// Minute part for the convert time
    /// </summary>
    [GraphQLDescription("Minute part for the convert time")]
    public int? ConvertMinute { get; private set; }

    /// <summary>
    /// Second part for the convert time
    /// </summary>
    [GraphQLDescription("Second part for the convert time")]
    public int? ConvertSecond { get; private set; }

    /// <summary>
    /// Hour part for the remaining convert time
    /// </summary>
    [GraphQLDescription("The remaining time for current conversion in hours")]
    public int? ConvertRestHour { get; private set; }

    /// <summary>
    /// Minute part for the remaining convert time
    /// </summary>
    [GraphQLDescription("The remaining time for current conversion in minutes")]
    public int? ConvertRestMinute { get; private set; }

    /// <summary>
    /// Second part for the remaining convert time
    /// </summary>
    [GraphQLDescription("The remaining time for current conversion in seconds")]
    public int? ConvertRestSecond { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    [GraphQLDescription("Error message when conversion is in state \"Finished with errors\"")]
    [MaxLength(2000)]
    public string ErrorMessage { get; private set; } = string.Empty;

    /// <summary>
    /// Progress of current conversion
    /// </summary>
    [GraphQLDescription("The progress for current conversion")]
    public int? Progress { get; private set; }

    /// <summary>
    /// Timestamp when converting the video to MP4 begins
    /// </summary>
    [GraphQLDescription("Timestamp when converting the video to MP4 begins")]
    public DateTimeOffset? StartDateMp4 { get; private set; }

    /// <summary>
    /// Timestamp when converting the video to MP4 ends
    /// </summary>
    [GraphQLDescription("Timestamp when converting the video to MP4 ends")]
    public DateTimeOffset? FinishDateMp4 { get; private set; }

    /// <summary>
    /// Timestamp when converting the video to HLS 640 x 360 begins
    /// </summary>
    [GraphQLDescription("Timestamp when converting the video to HLS 640 x 360 begins")]
    public DateTimeOffset? StartDateMp4360 { get; private set; }

    /// <summary>
    /// Timestamp when converting the video to HLS 640 x 360 ends
    /// </summary>
    [GraphQLDescription("Timestamp when converting the video to HLS 640 x 360 ends")]
    public DateTimeOffset? FinishDateMp4360 { get; private set; }

    /// <summary>
    /// Timestamp when converting the video to HLS 842 x 480 begins
    /// </summary>
    [GraphQLDescription("Timestamp when converting the video to HLS 852 x 480 begins")]
    public DateTimeOffset? StartDateMp4480 { get; private set; }

    /// <summary>
    /// Timestamp when converting the video to HLS 842 x 480 ends
    /// </summary>
    [GraphQLDescription("Timestamp when converting the video to HLS 852 x 480 ends")]
    public DateTimeOffset? FinishDateMp4480 { get; private set; }

    /// <summary>
    /// Timestamp when converting the video to HLS 1280 x 720 begins
    /// </summary>
    [GraphQLDescription("Timestamp when converting the video to HLS 1280 x 720 begins")]
    public DateTimeOffset? StartDateMp4720 { get; private set; }

    /// <summary>
    /// Timestamp when converting the video to DASH / HLS 1280 x 720 ends
    /// </summary>
    [GraphQLDescription("Timestamp when converting the video to HLS 1280 x 720 ends")]
    public DateTimeOffset? FinishDateMp4720 { get; private set; }

    /// <summary>
    /// Timestamp when converting the video to DASH 1920 x 1080 begins
    /// </summary>
    [GraphQLDescription("Timestamp when converting the video to DASH 1920 x 1080 begins")]
    public DateTimeOffset? StartDateMp41080 { get; private set; }

    /// <summary>
    /// Timestamp when converting the video to DASH 1920 x 1080 ends
    /// </summary>
    [GraphQLDescription("Timestamp when converting the video to DASH 1920 x 1080 ends")]
    public DateTimeOffset? FinishDateMp41080 { get; private set; }

    /// <summary>
    /// Timestamp when converting the video to DASH 3840 x 2160 begins
    /// </summary>
    [GraphQLDescription("Timestamp when converting the video to DASH 3840 x 2160 begins")]
    public DateTimeOffset? StartDateMp42160 { get; private set; }

    /// <summary>
    /// Timestamp when converting the video to DASH 3840 x 2160 ends
    /// </summary>
    [GraphQLDescription("Timestamp when converting the video to DASH 3840 x 2160 ends")]
    public DateTimeOffset? FinishDateMp42160 { get; private set; }

    /// <summary>
    /// Timestamp when creating HLS content begins
    /// </summary>
    [GraphQLDescription("Timestamp when creating HLS content begins")]
    public DateTimeOffset? StartDateHls { get; private set; }

    /// <summary>
    /// Timestamp when creating HLS content ends
    /// </summary>
    [GraphQLDescription("Timestamp when creating HLS content ends")]
    public DateTimeOffset? FinishDateHls { get; private set; }

    /// <summary>
    /// Timestamp when creating thumbnail begins
    /// </summary>
    [GraphQLDescription("Timestamp when creating thumbnail begins")]
    public DateTimeOffset? StartDateThumbnail { get; private set; }

    /// <summary>
    /// Timestamp when creating thumbnail ends
    /// </summary>
    [GraphQLDescription("Timestamp when creating thumbnail ends")]
    public DateTimeOffset? FinishDateThumbnail { get; private set; }

    /// <summary>
    /// Timestamp when reading media info begins
    /// </summary>
    [GraphQLDescription("Timestamp when reading media info begins")]
    public DateTimeOffset? StartDateMediaInfo { get; private set; }

    /// <summary>
    /// Timestamp when reading media info ends
    /// </summary>
    [GraphQLDescription("Timestamp when reading media info ends")]
    public DateTimeOffset? FinishDateMediaInfo { get; private set; }

    /// <summary>
    /// Timestamp when creating VTT content begins
    /// </summary>
    [GraphQLDescription("Timestamp when creating VTT content begins")]
    public DateTimeOffset? StartDateVtt { get; private set; }

    /// <summary>
    /// Timestamp when creating VTT content ends
    /// </summary>
    [GraphQLDescription("Timestamp when creating VTT content ends")]
    public DateTimeOffset? FinishDateVtt { get; private set; }

    /// <summary>
    /// Timestamp when copy of converted files begins
    /// </summary>
    [GraphQLDescription("Timestamp when copy of converted files begins")]
    public DateTimeOffset? StartDateCopyConverted { get; private set; }

    /// <summary>
    /// Timestamp when copy of converted files ends
    /// </summary>
    [GraphQLDescription("Timestamp when copy of converted files ends")]
    public DateTimeOffset? FinishDateCopyConverted { get; private set; }

    /// <summary>
    /// Timestamp when delete temporary files begins
    /// </summary>
    [GraphQLDescription("Timestamp when delete temporary files begins")]
    public DateTimeOffset? StartDateDeleteTemp { get; private set; }

    /// <summary>
    /// Timestamp when delete temporary files ends
    /// </summary>
    [GraphQLDescription("Timestamp when delete temporary files ends")]
    public DateTimeOffset? FinishDateDeleteTemp { get; private set; }

    /// <summary>
    /// Timestamp when delete original file begins
    /// </summary>
    [GraphQLDescription("Timestamp when delete original file begins")]
    public DateTimeOffset? StartDateDeleteOriginal { get; private set; }

    /// <summary>
    /// Timestamp when delete original file ends
    /// </summary>
    [GraphQLDescription("Timestamp when delete original file ends")]
    public DateTimeOffset? FinishDateDeleteOriginal { get; private set; }

    /// <summary>
    /// Timestamp when conversion if preview image begins
    /// </summary>
    [GraphQLDescription("Timestamp when conversion of preview image begins")]
    public DateTimeOffset? StartDateConvertPicture { get; private set; }

    /// <summary>
    /// Timestamp when conversion of preview image ends
    /// </summary>
    [GraphQLDescription("Timestamp when conversion of preview image ends")]
    public DateTimeOffset? FinishDateConvertPicture { get; private set; }

    /// <summary>
    /// The related upload video for this status
    /// </summary>
    [GraphQLDescription("The upload video that is converted")]
    public UploadVideo UploadVideo { get; private set; } = default!;

    [GraphQLIgnore] public long UploadVideoId { get; private set; }

    #endregion

    #region Private Members

    private readonly ILazyLoader? _lazyLoader;

    #endregion

    #region C'tor

    /// <summary>
    /// Constructor without parameters would be used by EF-Core
    /// </summary>
    private VideoConvertStatus(ILazyLoader lazyLoader)
    {
        _lazyLoader = lazyLoader;
    }

    private VideoConvertStatus()
    {
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="uploadVideo">The Upload-Video this status item belongs too</param>
    public VideoConvertStatus(UploadVideo uploadVideo)
    {
        UploadVideo = uploadVideo;

        Status = EnumVideoConvertStatus.WaitingForConversion;
    }

    #endregion

    #region Domain Methods

    #region Media-Info

    [GraphQLIgnore]
    public void SetStatusStartMediaInfo()
    {
        Status = EnumVideoConvertStatus.ReadMediaInfoBegin;

        StartDateMediaInfo = DateTimeOffset.UtcNow;
    }

    [GraphQLIgnore]
    public void SetStatusEndMediaInfo()
    {
        if (!StartDateMediaInfo.HasValue)
        {
            throw new DomainException("No start date for media info exists.");
        }

        Status = EnumVideoConvertStatus.ReadMediaInfoEnd;

        FinishDateMediaInfo = DateTimeOffset.UtcNow;
    }

    #endregion

    #region Convert-MP4

    [GraphQLIgnore]
    public void SetStatusStartConvertMp4()
    {
        Status = EnumVideoConvertStatus.ConvertMp4VideoBegin;
        VideoType = EnumVideoType.Progressive;

        StartDateMp4 = DateTimeOffset.UtcNow;
    }

    [GraphQLIgnore]
    public void SetStatusEndConvertMp4()
    {
        if (!StartDateMp4.HasValue)
        {
            throw new DomainException("No start date for convert MP4 video exists.");
        }

        Status = EnumVideoConvertStatus.ConvertMp4VideoEnd;

        FinishDateMp4 = DateTimeOffset.UtcNow;
    }

    #endregion

    #region Convert-MP4 x 360

    [GraphQLIgnore]
    public void SetStatusStartConvertMP4_360()
    {
        Status = EnumVideoConvertStatus.Convert640x360Begin;
        VideoType = EnumVideoType.Hls;

        StartDateMp4360 = DateTimeOffset.UtcNow;
    }

    [GraphQLIgnore]
    public void SetStatusEndConvertMP4_360()
    {
        if (!StartDateMp4360.HasValue)
        {
            throw new DomainException("No start date for convert MP4 x 360 video exists.");
        }

        Status = EnumVideoConvertStatus.Convert640x360End;

        FinishDateMp4360 = DateTimeOffset.UtcNow;
    }

    #endregion

    #region Convert-MP4 x 480

    [GraphQLIgnore]
    public void SetStatusStartConvertMP4_480()
    {
        Status = EnumVideoConvertStatus.Convert852x480Begin;

        StartDateMp4480 = DateTimeOffset.UtcNow;
    }

    [GraphQLIgnore]
    public void SetStatusEndConvertMP4_480()
    {
        if (!StartDateMp4480.HasValue)
        {
            throw new DomainException("No start date for convert MP4 x 480 video exists.");
        }

        Status = EnumVideoConvertStatus.Convert852x480End;

        FinishDateMp4480 = DateTimeOffset.UtcNow;
    }

    #endregion

    #region Convert-MP4 x 720

    [GraphQLIgnore]
    public void SetStatusStartConvertMP4_720()
    {
        Status = EnumVideoConvertStatus.Convert1280x720Begin;

        StartDateMp4720 = DateTimeOffset.UtcNow;
    }

    [GraphQLIgnore]
    public void SetStatusEndConvertMP4_720()
    {
        if (!StartDateMp4720.HasValue)
        {
            throw new DomainException("No start date for convert MP4 x 720 video exists.");
        }

        Status = EnumVideoConvertStatus.Convert1280x720End;

        FinishDateMp4720 = DateTimeOffset.UtcNow;
    }

    #endregion

    #region Convert-MP4 X 1080

    [GraphQLIgnore]
    public void SetStatusStartConvertMP4_1080()
    {
        Status = EnumVideoConvertStatus.Convert1920x1080Begin;

        StartDateMp41080 = DateTimeOffset.UtcNow;
    }

    [GraphQLIgnore]
    public void SetStatusEndConvertMP4_1080()
    {
        if (!StartDateMp41080.HasValue)
        {
            throw new DomainException("No start date for convert MP4 x 1080 video exists.");
        }

        Status = EnumVideoConvertStatus.Convert1920x1080End;

        FinishDateMp41080 = DateTimeOffset.UtcNow;
    }

    #endregion

    #region Convert-MP4 X 2160

    [GraphQLIgnore]
    public void SetStatusStartConvertMP4_2160()
    {
        Status = EnumVideoConvertStatus.Convert3840x2160Begin;

        StartDateMp42160 = DateTimeOffset.UtcNow;
    }

    [GraphQLIgnore]
    public void SetStatusEndConvertMP4_2160()
    {
        if (!StartDateMp42160.HasValue)
        {
            throw new DomainException("No start date for convert MP4 x 2160 video exists.");
        }

        Status = EnumVideoConvertStatus.Convert3840x2160End;

        FinishDateMp42160 = DateTimeOffset.UtcNow;
    }

    #endregion

    #region Create HLS Content

    [GraphQLIgnore]
    public void SetStatusStartCreateHls()
    {
        Status = EnumVideoConvertStatus.CreateHlsBegin;

        StartDateHls = DateTimeOffset.UtcNow;
    }

    [GraphQLIgnore]
    public void SetStatusEndCreateHls()
    {
        if (!StartDateHls.HasValue)
        {
            throw new DomainException("No start date for create HLS content exists.");
        }

        Status = EnumVideoConvertStatus.CreateHlsEnd;

        FinishDateHls = DateTimeOffset.UtcNow;
    }

    #endregion

    #region Create Thumbnails

    [GraphQLIgnore]
    public void SetStatusStartCreateThumbnails()
    {
        Status = EnumVideoConvertStatus.CreatePreviewImageBegin;

        StartDateThumbnail = DateTimeOffset.UtcNow;
    }

    [GraphQLIgnore]
    public void SetStatusEndCreateThumbnails()
    {
        if (!StartDateThumbnail.HasValue)
        {
            throw new DomainException("No start date for create thumbnails exists.");
        }

        Status = EnumVideoConvertStatus.CreatePreviewImageEnd;

        FinishDateThumbnail = DateTimeOffset.UtcNow;
    }

    #endregion

    #region Create VTT-Files

    [GraphQLIgnore]
    public void SetStatusStartCreateVttFile()
    {
        Status = EnumVideoConvertStatus.CreateVttBegin;

        StartDateVtt = DateTimeOffset.UtcNow;
    }

    [GraphQLIgnore]
    public void SetStatusEndCreateVttFile()
    {
        if (!StartDateVtt.HasValue)
        {
            throw new DomainException("No start date for create VTT-File exists.");
        }

        Status = EnumVideoConvertStatus.CreateVttEnd;

        FinishDateVtt = DateTimeOffset.UtcNow;
    }

    #endregion

    #region Convert Picture

    [GraphQLIgnore]
    public void SetStatusStartConvertPicture()
    {
        Status = EnumVideoConvertStatus.ConvertPictureBegin;

        StartDateConvertPicture = DateTimeOffset.UtcNow;
    }

    [GraphQLIgnore]
    public void SetStatusEndConvertPicture()
    {
        if (!StartDateConvertPicture.HasValue)
        {
            throw new DomainException("No start date for convert picture exists.");
        }

        Status = EnumVideoConvertStatus.ConvertPictureEnd;

        FinishDateConvertPicture = DateTimeOffset.UtcNow;
    }

    #endregion

    #region Copy converted files

    [GraphQLIgnore]
    public void SetStatusStartCopyConvertedFiles()
    {
        Status = EnumVideoConvertStatus.CopyConvertedBegin;

        StartDateCopyConverted = DateTimeOffset.UtcNow;
    }

    [GraphQLIgnore]
    public void SetStatusEndCopyConvertedFiles()
    {
        if (!StartDateCopyConverted.HasValue)
        {
            throw new DomainException("No start date for copy converted files exists.");
        }

        Status = EnumVideoConvertStatus.CopyConvertedEnd;

        FinishDateCopyConverted = DateTimeOffset.UtcNow;
    }

    #endregion

    #region Delete temporary files

    [GraphQLIgnore]
    public void SetStatusStartDeleteTemporaryFiles()
    {
        Status = EnumVideoConvertStatus.DeleteConvertedBegin;

        StartDateDeleteTemp = DateTimeOffset.UtcNow;
    }

    [GraphQLIgnore]
    public void SetStatusEndDeleteTemporaryFiles()
    {
        if (!StartDateDeleteTemp.HasValue)
        {
            throw new DomainException("No start date for delete temporary files exists.");
        }

        Status = EnumVideoConvertStatus.DeleteConvertedEnd;

        FinishDateDeleteTemp = DateTimeOffset.UtcNow;
    }

    #endregion

    #region Delete original file

    [GraphQLIgnore]
    public void SetStatusStartDeleteOriginalFile()
    {
        Status = EnumVideoConvertStatus.DeleteOriginalBegin;

        StartDateDeleteOriginal = DateTimeOffset.UtcNow;
    }

    [GraphQLIgnore]
    public void SetStatusEndDeleteOriginalFile()
    {
        if (!StartDateDeleteOriginal.HasValue)
        {
            throw new DomainException("No start date for delete original file exists.");
        }

        Status = EnumVideoConvertStatus.DeleteOriginalEnd;

        FinishDateDeleteOriginal = DateTimeOffset.UtcNow;
    }

    #endregion

    [GraphQLIgnore]
    public void UpdateProgress(int progressCurrent, TimeSpan currentTime, TimeSpan restTime)
    {
        if (Status != EnumVideoConvertStatus.ConvertMp4VideoBegin &&
            Status != EnumVideoConvertStatus.Convert640x360Begin &&
            Status != EnumVideoConvertStatus.Convert852x480Begin &&
            Status != EnumVideoConvertStatus.Convert1280x720Begin &&
            Status != EnumVideoConvertStatus.Convert1920x1080Begin)
        {
            throw new DomainException("Progress could only be set in status for video conversion");
        }

        Progress = progressCurrent;

        ConvertHour = currentTime.Hours;
        ConvertMinute = currentTime.Minutes;
        ConvertSecond = currentTime.Seconds;

        ConvertRestHour = restTime.Hours;
        ConvertRestMinute = restTime.Minutes;
        ConvertRestSecond = restTime.Seconds;
    }

    [GraphQLIgnore]
    public async Task ConversionFinished()
    {
        if (_lazyLoader is not null)
        {
            await _lazyLoader.LoadAsync(this, navigationName: nameof(UploadVideo));
        }

        if (!StartDateMediaInfo.HasValue)
        {
            throw new DomainException("No start date for media info exists.");
        }

        if (!FinishDateMediaInfo.HasValue)
        {
            throw new DomainException("No finish date for media info exists.");
        }

        if (VideoType != EnumVideoType.NotSet)
        {
            switch (VideoType)
            {
                case EnumVideoType.Hls:
                    if (!StartDateMp4360.HasValue)
                    {
                        throw new DomainException("No start date for create MP4 x 360 exists");
                    }

                    if (!FinishDateMp4360.HasValue)
                    {
                        throw new DomainException("No finish date for create MP4 x 360 exists");
                    }

                    if (!StartDateMp4480.HasValue && UploadVideo.Height > 360)
                    {
                        throw new DomainException("No start date for create MP4 x 480 exists");
                    }

                    if (!FinishDateMp4480.HasValue && UploadVideo.Height > 360)
                    {
                        throw new DomainException("No finish date for create MP4 x 480 exists");
                    }

                    if (!StartDateMp4720.HasValue && UploadVideo.Height > 480)
                    {
                        throw new DomainException("No start date for create MP4 x 720 exists");
                    }

                    if (!FinishDateMp4720.HasValue && UploadVideo.Height > 480)
                    {
                        throw new DomainException("No finish date for create MP4 x 720 exists");
                    }

                    if (!StartDateMp41080.HasValue && UploadVideo.Height > 720)
                    {
                        throw new DomainException("No start date for create MP4 x 1080 exists");
                    }

                    if (!FinishDateMp41080.HasValue && UploadVideo.Height > 720)
                    {
                        throw new DomainException("No finish date for create MP4 x 1080 exists");
                    }

                    if (!StartDateMp42160.HasValue && UploadVideo.Height > 1080)
                    {
                        throw new DomainException("No start date for create MP4 x 2160 exists");
                    }

                    if (!FinishDateMp42160.HasValue && UploadVideo.Height > 1080)
                    {
                        throw new DomainException("No finish date for create MP4 x 2160 exists");
                    }

                    if (!StartDateHls.HasValue)
                    {
                        throw new DomainException("No start date for create HLS content exists");
                    }

                    if (!FinishDateHls.HasValue)
                    {
                        throw new DomainException("No finish date for create HLS content exists");
                    }

                    if (!StartDateDeleteTemp.HasValue)
                    {
                        throw new DomainException("No start date for delete temporary files exists");
                    }

                    if (!FinishDateDeleteTemp.HasValue)
                    {
                        throw new DomainException("No finish date for delete temporary files exists");
                    }

                    break;
                case EnumVideoType.Progressive:
                    if (!StartDateMp4.HasValue)
                    {
                        throw new DomainException("No start date for create MP4 exists");
                    }

                    if (!FinishDateMp4.HasValue)
                    {
                        throw new DomainException("No finish date for create MP4 exists");
                    }

                    break;
                case EnumVideoType.NotSet:
                    throw new DomainException("Video type info is not set correctly");
                default:
                    throw new DomainException("Video type info is not set correctly");
            }
        }
        else
        {
            throw new DomainException("Video type info is not set correctly");
        }

        if (!StartDateThumbnail.HasValue)
        {
            throw new DomainException("No start date for create thumbnails exists");
        }

        if (!FinishDateThumbnail.HasValue)
        {
            throw new DomainException("No finish date for create thumbnails exists");
        }

        if (!StartDateVtt.HasValue)
        {
            throw new DomainException("No start date for create VTT-File exists");
        }

        if (!FinishDateVtt.HasValue)
        {
            throw new DomainException("No finish date for create VTT-File exists");
        }

        if (!StartDateCopyConverted.HasValue)
        {
            throw new DomainException("No start date for copy converted files exists");
        }

        if (!FinishDateCopyConverted.HasValue)
        {
            throw new DomainException("No finish date for copy converted files exists");
        }

        if (!StartDateDeleteOriginal.HasValue)
        {
            throw new DomainException("No start date for delete original file exists");
        }

        if (!FinishDateDeleteOriginal.HasValue)
        {
            throw new DomainException("No finish date for delete original file exists");
        }

        if (!StartDateConvertPicture.HasValue)
        {
            throw new DomainException("No start date for convert picture exists");
        }

        if (!FinishDateConvertPicture.HasValue)
        {
            throw new DomainException("No finish date for convert picture exists");
        }

        Status = EnumVideoConvertStatus.SucessfullyConverted;
    }

    [GraphQLIgnore]
    public async Task ConversionFinishedWithError(string errorMessage)
    {
        Status = EnumVideoConvertStatus.ConvertedWithErrors;

        ErrorMessage = errorMessage;
    }

    [GraphQLIgnore]
    public void TransientError(string errorMessage)
    {
        Status = EnumVideoConvertStatus.TransientError;

        ErrorMessage = errorMessage;
    }

    #endregion

    #region Called from Change-Tracker

    [GraphQLIgnore]
    public override Task EntityAddedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
    {
        return Task.CompletedTask;
    }

    [GraphQLIgnore]
    public override Task EntityDeletedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
    {
        return Task.CompletedTask;
    }

    #endregion
}