using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissMappingExtensions.GoogleGeoCodingAddress;
using FamilieLaissModels.Models.UploadPicture;

namespace FamilieLaissMappingExtensions.UploadPicture;

public static class UploadPictureMappingExtensions
{
    public static IUploadPictureModel? Map(this IFrUploadPictureOnlyId source)
    {
        var result = new UploadPictureModel()
        {
            Id = source.Id,
        };

        return result;
    }

    public static IEnumerable<IUploadPictureModel> Map(this IReadOnlyList<IFrUploadPictureOnlyId> sourceItems)
    {
        return sourceItems.Select(sourceItem => sourceItem.Map()!).ToList();
    }

    public static IUploadPictureModel Map(this IFrUploadPictureIdWithFilename sourceItem)
    {
        var newItem = new UploadPictureModel()
        {
            Id = sourceItem.Id,
            Filename = sourceItem.Filename
        };

        return newItem;
    }

    public static IUploadPictureModel Map(this IFrUploadPictureForChooseView sourceItem)
    {
        var result = new UploadPictureModel()
        {
            Id = sourceItem.Id,
            Filename = sourceItem.Filename,
            Status = sourceItem.Status,
            CreateDate = sourceItem.CreateDate,
        };

        return result;
    }

    public static IEnumerable<IUploadPictureModel> Map(this IReadOnlyList<IFrUploadPictureForChooseView> sourceItems)
    {
        return sourceItems.Select(sourceItem => sourceItem.Map()).ToList();
    }

    public static IUploadPictureModel Map(this IFrUploadPictureForUploadView sourceItem)
    {
        var result = new UploadPictureModel()
        {
            Id = sourceItem.Id,
            Filename = sourceItem.Filename,
            Height = sourceItem.Height,
            Width = sourceItem.Width,
            Status = sourceItem.Status,
            CreateDate = sourceItem.CreateDate,
            UploadPictureExifInfo = sourceItem.UploadPictureExifInfo.Map(),
            GoogleGeoCodingAddress = sourceItem.GoogleGeoCodingAddress.Map()
        };

        return result;
    }

    public static IEnumerable<IUploadPictureModel> Map(this IReadOnlyList<IFrUploadPictureForUploadView> sourceItems)
    {
        return sourceItems.Select(sourceItem => sourceItem.Map()).ToList();
    }

    public static IUploadPictureModel? Map(this IFrUploadPictureForMediaItem sourceItem)
    {
        var result = new UploadPictureModel()
        {
            Id = sourceItem.Id,
            Filename = sourceItem.Filename,
            Height = sourceItem.Height,
            Width = sourceItem.Width,
            Status = sourceItem.Status,
            CreateDate = sourceItem.CreateDate,
            UploadPictureExifInfo = sourceItem.UploadPictureExifInfo.Map(),
            GoogleGeoCodingAddress = sourceItem.GoogleGeoCodingAddress.Map()
        };

        return result;
    }

    public static IUploadPictureExifInfoModel? Map(this IFrUploadPictureExifInfo? source)
    {
        if (source is null) return null;
        
        var result = new UploadPictureExifInfoModel()
        {
            Contrast = source.Contrast,
            DdlRecorded = source.DdlRecorded,
            ExposureMode = source.ExposureMode,
            ExposureProgram = source.ExposureProgram,
            ExposureTime = source.ExposureTime,
            FlashMode = source.FlashMode,
            FNumber = source.FNumber,
            FocalLength = source.FocalLength,
            GpsLatitude = source.GpsLatitude,
            GpsLongitude = source.GpsLongitude,
            IsoSensitivity = source.IsoSensitivity,
            WhiteBalanceMode = source.WhiteBalanceMode,
            ShutterSpeed = source.ShutterSpeed,
            Make = source.Make,
            MeteringMode = source.MeteringMode,
            Model = source.Model,
            Orientation = source.Orientation,
            ResolutionUnit = source.ResolutionUnit,
            ResolutionX = source.ResolutionX,
            ResolutionY = source.ResolutionY,
            Saturation = source.Saturation,
            SensingMode = source.SensingMode,
            Sharpness = source.Sharpness,
        };

        return result;

    }
}
