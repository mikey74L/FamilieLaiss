using FamilieLaissMassTransitDefinitions.Commands.UploadPicture;
using FamilieLaissMassTransitDefinitions.Contracts.Commands.UploadPicture;
using MassTransit;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using Microsoft.Extensions.Logging;
using PictureConvertExecuteService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PictureConvertExecuteService.Services;

public class MetaExtractorService(
    ILogger<MetaExtractorService> logger,
    IDatabaseOperations databaseOperations,
    IBus massTransit)
    : IMetaExtractor
{
    #region Private Classes

    private sealed class URational
    {
        public URational(byte[] bytes)
        {
            var n = new byte[4];
            var d = new byte[4];
            Array.Copy(bytes, 0, n, 0, 4);
            Array.Copy(bytes, 4, d, 0, 4);
            Num = BitConverter.ToUInt32(n, 0);
            Denom = BitConverter.ToUInt32(d, 0);
        }

        public uint Num { private set; get; }
        public uint Denom { private set; get; }
    }

    #endregion

    #region Private Methods

    private async Task GetExifInfo(long id, string filename)
    {
        string make = string.Empty;
        string model = string.Empty;
        double? resolutionX = null;
        double? resolutionY = null;
        short? resolutionUnit = null;
        short? orientation = null;
        DateTimeOffset? ddlRecorded = null;
        double? exposureTime = null;
        short? exposureProgram = null;
        short? exposureMode = null;
        double? fNumber = null;
        int? isoSensitivity = null;
        double? shutterSpeed = null;
        short? meteringMode = null;
        short? flashMode = null;
        double? focalLength = null;
        short? sensingMode = null;
        short? whiteBalanceMode = null;
        short? sharpness = null;
        double? gpsLongitude;
        double? gpsLatitude;
        short? contrast = null;
        short? saturation = null;

        IEnumerable<Directory> directories = ImageMetadataReader.ReadMetadata(filename);

        var ifd0Directory = directories.OfType<ExifIfd0Directory>().FirstOrDefault();

        if (ifd0Directory is not null)
        {
            try
            {
                make = ifd0Directory.ContainsTag(ExifDirectoryBase.TagMake)
                    ? ifd0Directory.GetString(ExifDirectoryBase.TagMake) ?? ""
                    : "";
            }
            catch
            {
                make = "";
            }

            try
            {
                model = ifd0Directory.ContainsTag(ExifDirectoryBase.TagModel)
                    ? ifd0Directory.GetString(ExifDirectoryBase.TagModel) ?? ""
                    : "";
            }
            catch
            {
                model = "";
            }

            try
            {
                resolutionX = ifd0Directory.ContainsTag(ExifDirectoryBase.TagXResolution)
                    ? ifd0Directory.GetDouble(ExifDirectoryBase.TagXResolution)
                    : null;
            }
            catch
            {
                resolutionX = null;
            }

            try
            {
                resolutionY = ifd0Directory.ContainsTag(ExifDirectoryBase.TagYResolution)
                    ? ifd0Directory.GetDouble(ExifDirectoryBase.TagYResolution)
                    : null;
            }
            catch
            {
                resolutionY = null;
            }

            try
            {
                resolutionUnit = ifd0Directory.ContainsTag(ExifDirectoryBase.TagResolutionUnit)
                    ? ifd0Directory.GetInt16(ExifDirectoryBase.TagResolutionUnit)
                    : null;
            }
            catch
            {
                resolutionUnit = null;
            }

            try
            {
                orientation = ifd0Directory.ContainsTag(ExifDirectoryBase.TagOrientation)
                    ? ifd0Directory.GetInt16(ExifDirectoryBase.TagOrientation)
                    : null;
            }
            catch
            {
                orientation = null;
            }

            try
            {
                ddlRecorded = ifd0Directory.ContainsTag(ExifDirectoryBase.TagDateTime)
                    ? ifd0Directory.GetDateTime(ExifDirectoryBase.TagDateTime)
                    : null;
            }
            catch
            {
                ddlRecorded = null;
            }
        }

        var subIfdDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();

        if (subIfdDirectory is not null)
        {
            try
            {
                fNumber = subIfdDirectory.ContainsTag(ExifDirectoryBase.TagFNumber)
                    ? subIfdDirectory.GetDouble(ExifDirectoryBase.TagFNumber)
                    : null;

                if (fNumber is not null)
                {
                    fNumber = Math.Round(fNumber.Value, 1, MidpointRounding.AwayFromZero);
                }
            }
            catch
            {
                fNumber = null;
            }

            try
            {
                exposureProgram = subIfdDirectory.ContainsTag(ExifDirectoryBase.TagExposureProgram)
                    ? subIfdDirectory.GetInt16(ExifDirectoryBase.TagExposureProgram)
                    : null;
            }
            catch
            {
                exposureProgram = null;
            }

            try
            {
                isoSensitivity = subIfdDirectory.ContainsTag(ExifDirectoryBase.TagIsoEquivalent)
                    ? subIfdDirectory.GetInt32(ExifDirectoryBase.TagIsoEquivalent)
                    : null;
            }
            catch
            {
                isoSensitivity = null;
            }

            try
            {
                shutterSpeed = subIfdDirectory.ContainsTag(ExifDirectoryBase.TagShutterSpeed)
                    ? subIfdDirectory.GetDouble(ExifDirectoryBase.TagShutterSpeed)
                    : null;
            }
            catch
            {
                shutterSpeed = null;
            }

            try
            {
                meteringMode = subIfdDirectory.ContainsTag(ExifDirectoryBase.TagMeteringMode)
                    ? subIfdDirectory.GetInt16(ExifDirectoryBase.TagMeteringMode)
                    : null;
            }
            catch
            {
                meteringMode = null;
            }

            try
            {
                flashMode = subIfdDirectory.ContainsTag(ExifDirectoryBase.TagFlash)
                    ? subIfdDirectory.GetInt16(ExifDirectoryBase.TagFlash)
                    : null;
            }
            catch
            {
                flashMode = null;
            }

            try
            {
                focalLength = subIfdDirectory.ContainsTag(ExifDirectoryBase.TagFocalLength)
                    ? Math.Round(subIfdDirectory.GetDouble(ExifDirectoryBase.TagFocalLength), 0,
                        MidpointRounding.AwayFromZero)
                    : null;
            }
            catch
            {
                focalLength = null;
            }

            try
            {
                sensingMode = subIfdDirectory.ContainsTag(ExifDirectoryBase.TagSensingMethod)
                    ? subIfdDirectory.GetInt16(ExifDirectoryBase.TagSensingMethod)
                    : null;
            }
            catch
            {
                sensingMode = null;
            }

            try
            {
                exposureMode = subIfdDirectory.ContainsTag(ExifDirectoryBase.TagExposureMode)
                    ? subIfdDirectory.GetInt16(ExifDirectoryBase.TagExposureMode)
                    : null;
            }
            catch
            {
                exposureMode = null;
            }

            try
            {
                whiteBalanceMode = subIfdDirectory.ContainsTag(ExifDirectoryBase.TagWhiteBalanceMode)
                    ? subIfdDirectory.GetInt16(ExifDirectoryBase.TagWhiteBalanceMode)
                    : null;
            }
            catch
            {
                whiteBalanceMode = null;
            }

            try
            {
                sharpness = subIfdDirectory.ContainsTag(ExifDirectoryBase.TagSharpness)
                    ? subIfdDirectory.GetInt16(ExifDirectoryBase.TagSharpness)
                    : null;
            }
            catch
            {
                sharpness = null;
            }

            try
            {
                contrast = subIfdDirectory.ContainsTag(ExifDirectoryBase.TagContrast)
                    ? subIfdDirectory.GetInt16(ExifDirectoryBase.TagContrast)
                    : null;
            }
            catch
            {
                contrast = null;
            }

            try
            {
                exposureTime = subIfdDirectory.ContainsTag(ExifDirectoryBase.TagExposureTime)
                    ? Math.Round(subIfdDirectory.GetDouble(ExifDirectoryBase.TagExposureTime), 2,
                        MidpointRounding.AwayFromZero)
                    : null;
            }
            catch
            {
                exposureTime = null;
            }

            try
            {
                saturation = subIfdDirectory.ContainsTag(ExifDirectoryBase.TagSaturation)
                    ? subIfdDirectory.GetInt16(ExifDirectoryBase.TagSaturation)
                    : null;
            }
            catch
            {
                saturation = null;
            }
        }

        var gpsDirectory = directories.OfType<GpsDirectory>().FirstOrDefault();

        if (gpsDirectory != null)
        {
            try
            {
                var gpsLocation = gpsDirectory.GetGeoLocation();
                if (gpsLocation != null)
                {
                    gpsLongitude = gpsLocation.Longitude;
                    gpsLatitude = gpsLocation.Latitude;
                }
                else
                {
                    gpsLongitude = null;
                    gpsLatitude = null;
                }
            }
            catch
            {
                gpsLongitude = null;
                gpsLatitude = null;
            }
        }
        else
        {
            gpsLongitude = null;
            gpsLatitude = null;
        }

        //Correct the GPS data if the geo position is 0 or -0
        //This occurs in some images because not all Apps write clean GPS positions into the Exif information
        //This needs to be corrected to NULL, as these are not valid GPS positions, and will lead to
        //errors when determining the geo position later
        if (gpsLongitude.HasValue || gpsLatitude.HasValue)
        {
            if (gpsLongitude.HasValue && (gpsLongitude.Value == 0 || gpsLongitude.Value == -0))
            {
                gpsLongitude = null;
                gpsLatitude = null;
            }

            if (gpsLatitude.HasValue && (gpsLatitude.Value == 0 || gpsLatitude.Value == -0))
            {
                gpsLongitude = null;
                gpsLatitude = null;
            }
        }

        logger.LogInformation("Write exif Info to database");
        var newCommand = new MassSetUploadPictureExifInfoCmd()
        {
            Contrast = contrast,
            ExposureMode = exposureMode,
            ExposureProgram = exposureProgram,
            FlashMode = flashMode,
            FNumber = fNumber,
            FocalLength = focalLength,
            GpsLatitude = gpsLatitude,
            GpsLongitude = gpsLongitude,
            Id = id,
            IsoSensitivity = isoSensitivity,
            Make = make,
            MeteringMode = meteringMode,
            Model = model,
            Orientation = orientation,
            ResolutionUnit = resolutionUnit,
            ResolutionX = resolutionX,
            ResolutionY = resolutionY,
            Saturation = saturation,
            SensingMode = sensingMode,
            Sharpness = sharpness,
            ShutterSpeed = shutterSpeed,
            WhiteBalanceMode = whiteBalanceMode,
            DdlRecorded = ddlRecorded,
            ExposureTime = exposureTime
        };
        await massTransit.Send<IMassSetUploadPictureExifInfoCmd>(newCommand);
    }

    #endregion

    #region Interface iMetaExtractor

    public async Task ExtractMetadataAsync(ConsumeContext<IMassConvertPictureCmd> consumerContext, string filename)
    {
        logger.LogInformation("Set status for read Exif-Info begin");
        await databaseOperations.SetStatusReadExifBeginAsync(consumerContext.Message.ConvertStatusId);

        logger.LogInformation("Reading Exif-Info from picture file");
        await GetExifInfo(consumerContext.Message.Id, filename);

        logger.LogInformation("Set status for read Exif-Info end");
        await databaseOperations.SetStatusReadExifEndAsync(consumerContext.Message.ConvertStatusId);
    }

    #endregion
}