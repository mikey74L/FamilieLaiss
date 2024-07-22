using FamilieLaissMassTransitDefinitions.Commands;
using FamilieLaissMassTransitDefinitions.Contracts.Commands;
using FamilieLaissMassTransitDefinitions.Contracts.Events;
using FamilieLaissMassTransitDefinitions.Events;
using FamilieLaissSharedObjects.Enums;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceHelper.Exceptions;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using VideoConvertExecuteService.Interfaces;
using VideoConvertExecuteService.Models;

namespace VideoConvertExecuteService.Services;

public class MetadataExtractorService(
    ILogger<MetadataExtractorService> logger,
    IOptions<AppSettings> appSettings,
    IBus massTransit,
    IDatabaseOperations databaseOperations)
    : IMetadataExtractor
{
    #region Private Methods

    private string MediaInfoGetValue(string fileName, string groupName, string valueName)
    {
        ProcessStartInfo procInfo = new()
        {
            FileName = "mediainfo",
            Arguments = $"--Inform=\"{groupName};%{valueName}%\" {fileName}",
            UseShellExecute = false,
            WindowStyle = ProcessWindowStyle.Hidden,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            CreateNoWindow = true
        };

        Process processMetadata = new()
        {
            StartInfo = procInfo
        };
        processMetadata.Start();

        var streamProcessOutput = processMetadata.StandardOutput;
        var output = streamProcessOutput.ReadLine();

        processMetadata.Dispose();

        return output;
    }

    private string MediaInfoGetHeight(string filename)
    {
        return MediaInfoGetValue(filename, "Video", "Height");
    }

    private string MediaInfoGetWidth(string filename)
    {
        return MediaInfoGetValue(filename, "Video", "Width");
    }

    private string MediaInfoGetDuration(string filename)
    {
        return MediaInfoGetValue(filename, "Video", "Duration");
    }

    private string MediaInfoGetScanType(string filename)
    {
        return MediaInfoGetValue(filename, "Video", "ScanType");
    }

    private string MediaInfoGetScanOrder(string filename)
    {
        return MediaInfoGetValue(filename, "Video", "ScanOrder");
    }

    private string MediaInfoGetXyz(string filename)
    {
        return MediaInfoGetValue(filename, "General", "xyz");
    }

    private (double? longitude, double? latitude) GetGpsCoordinates(string filename)
    {
        var result = MediaInfoGetXyz(filename);

        if (string.IsNullOrEmpty(result))
        {
            return (longitude: null, latitude: null);
        }

        var positionSecond = result.IndexOf('+', 1);
        if (positionSecond == -1)
        {
            positionSecond = result.IndexOf('-', 1);
        }

        var latitude = result.Substring(0, positionSecond);

        var longitude = result.Substring(positionSecond, result.Length - positionSecond - 1);

        return (longitude: Convert.ToDouble(longitude), latitude: Convert.ToDouble(latitude));
    }

    #endregion

    #region Interface IMetadataExtractor

    public async Task<MediaInfoData> ExtractMetadata(ConsumeContext<IMassConvertVideoCmd> context,
        string filenameSourceVideo)
    {
        var currentStep = 0;

        logger.LogInformation("Get information from original video file with MediaInfo.dll");

        try
        {
            currentStep = 1;
            logger.LogInformation("Set status to read media info - begin");
            await databaseOperations.SetStatusReadMediaInfoBeginAsync(context.Message.ConvertStatusId);

            currentStep = 2;
            var @eventProgress = new VideoConvertProgressEvent()
            {
                ConvertStatusId = context.Message.ConvertStatusId,
                UploadVideoId = context.Message.Id
            };
            await context.Publish<IVideoConvertProgressEvent>(@eventProgress);

            currentStep = 3;
            logger.LogInformation("Read media info from file");
            var scanType = MediaInfoGetScanType(filenameSourceVideo);
            var scanOrder = scanType == "Interlaced" ? MediaInfoGetScanOrder(filenameSourceVideo) : "";
            var width = MediaInfoGetWidth(filenameSourceVideo);
            var height = MediaInfoGetHeight(filenameSourceVideo);
            var duration = MediaInfoGetDuration(filenameSourceVideo);
            logger.LogDebug($"Original Video Size is (Width / Height): {width} / {height}");

            logger.LogInformation("Calculating duration");
            var durationNumber = Convert.ToInt32(duration);
            int hours;
            if (durationNumber >= 3600000)
            {
                hours = durationNumber / 3600000;
                durationNumber -= (hours * 3600000);
            }
            else
            {
                hours = 0;
            }

            int minutes;
            if (durationNumber >= 60000)
            {
                minutes = durationNumber / 60000;
                durationNumber -= (minutes * 60000);
            }
            else
            {
                minutes = 0;
            }

            int seconds;
            if (durationNumber >= 1000)
            {
                seconds = durationNumber / 1000;
            }
            else
            {
                seconds = 0;
            }

            var durationSecondsTotal = (hours * 60 * 60) + (minutes * 60) + seconds;
            var returnValue = new MediaInfoData(Convert.ToInt32(width), Convert.ToInt32(height), scanType, scanOrder,
                durationSecondsTotal);
            logger.LogDebug($"Original Duration (HH:MM:SS): {hours}:{minutes}:{seconds}");

            logger.LogDebug("Get streaming type for video");
            EnumVideoType videoType;
            if (durationSecondsTotal > appSettings.Value.ThresholdStreaming)
            {
                videoType = EnumVideoType.Hls;
                logger.LogDebug("Streaming type: HLS");
            }
            else
            {
                videoType = EnumVideoType.Progressive;
                logger.LogDebug("Streaming type: Progressive");
            }

            logger.LogDebug("Get GPS-Coordinates from video");
            var coordinates = GetGpsCoordinates(filenameSourceVideo);
            //Correcting GPS data when Geo position = 0 or = -0
            //This occurs with some images because not all apps write clean GPS positions in the Exif information
            //This must be corrected to NULL, because this is not valid GPS positions, and will lead to errors later
            //in determining the geolocation
            if (coordinates.latitude.HasValue || coordinates.longitude.HasValue)
            {
                if (coordinates.latitude.HasValue &&
                    (coordinates.latitude.Value == 0 || coordinates.latitude.Value == -0))
                {
                    coordinates.latitude = null;
                    coordinates.longitude = null;
                }

                if (coordinates.longitude.HasValue &&
                    (coordinates.longitude.Value == 0 || coordinates.longitude.Value == -0))
                {
                    coordinates.latitude = null;
                    coordinates.longitude = null;
                }
            }

            logger.LogDebug(
                $"Longitude: {(coordinates.longitude.HasValue ? coordinates.longitude.Value.ToString(CultureInfo.InvariantCulture) : "NULL")}");
            logger.LogDebug(
                $"Latitude : {(coordinates.latitude.HasValue ? coordinates.latitude.Value.ToString(CultureInfo.InvariantCulture) : "NULL")}");

            currentStep = 4;
            logger.LogInformation("Send SetVideoInfo over service bus");

            var newCommand = new MassSetVideoInfoDataCmd()
            {
                Id = context.Message.Id,
                VideoType = videoType,
                Height = Convert.ToInt32(height),
                Width = Convert.ToInt32(width),
                Hours = hours,
                Minutes = minutes,
                Seconds = seconds,
                Longitude = coordinates.longitude,
                Latitude = coordinates.latitude
            };
            await massTransit.Send<IMassSetVideoInfoDataCmd>(newCommand);

            currentStep = 5;
            logger.LogInformation("Set status to read media info - end");
            await databaseOperations.SetStatusReadMediaInfoEndAsync(context.Message.ConvertStatusId);

            currentStep = 6;
            await context.Publish<IVideoConvertProgressEvent>(@eventProgress);

            return returnValue;
        }
        catch (Exception ex)
        {
            switch (currentStep)
            {
                case 1:
                    throw new ServiceException("Error!!! Could not set status to 'read media info start' in database.",
                        ex);
                case 2:
                    throw new ServiceException("Error!!! Could not set event over mass transit", ex);
                case 3:
                    throw new ServiceException("Error!!! Could not read media info from file.", ex);
                case 4:
                    throw new ServiceException("Error!!! Could not write duration info in database.", ex);
                case 5:
                    throw new ServiceException("Error!!! Could not set status to 'read media info end' in database.",
                        ex);
                case 6:
                    throw new ServiceException("Error!!! Could not set event over mass transit", ex);
                default:
                    throw new ServiceException("Error!!! Could not read media info from file.", ex);
            }
        }
    }

    #endregion
}