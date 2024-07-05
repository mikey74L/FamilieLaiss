using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FamilieLaissMassTransitDefinitions.Contracts.Events;
using FamilieLaissMassTransitDefinitions.Events;
using Microsoft.Extensions.Logging;
using ServiceHelper.Exceptions;
using Xabe.FFmpeg;

namespace VideoConvertExecuteService.Services;

public partial class VideoConverterService
{
    private async Task<string> CreatePreviewImage()
    {
        var actualStep = 0;

        try
        {
            actualStep = 1;
            logger.LogInformation("Set Status in database to 'creating thumbnail'");
            await databaseOperations.SetStatusCreateThumbnailBeginAsync(_convertStatusId);

            actualStep = 2;
            var @event = new VideoConvertProgressEvent()
            {
                ConvertStatusId = _consumerContext.Message.ConvertStatusId,
                UploadVideoId = _consumerContext.Message.Id
            };
            await _consumerContext.Publish<IVideoConvertProgressEvent>(@event);

            actualStep = 3;
            logger.LogInformation("Setting command line parameter for thumbnail");
            var destinationFilename = Path.Combine(appSettings.Value.DirectoryConvertVideo,
                Path.GetFileNameWithoutExtension(_filenameSource) + ".jpg");
            var returnValue = destinationFilename;
            var commandLineParameter =
                "-vf \"thumbnail,scale=w=640:h=360:force_original_aspect_ratio=decrease\" -frames:v 1";
            logger.LogDebug($"Using command line parameter: {commandLineParameter}");

            logger.LogDebug("Create media info");
            var mediaInfo = await FFmpeg.GetMediaInfo(_filenameSource);

            logger.LogDebug("Create video stream");
            var videoStream = mediaInfo.VideoStreams.First();

            logger.LogDebug("Create conversion");
            var conversionPreview = FFmpeg.Conversions.New()
                .AddStream(videoStream)
                .SetOutput(destinationFilename)
                .AddParameter(commandLineParameter);

            actualStep = 4;
            logger.LogInformation("Start generating thumbnail with ffmpeg");
            await conversionPreview.Start();

            actualStep = 5;
            logger.LogInformation("Setting command line parameter for temporary sprite images");
            destinationFilename = Path.Combine(appSettings.Value.DirectoryConvertVideo,
                Path.GetFileNameWithoutExtension(_filenameSource) + "-Sprite-%05d.jpg");
            commandLineParameter = $" -f image2 -vf " +
                                   $"\"fps=fps=1/{_metadata.IntervalThumbnails}," +
                                   $"scale=w={appSettings.Value.WidthThumbnailImage}:h={appSettings.Value.HeightThumbnailImage}:force_original_aspect_ratio=decrease," +
                                   $"pad={appSettings.Value.WidthThumbnailImage}:{appSettings.Value.HeightThumbnailImage}:(ow-iw)/2:(oh-ih)/2\"";
            logger.LogDebug($"Using command line parameter: {commandLineParameter}");

            logger.LogDebug("Create conversion");
            var conversionThumbnails = FFmpeg.Conversions.New()
                .AddStream(videoStream)
                .SetOutput(destinationFilename)
                .AddParameter(commandLineParameter);

            actualStep = 6;
            logger.LogInformation("Start generating temporary sprite images with ffmpeg");
            await conversionThumbnails.Start();

            actualStep = 7;
            logger.LogInformation("Setting command line parameter for montage");
            var filenameSprites = Path.Combine(appSettings.Value.DirectoryConvertVideo,
                Path.GetFileNameWithoutExtension(_filenameSource) + "-Sprite-");
            destinationFilename = Path.Combine(appSettings.Value.DirectoryConvertVideo,
                Path.GetFileNameWithoutExtension(_filenameSource) + "_Sprite.jpg");
            var countThumbnails = _metadata.DurationSecondsTotal / _metadata.IntervalThumbnails;
            if (_metadata.DurationSecondsTotal % _metadata.IntervalThumbnails != 0) countThumbnails += 1;
            var tileCount = Convert.ToInt32(Math.Round(Math.Sqrt(countThumbnails), 0));
            commandLineParameter = "\"{0}*.jpg\" -tile {1}x -geometry {2}x{3}! {4}";
            commandLineParameter = string.Format(commandLineParameter, filenameSprites, tileCount.ToString(),
                appSettings.Value.WidthThumbnailImage.ToString(), appSettings.Value.HeightThumbnailImage.ToString(),
                destinationFilename);
            logger.LogDebug($"Using command line parameter: {commandLineParameter}");

            actualStep = 8;
            logger.LogInformation("Start generating sprite with montage");
            ProcessStartInfo procInfoMontage = new()
            {
                FileName = "montage",
                Arguments = commandLineParameter,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardError = false,
                RedirectStandardOutput = false,
                CreateNoWindow = true
            };
            Process processMontage = new();
            processMontage.StartInfo = procInfoMontage;
            processMontage.Start();
            while (!processMontage.HasExited)
            {
                //Waiting one second for next check if process is still running
                await Task.Delay(1000);
            }

            processMontage.Dispose();

            actualStep = 9;
            logger.LogInformation("Thumbnail-Image successfully generated");
            await databaseOperations.SetStatusCreateThumbnailEndAsync(_convertStatusId);

            actualStep = 10;
            await _consumerContext.Publish<IVideoConvertProgressEvent>(@event);

            return returnValue;
        }
        catch (Exception ex)
        {
            switch (actualStep)
            {
                case 1:
                    throw new ServiceException("Error!!! Could not set status to 'creating thumbnail' in database.",
                        ex);
                case 2:
                    throw new ServiceException("Error!!! Could not send event with mass transit", ex);
                case 3:
                    throw new ServiceException("Error!!! Could not set ffmpeg parameter for thumbnail.", ex);
                case 4:
                    throw new ServiceException("Error!!! Could not start ffmpeg process for generating thumbnail.", ex);
                case 5:
                    throw new ServiceException(
                        "Error!!! Could not set ffmpeg parameter for temporary images for sprite.", ex);
                case 6:
                    throw new ServiceException(
                        "Error!!! Could not start ffmpeg process for generating temporary sprite images.", ex);
                case 7:
                    throw new ServiceException("Error!!! Could not set montage parameter for sprite.", ex);
                case 8:
                    throw new ServiceException("Error!!! Could not start montage process for generating sprite.", ex);
                case 9:
                    throw new ServiceException("Error!!! Could not set status to 'thumbnail created' in database.", ex);
                case 10:
                    throw new ServiceException("Error!!! Could not send event with mass transit", ex);
                default:
                    throw new ServiceException("Error!!! Could not create thumbnail.", ex);
            }
        }
    }

