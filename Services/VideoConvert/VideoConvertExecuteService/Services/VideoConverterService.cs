using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FamilieLaissMassTransitDefinitions.Contracts.Commands;
using FamilieLaissMassTransitDefinitions.Contracts.Events;
using FamilieLaissMassTransitDefinitions.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceHelper.Exceptions;
using Tocronx.SimpleAsync;
using VideoConvertExecuteService.Interfaces;
using VideoConvertExecuteService.Models;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Events;

namespace VideoConvertExecuteService.Services;

public partial class VideoConverterService(
    ILogger<VideoConverterService> logger,
    IOptions<AppSettings> appSettings,
    IDatabaseOperations databaseOperations) : IVideoConverter
{
    #region Members

    private MediaInfoData _metadata;
    private long _convertStatusId;
    private string _filenameSource;
    private long _currentConvertingId;
    private DateTime _startDateConversion;
    private ConsumeContext<IConvertVideoCmd> _consumerContext;

    #endregion

    #region Private Methods

    private async Task<string> Convert_MP4_Video_Streaming(int height)
    {
        var actualStep = 0;
        var commandLineParameter = "";
        var destinationFilename = "";
        var segmentFileNamePatternHls = "";

        try
        {
            actualStep = 1;
            switch (height)
            {
                case 360:
                    logger.LogInformation("Set Status in database to 'converting mp4 for 640 x 360'");
                    await databaseOperations.SetStatusConvert640X360BeginAsync(_convertStatusId);
                    break;
                case 480:
                    logger.LogInformation("Set Status in database to 'converting mp4 for 852 x 480'");
                    await databaseOperations.SetStatusConvert852X480BeginAsync(_convertStatusId);
                    break;
                case 720:
                    logger.LogInformation("Set Status in database to 'converting mp4 for 1280 x 720'");
                    await databaseOperations.SetStatusConvert1280X720BeginAsync(_convertStatusId);
                    break;
                case 1080:
                    logger.LogInformation("Set Status in database to 'converting mp4 for 1920 x 1080'");
                    await databaseOperations.SetStatusConvert1920X1080BeginAsync(_convertStatusId);
                    break;
                case 2160:
                    logger.LogInformation("Set Status in database to 'converting mp4 for 3840 x 2160'");
                    await databaseOperations.SetStatusConvert3840X2160BeginAsync(_convertStatusId);
                    break;
            }

            actualStep = 2;
            switch (height)
            {
                case 360:
                    logger.LogInformation("Creating MP4-Video for 640 x 360");
                    destinationFilename = Path.Combine(appSettings.Value.DirectoryConvertVideo,
                        Path.GetFileNameWithoutExtension(_filenameSource) + "_360p.m3u8");
                    segmentFileNamePatternHls = Path.Combine(appSettings.Value.DirectoryConvertVideo,
                        Path.GetFileNameWithoutExtension(_filenameSource) + "_360p_%03d.ts");
                    break;
                case 480:
                    logger.LogInformation("Creating MP4-Video for 852 x 480");
                    destinationFilename = Path.Combine(appSettings.Value.DirectoryConvertVideo,
                        Path.GetFileNameWithoutExtension(_filenameSource) + "_480p.m3u8");
                    segmentFileNamePatternHls = Path.Combine(appSettings.Value.DirectoryConvertVideo,
                        Path.GetFileNameWithoutExtension(_filenameSource) + "_480p_%03d.ts");
                    break;
                case 720:
                    logger.LogInformation("Creating MP4-Video for 1280 x 720");
                    destinationFilename = Path.Combine(appSettings.Value.DirectoryConvertVideo,
                        Path.GetFileNameWithoutExtension(_filenameSource) + "_720p.m3u8");
                    segmentFileNamePatternHls = Path.Combine(appSettings.Value.DirectoryConvertVideo,
                        Path.GetFileNameWithoutExtension(_filenameSource) + "_720p_%03d.ts");
                    break;
                case 1080:
                    logger.LogInformation("Creating MP4-Video for 1920 x 1080");
                    destinationFilename = Path.Combine(appSettings.Value.DirectoryConvertVideo,
                        Path.GetFileNameWithoutExtension(_filenameSource) + "_1080p.m3u8");
                    segmentFileNamePatternHls = Path.Combine(appSettings.Value.DirectoryConvertVideo,
                        Path.GetFileNameWithoutExtension(_filenameSource) + "_1080p_%03d.ts");
                    break;
                case 2160:
                    logger.LogInformation("Creating MP4-Video for 3840 x 2160");
                    destinationFilename = Path.Combine(appSettings.Value.DirectoryConvertVideo,
                        Path.GetFileNameWithoutExtension(_filenameSource) + "_2160p.m3u8");
                    segmentFileNamePatternHls = Path.Combine(appSettings.Value.DirectoryConvertVideo,
                        Path.GetFileNameWithoutExtension(_filenameSource) + "_2160p_%03d.ts");
                    break;
            }

            logger.LogDebug("Create media info");
            var mediaInfo = await FFmpeg.GetMediaInfo($@"{_filenameSource}");

            logger.LogDebug("Create video stream");
            var videoStream = mediaInfo.VideoStreams.First();

            logger.LogDebug("Create audio stream");
            var audioStream = mediaInfo.AudioStreams.First();

            logger.LogDebug("Set video codec");
            videoStream.SetCodec(VideoCodec.h264);

            switch (height)
            {
                case 360:
                    videoStream.SetSize(VideoSize.Nhd);
                    break;
                case 480:
                    videoStream.SetSize(VideoSize.Hd480);
                    break;
                case 720:
                    videoStream.SetSize(VideoSize.Hd720);
                    break;
                case 1080:
                    videoStream.SetSize(VideoSize.Hd1080);
                    break;
                case 2160:
                    videoStream.SetSize(VideoSize._4K);
                    break;
            }

            audioStream.SetCodec(AudioCodec.aac);
            audioStream.SetSampleRate(48000);
            switch (height)
            {
                case 360:
                    audioStream.SetBitrate(96000);
                    break;
                case 480:
                    audioStream.SetBitrate(128000);
                    break;
                case 720:
                    audioStream.SetBitrate(128000);
                    break;
                case 1080:
                    audioStream.SetBitrate(192000);
                    break;
                case 2160:
                    audioStream.SetBitrate(192000);
                    break;
            }

            switch (height)
            {
                case 360:
                    videoStream.SetBitrate(800000, 856000, 1200000);
                    break;
                case 480:
                    videoStream.SetBitrate(1400000, 1498000, 2100000);
                    break;
                case 720:
                    videoStream.SetBitrate(5300000, 5398000, 4200000);
                    break;
                case 1080:
                    videoStream.SetBitrate(12000000, 12990000, 7500000);
                    break;
                case 2160:
                    videoStream.SetBitrate(30000000, 39889999, 9500000);
                    break;
            }

            if (_metadata.ScanType == "Interlaced")
            {
                if (_metadata.ScanOrder == "TFF")
                {
                    logger.LogDebug("Interlaced: Yes  / Mode: Top Field First");
                    commandLineParameter += $" -vf \"yadif=0\"";
                }
                else
                {
                    logger.LogDebug("Interlaced: Yes  / Mode: Bottom Field First");
                    commandLineParameter += $" -vf \"yadif=1\"";
                }
            }

            //Setting command line parameter for Keyframes
            commandLineParameter += " -g 48";

            //Setting special parameter for HLS
            commandLineParameter +=
                $" -profile:v main -crf 20 -sc_threshold 0 -keyint_min 48 -hls_time {appSettings.Value.SegmentSizeHls} -hls_playlist_type vod -hls_segment_filename \"{segmentFileNamePatternHls}\"";

            var conversion = FFmpeg.Conversions.New()
                .AddStream(videoStream)
                .AddStream(audioStream)
                .SetPreset(ConversionPreset.Slow);

            if (appSettings.Value.UsedThreads > Environment.ProcessorCount)
            {
                logger.LogDebug($"Using {Environment.ProcessorCount} threads for encoding");
                conversion.UseMultiThread(Environment.ProcessorCount);
            }
            else
            {
                logger.LogDebug($"Using {appSettings.Value.UsedThreads} threads for encoding");
                conversion.UseMultiThread(appSettings.Value.UsedThreads);
            }

            logger.LogDebug($"Destination filename: {destinationFilename}");
            conversion.SetOutput(destinationFilename);

            conversion.AddParameter(commandLineParameter);
            logger.LogDebug($"Used Commandline-Parameter: {commandLineParameter}");

            _currentConvertingId = _convertStatusId;
            _startDateConversion = DateTime.Now;
            conversion.OnProgress += WriteProgress;

            actualStep = 3;
            await conversion.Start();

            conversion.OnProgress -= WriteProgress;

            actualStep = 4;
            switch (height)
            {
                case 360:
                    logger.LogInformation("Set Status in database to 'MP4 video successfully converted for 640 x 360'");
                    await databaseOperations.SetStatusConvert640X360EndAsync(_convertStatusId);
                    break;
                case 480:
                    logger.LogInformation("Set Status in database to 'MP4 video successfully converted for 852 x 480'");
                    await databaseOperations.SetStatusConvert852X480EndAsync(_convertStatusId);
                    break;
                case 720:
                    logger.LogInformation(
                        "Set Status in database to 'MP4 video successfully converted for 1280 x 720'");
                    await databaseOperations.SetStatusConvert1280X720EndAsync(_convertStatusId);
                    break;
                case 1080:
                    logger.LogInformation(
                        "Set Status in database to 'MP4 video successfully converted for 1920 x 1080'");
                    await databaseOperations.SetStatusConvert1920X1080EndAsync(_convertStatusId);
                    break;
                case 2160:
                    logger.LogInformation(
                        "Set Status in database to 'MP4 video successfully converted for 3840 x 2160'");
                    await databaseOperations.SetStatusConvert3840X2160EndAsync(_convertStatusId);
                    break;
            }

            return destinationFilename;
        }
        catch (Exception ex)
        {
            switch (actualStep)
            {
                case 1:
                    throw new ServiceException("Error!!! Could not set status to 'converting mp4' in database.", ex);
                case 2:
                    throw new ServiceException("Error!!! Could not set ffmpeg parameter for mp4 video.", ex);
                case 3:
                    throw new ServiceException("Error!!! Could not start ffmpeg process for converting mp4 video.", ex);
                case 4:
                    throw new ServiceException("Error!!! Could not set status to 'mp4 converted' in database.", ex);
                default:
                    throw new ServiceException("Error!!! Could not convert mp4 video.", ex);
            }
        }
    }

    private async Task<string> Convert_MP4_Video(int height)
    {
        var actualStep = 0;
        var commandLineParameter = "";

        try
        {
            actualStep = 1;
            logger.LogInformation("Set Status in database to 'converting mp4'");
            await databaseOperations.SetStatusConvertMp4BeginAsync(_convertStatusId);

            actualStep = 2;
            logger.LogInformation("Creating MP4-Video");
            var destinationFilename = Path.Combine(appSettings.Value.DirectoryConvertVideo,
                Path.GetFileNameWithoutExtension(_filenameSource) + ".mp4");

            logger.LogDebug("Create media info");
            var mediaInfo = await FFmpeg.GetMediaInfo($@"{_filenameSource}");

            logger.LogDebug("Create video stream");
            var videoStream = mediaInfo.VideoStreams.First();

            logger.LogDebug("Create audio stream");
            var audioStream = mediaInfo.AudioStreams.First();

            logger.LogDebug("Set video codec");
            videoStream.SetCodec(VideoCodec.h264);

            audioStream.SetCodec(AudioCodec.aac);
            audioStream.SetSampleRate(48000);
            switch (height)
            {
                case 360:
                    audioStream.SetBitrate(96000);
                    break;
                case 480:
                    audioStream.SetBitrate(128000);
                    break;
                case 720:
                    audioStream.SetBitrate(128000);
                    break;
                case 1080:
                    audioStream.SetBitrate(192000);
                    break;
                case 2160:
                    audioStream.SetBitrate(192000);
                    break;
            }

            switch (height)
            {
                case 360:
                    videoStream.SetBitrate(800000, 856000, 1200000);
                    break;
                case 480:
                    videoStream.SetBitrate(1400000, 1498000, 2100000);
                    break;
                case 720:
                    videoStream.SetBitrate(5300000, 5398000, 4200000);
                    break;
                case 1080:
                    videoStream.SetBitrate(12000000, 12990000, 7500000);
                    break;
                case 2160:
                    videoStream.SetBitrate(30000000, 39889999, 9500000);
                    break;
            }

            var conversion = FFmpeg.Conversions.New()
                .AddStream(videoStream)
                .AddStream(audioStream)
                .SetPreset(ConversionPreset.Slow);

            if (_metadata.ScanType == "Interlaced")
            {
                if (_metadata.ScanOrder == "TFF")
                {
                    logger.LogDebug("Interlaced: Yes  / Mode: Top Field First");
                    commandLineParameter += $" -vf \"yadif=0\"";
                }
                else
                {
                    logger.LogDebug("Interlaced: Yes  / Mode: Bottom Field First");
                    commandLineParameter += $" -vf \"yadif=1\"";
                }
            }

            if (appSettings.Value.UsedThreads > Environment.ProcessorCount)
            {
                logger.LogDebug($"Using {Environment.ProcessorCount} threads for encoding");
                conversion.UseMultiThread(Environment.ProcessorCount);
            }
            else
            {
                logger.LogDebug($"Using {appSettings.Value.UsedThreads} threads for encoding");
                conversion.UseMultiThread(appSettings.Value.UsedThreads);
            }

            commandLineParameter += " -movflags faststart ";

            logger.LogDebug($"Destination filename: {destinationFilename}");
            conversion.SetOutput(destinationFilename);

            logger.LogDebug($"Used Commandline-Parameter: {commandLineParameter}");
            conversion.AddParameter(commandLineParameter);

            _currentConvertingId = _convertStatusId;
            _startDateConversion = DateTime.Now;
            conversion.OnProgress += WriteProgress;

            actualStep = 3;
            await conversion.Start();

            conversion.OnProgress -= WriteProgress;

            actualStep = 4;
            logger.LogInformation("Set Status in database to 'MP4 video successfully converted'");
            await databaseOperations.SetStatusConvertMp4EndAsync(_convertStatusId);

            return destinationFilename;
        }
        catch (Exception ex)
        {
            switch (actualStep)
            {
                case 1:
                    throw new ServiceException("Error!!! Could not set status to 'converting mp4' in database.", ex);
                case 2:
                    throw new ServiceException("Error!!! Could not set ffmpeg parameter for mp4 video.", ex);
                case 3:
                    throw new ServiceException("Error!!! Could not start ffmpeg process for converting mp4 video.", ex);
                case 4:
                    throw new ServiceException("Error!!! Could not set status to 'mp4 converted' in database.", ex);
                default:
                    throw new ServiceException("Error!!! Could not convert mp4 video.", ex);
            }
        }
    }

    private void WriteProgress(object sender, ConversionProgressEventArgs args)
    {
        if (args.Percent > 0)
        {
            var percentValue = args.Percent;

            var spanDuration = DateTime.Now.Subtract(_startDateConversion);
            var secondsDuration = spanDuration.TotalSeconds;

            var secondsTotal = 100 * secondsDuration / percentValue;

            var secondsRest = secondsTotal - secondsDuration;

            databaseOperations
                .UpdateProgressAsync(_currentConvertingId, percentValue, spanDuration,
                    TimeSpan.FromSeconds(secondsRest)).FireAndForget();

            var @event = new VideoConvertProgressEvent()
            {
                ConvertStatusId = _consumerContext.Message.ConvertStatusId,
                UploadVideoId = _consumerContext.Message.Id
            };
            _consumerContext.Publish<IVideoConvertProgressEvent>(@event).FireAndForget();
        }
    }

    #endregion

    #region Interface IVideoConverter

    public async Task ConvertVideo(ConsumeContext<IConvertVideoCmd> consumerContext, string filenameSourceVideo,
        MediaInfoData metadata)
    {
        _filenameSource = filenameSourceVideo;
        _metadata = metadata;
        _convertStatusId = consumerContext.Message.ConvertStatusId;
        _consumerContext = consumerContext;

        // Check if the threshold for conversion into the streaming format is exceeded
        // If the threshold is exceeded, then the video is converted in HLS,
        // if not, then the video is converted as a normal MP4
        // The threshold refers to the number of seconds of the video's runtime
        logger.LogInformation(
            $"Total Duration in seconds = {metadata.DurationSecondsTotal} / Threshold for Streaming = {appSettings.Value.ThresholdStreaming}");
        if (_metadata.DurationSecondsTotal > appSettings.Value.ThresholdStreaming)
        {
            logger.LogInformation(
                "Total Duration in seconds is over threshold for streaming. Using Streaming-Format for conversion.");
            logger.LogInformation("Using HLS as streaming format.");

            var filename360 = await Convert_MP4_Video_Streaming(360);

            string filename480;
            if (_metadata.Height > 360)
            {
                filename480 = await Convert_MP4_Video_Streaming(480);
            }
            else
            {
                filename480 = "";
            }

            string filename720;
            if (_metadata.Height > 480)
            {
                filename720 = await Convert_MP4_Video_Streaming(720);
            }
            else
            {
                filename720 = "";
            }

            string filename1080;
            if (_metadata.Height > 720)
            {
                filename1080 = await Convert_MP4_Video_Streaming(1080);
            }
            else
            {
                filename1080 = "";
            }

            string filename2160;
            if (_metadata.Height > 1080)
            {
                filename2160 = await Convert_MP4_Video_Streaming(2160);
            }
            else
            {
                filename2160 = "";
            }

            var filenameThumbnail = await CreatePreviewImage();

            var filenameStream = await CreateHlsMetadata(filename360, filename480, filename720,
                filename1080, filename2160);

            var filenameVtt = await CreateVttFile();

            await CopyConvertedToDestinationHls(_convertStatusId, filename360, filename480, filename720, filename1080,
                filename2160, filenameStream, filenameThumbnail, filenameVtt);

            await DeleteOriginalFile();

            await DeleteGeneratedFilesHls(filename360);
        }
        else
        {
            logger.LogInformation(
                "Total Duration in seconds is under threshold for streaming. Using normal MP4-Format for conversion.");

            var filenameMp4 = await Convert_MP4_Video(_metadata.Height);

            var filenameThumbnail = await CreatePreviewImage();

            var filenameVtt = await CreateVttFile();

            await DeleteOriginalFile();

            await CopyConvertedToDestination(_convertStatusId, filenameMp4, filenameThumbnail, filenameVtt);
        }
    }

    #endregion
}