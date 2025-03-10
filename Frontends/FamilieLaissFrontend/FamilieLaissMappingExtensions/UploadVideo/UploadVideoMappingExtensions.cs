using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissModels.Models.UploadVideo;

namespace FamilieLaissMappingExtensions.UploadVideo;

public static class UploadVideoMappingExtensions
{
    public static IUploadVideoModel Map(this IFrUploadVideoOnlyId source)
    {
        var result = new UploadVideoModel()
        {
            Id = source.Id,
        };

        return result;
    }

    public static IEnumerable<IUploadVideoModel> Map(this IReadOnlyList<IFrUploadVideoOnlyId> sourceItems)
    {
        return sourceItems.Select(sourceItem => sourceItem.Map()).ToList();
    }

    public static IUploadVideoModel Map(this IFrUploadVideoForChooseView sourceItem)
    {
        var result = new UploadVideoModel()
        {
            Id = sourceItem.Id,
            Filename = sourceItem.Filename,
            Height = sourceItem.Height,
            Width = sourceItem.Width,
            VideoType = sourceItem.VideoType,
            DurationHour = sourceItem.DurationHour,
            DurationMinute = sourceItem.DurationMinute,
            DurationSecond = sourceItem.DurationSecond,
            State = sourceItem.State,
            CreateDate = sourceItem.CreateDate,
        };

        return result;
    }

    public static IEnumerable<IUploadVideoModel> Map(this IReadOnlyList<IFrUploadVideoForChooseView> sourceItems)
    {
        return sourceItems.Select(sourceItem => sourceItem.Map()).ToList();
    }

    public static IUploadVideoModel Map(this IFrUploadVideoIdWithFilename sourceItem)
    {
        var newItem = new UploadVideoModel()
        {
            Id = sourceItem.Id,
            Filename = sourceItem.Filename
        };

        return newItem;
    }

    public static IEnumerable<IUploadVideoModel> Map(this IReadOnlyList<IFrUploadVideoForUploadView> sourceItems,
        IServiceProvider serviceProvider)
    {
        return sourceItems.Select(sourceItem => new UploadVideoModel()
        {
            Id = sourceItem.Id,
            Filename = sourceItem.Filename,
            Height = sourceItem.Height,
            Width = sourceItem.Width,
            VideoType = sourceItem.VideoType,
            State = sourceItem.State,
            CreateDate = sourceItem.CreateDate,
            DurationHour = sourceItem.DurationHour,
            DurationMinute = sourceItem.DurationMinute,
            DurationSecond = sourceItem.DurationSecond,
            //GoogleGeoCodingAddress = sourceItem.GoogleGeoCodingAddress.Map()
        })
            .Cast<IUploadVideoModel>()
            .ToList();
    }

    public static IUploadVideoModel Map(this IFrUploadVideoForConvert sourceItem)
    {
        var newItem = new UploadVideoModel()
        {
            Id = sourceItem.Id,
            Filename = sourceItem.Filename,
            Height = sourceItem.Height,
            Width = sourceItem.Width,
            DurationHour = sourceItem.DurationHour,
            DurationMinute = sourceItem.DurationMinute,
            DurationSecond = sourceItem.DurationSecond
        };

        return newItem;
    }
}