    private async Task<string> CreateVttFile()
    {
        var actualStep = 0;
        var currentSecond = 1;
        var minutes = 0;
        var hours = 0;
        var hoursOld = 0;
        var minutesOld = 0;
        var secondsOld = 0;
        var countColumn = 1;
        var countRow = 1;

        try
        {
            actualStep = 1;
            logger.LogInformation("Set Status in database to 'creating VTT'");
            await databaseOperations.SetStatusCreateVttBeginAsync(_convertStatusId);

            actualStep = 2;
            var @event = new VideoConvertProgressEvent()
            {
                ConvertStatusId = _consumerContext.Message.ConvertStatusId,
                UploadVideoId = _consumerContext.Message.Id
            };
            await _consumerContext.Publish<IVideoConvertProgressEvent>(@event);

            actualStep = 3;
            var destinationFilename = Path.Combine(appSettings.Value.DirectoryConvertVideo,
                Path.GetFileNameWithoutExtension(_filenameSource) + ".vtt");
            var urlImage = "{0}/" + Path.GetFileNameWithoutExtension(destinationFilename) + "_Sprite.jpg";
            logger.LogDebug($"Destination filename: {destinationFilename}");
            logger.LogDebug($"URL-Image: {urlImage}");

            actualStep = 4;
            StreamWriter sw = new(destinationFilename, false, Encoding.UTF8);

            actualStep = 5;
            await sw.WriteLineAsync("WEBVTT");
            await sw.WriteLineAsync("");

            var countThumbnails = _metadata.DurationSecondsTotal / _metadata.IntervalThumbnails;
            if (_metadata.DurationSecondsTotal % _metadata.IntervalThumbnails != 0) countThumbnails += 1;
            var tileCountX = Convert.ToInt32(Math.Round(Math.Sqrt(countThumbnails), 0));

            while (currentSecond <= _metadata.DurationSecondsTotal)
            {
                if ((currentSecond % _metadata.IntervalThumbnails == 0) ||
                    (currentSecond == _metadata.DurationSecondsTotal))
                {
                    var seconds = currentSecond;
                    if (seconds > 59)
                    {
                        minutes = seconds / 60;
                        seconds -= (minutes * 60);
                    }

                    if (minutes > 59)
                    {
                        hours = minutes / 60;
                        minutes -= (hours * 60);
                    }

                    await sw.WriteLineAsync(hoursOld.ToString("D2") + ":" + minutesOld.ToString("D2") + ":" +
                                            secondsOld.ToString("D2") + ".000 --> " + hours.ToString("D2") + ":" +
                                            minutes.ToString("D2") + ":" + seconds.ToString("D2") + ".000");
                    await sw.WriteLineAsync(urlImage + "#xywh=" +
                                            ((countColumn - 1) * appSettings.Value.WidthThumbnailImage).ToString() +
                                            "," + ((countRow - 1) * appSettings.Value.HeightThumbnailImage).ToString() +
                                            "," + appSettings.Value.WidthThumbnailImage.ToString() + "," +
                                            appSettings.Value.HeightThumbnailImage.ToString());
                    await sw.WriteLineAsync("");

                    countColumn += 1;
                    if (countColumn > tileCountX)
                    {
                        countColumn = 1;
                        countRow += 1;
                    }

                    hoursOld = hours;
                    minutesOld = minutes;
                    secondsOld = seconds;
                }

                currentSecond += 1;
            }

            actualStep = 6;
            await sw.FlushAsync();
            sw.Close();
            await sw.DisposeAsync();

            actualStep = 7;
            logger.LogInformation("VTT successfully generated");
            await databaseOperations.SetStatusCreateVttEndAsync(_convertStatusId);

            actualStep = 8;
            await _consumerContext.Publish<IVideoConvertProgressEvent>(@event);

            return destinationFilename;
        }
        catch (Exception ex)
        {
            switch (actualStep)
            {
                case 1:
                    throw new ServiceException("Error!!! Could not set status to 'creating VTT' in database.", ex);
                case 2:
                    throw new ServiceException("Error!!! Could not send event with mass transit", ex);
                case 3:
                    throw new ServiceException("Error!!! Could not set filenames for VTT.", ex);
                case 4:
                    throw new ServiceException("Error!!! Could not create file stream for VTT.", ex);
                case 5:
                    throw new ServiceException("Error!!! Could not write data to VTT stream.", ex);
                case 6:
                    throw new ServiceException("Error!!! Could not close file stream for VTT.", ex);
                case 7:
                    throw new ServiceException("Error!!! Could not set status to 'VTT created' in database.", ex);
                case 8:
                    throw new ServiceException("Error!!! Could not send event with mass transit", ex);
                default:
                    throw new ServiceException("Error!!! Could not create VTT.", ex);
            }
        }
    }

