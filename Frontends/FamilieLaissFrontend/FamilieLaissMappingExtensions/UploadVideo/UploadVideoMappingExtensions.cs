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
        var result = new List<IUploadVideoModel>();

        foreach (var sourceItem in sourceItems)
        {
            result.Add(sourceItem.Map());
        }

        return result;
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
            Status = sourceItem.Status,
            CreateDate = sourceItem.CreateDate,
        };

        return result;
    }

    public static IEnumerable<IUploadVideoModel> Map(this IReadOnlyList<IFrUploadVideoForChooseView> sourceItems)
    {
        var result = new List<IUploadVideoModel>();

        foreach (var sourceItem in sourceItems)
        {
            result.Add(sourceItem.Map());
        }

        return result;
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
        var result = new List<IUploadVideoModel>();

        foreach (var sourceItem in sourceItems)
        {
            var newItem = new UploadVideoModel()
            {
                Id = sourceItem.Id,
                Filename = sourceItem.Filename,
                Height = sourceItem.Height,
                Width = sourceItem.Width,
                VideoType = sourceItem.VideoType,
                Status = sourceItem.Status,
                CreateDate = sourceItem.CreateDate,
                DurationHour = sourceItem.DurationHour,
                DurationMinute = sourceItem.DurationMinute,
                DurationSecond = sourceItem.DurationSecond,
                //GoogleGeoCodingAddress = sourceItem.GoogleGeoCodingAddress.Map()
            };

            result.Add(newItem);
        }

        return result;
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