    private async Task<string> CreateHlsMetadata(string filename360, string filename480, string filename720,
        string filename1080, string filename2160)
    {
        var actualStep = 0;

        try
        {
            actualStep = 1;
            logger.LogInformation("Set Status in database to 'creating HLS'");
            await databaseOperations.SetStatusCreateHlsBeginAsync(_convertStatusId);

            actualStep = 2;
            var @event = new VideoConvertProgressEvent()
            {
                ConvertStatusId = _consumerContext.Message.ConvertStatusId,
                UploadVideoId = _consumerContext.Message.Id
            };
            await _consumerContext.Publish<IVideoConvertProgressEvent>(@event);

            actualStep = 3;
            logger.LogInformation("Setting filename for HLS metadata file");
            var destinationFilename = Path.Combine(appSettings.Value.DirectoryConvertVideo,
                Path.GetFileNameWithoutExtension(_filenameSource) + ".m3u8");

            actualStep = 4;
            StreamWriter metadataFile = new(destinationFilename, false, Encoding.UTF8);

            //Writing metadata to file
            //#EXT-X-STREAM-INF:AVERAGE-BANDWIDTH=2778321,BANDWIDTH=3971374,VIDEO-RANGE=SDR,CODECS="hvc1.2.4.L123.B0",RESOLUTION=1280x720,FRAME-RATE=23.976,CLOSED-CAPTIONS=NONE,HDCP-LEVEL=NONE
            //sdr_720/prog_index.m3u8
            //#EXT-X-STREAM-INF:AVERAGE-BANDWIDTH=6759875,BANDWIDTH=10022043,VIDEO-RANGE=SDR,CODECS="hvc1.2.4.L123.B0",RESOLUTION=1920x1080,FRAME-RATE=23.976,CLOSED-CAPTIONS=NONE,HDCP-LEVEL=TYPE-0
            //sdr_1080/prog_index.m3u8
            //#EXT-X-STREAM-INF:AVERAGE-BANDWIDTH=20985770,BANDWIDTH=28058971,VIDEO-RANGE=SDR,CODECS="hvc1.2.4.L150.B0",RESOLUTION=3840x2160,FRAME-RATE=23.976,CLOSED-CAPTIONS=NONE,HDCP-LEVEL=TYPE-1
            //sdr_2160/prog_index.m3u8
            //#EXT-X-STREAM-INF:AVERAGE-BANDWIDTH=3385450,BANDWIDTH=5327059,VIDEO-RANGE=PQ,CODECS="dvh1.05.01",RESOLUTION=1280x720,FRAME-RATE=23.976,CLOSED-CAPTIONS=NONE,HDCP-LEVEL=NONE
            //dolby_720/prog_index.m3u8
            //#EXT-X-STREAM-INF:AVERAGE-BANDWIDTH=7999361,BANDWIDTH=12876596,VIDEO-RANGE=PQ,CODECS="dvh1.05.03",RESOLUTION=1920x1080,FRAME-RATE=23.976,CLOSED-CAPTIONS=NONE,HDCP-LEVEL=TYPE-0
            //dolby_1080/prog_index.m3u8
            //#EXT-X-STREAM-INF:AVERAGE-BANDWIDTH=24975091,BANDWIDTH=30041698,VIDEO-RANGE=PQ,CODECS="dvh1.05.06",RESOLUTION=3840x2160,FRAME-RATE=23.976,CLOSED-CAPTIONS=NONE,HDCP-LEVEL=TYPE-1
            //dolby_2160/prog_index.m3u8
            //#EXT-X-STREAM-INF:AVERAGE-BANDWIDTH=3320040,BANDWIDTH=5280654,VIDEO-RANGE=PQ,CODECS="hvc1.2.4.L123.B0",RESOLUTION=1280x720,FRAME-RATE=23.976,CLOSED-CAPTIONS=NONE,HDCP-LEVEL=NONE
            //hdr10_720/prog_index.m3u8
            //#EXT-X-STREAM-INF:AVERAGE-BANDWIDTH=7964551,BANDWIDTH=12886714,VIDEO-RANGE=PQ,CODECS="hvc1.2.4.L123.B0",RESOLUTION=1920x1080,FRAME-RATE=23.976,CLOSED-CAPTIONS=NONE,HDCP-LEVEL=TYPE-0
            //hdr10_1080/prog_index.m3u8
            //#EXT-X-STREAM-INF:AVERAGE-BANDWIDTH=24833402,BANDWIDTH=29983769,VIDEO-RANGE=PQ,CODECS="hvc1.2.4.L150.B0",RESOLUTION=3840x2160,FRAME-RATE=23.976,CLOSED-CAPTIONS=NONE,HDCP-LEVEL=TYPE-1
            //hdr10_2160/prog_index.m3u8
            actualStep = 5;
            await metadataFile.WriteLineAsync("#EXTM3U");
            await metadataFile.WriteLineAsync("#EXT-X-VERSION:3");
            await metadataFile.WriteLineAsync("#EXT-X-STREAM-INF:BANDWIDTH=800000,RESOLUTION=640x360");
            await metadataFile.WriteLineAsync(Path.GetFileName(filename360));
            if (!string.IsNullOrEmpty(filename480))
            {
                await metadataFile.WriteLineAsync("#EXT-X-STREAM-INF:BANDWIDTH=1400000,RESOLUTION=852x480");
                await metadataFile.WriteLineAsync(Path.GetFileName(filename480));
            }

            if (!string.IsNullOrEmpty(filename720))
            {
                await metadataFile.WriteLineAsync("#EXT-X-STREAM-INF:BANDWIDTH=5300000,RESOLUTION=1280x720");
                await metadataFile.WriteLineAsync(Path.GetFileName(filename720));
            }

            if (!string.IsNullOrEmpty(filename1080))
            {
                await metadataFile.WriteLineAsync("#EXT-X-STREAM-INF:BANDWIDTH=12000000,RESOLUTION=1920x1080");
                await metadataFile.WriteLineAsync(Path.GetFileName(filename1080));
            }

            if (!string.IsNullOrEmpty(filename2160))
            {
                await metadataFile.WriteLineAsync("#EXT-X-STREAM-INF:BANDWIDTH=30000000,RESOLUTION=3840x2160");
                await metadataFile.WriteLineAsync(Path.GetFileName(filename2160));
            }

            await metadataFile.FlushAsync();
            metadataFile.Close();
            await metadataFile.DisposeAsync();

            actualStep = 6;
            logger.LogInformation("HLS metadata successfully generated");
            await databaseOperations.SetStatusCreateHlsEndAsync(_convertStatusId);

            actualStep = 7;
            await _consumerContext.Publish<IVideoConvertProgressEvent>(@event);

            return destinationFilename;
        }
        catch (Exception ex)
        {
            switch (actualStep)
            {
                case 1:
                    throw new ServiceException("Error!!! Could not set status to 'creating HLS' in database.", ex);
                case 2:
                    throw new ServiceException("Error!!! Could not send event with mass transit", ex);
                case 3:
                    throw new ServiceException("Error!!! Could not set filename for HLS metadata file.", ex);
                case 4:
                    throw new ServiceException("Error!!! Could not open file stream for HLS metadata file.", ex);
                case 5:
                    throw new ServiceException("Error!!! Could not write to HLS metadata file.", ex);
                case 6:
                    throw new ServiceException("Error!!! Could not set status to 'HLS created' in database.", ex);
                case 7:
                    throw new ServiceException("Error!!! Could not send event with mass transit", ex);
                default:
                    throw new ServiceException("Error!!! Could not create HLS metadata file.", ex);
            }
        }
    }
}