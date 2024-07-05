using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissMappingExtensions.UploadPicture;
using FamilieLaissModels.Models.MediaItem;

namespace FamilieLaissMappingExtensions.MediaItem;

public static class MediaItemMappingExtensions
{
    public static IEnumerable<IMediaItemModel> Map(this IReadOnlyList<IFrMediaItemFull> sourceItems)
    {
        var result = new List<IMediaItemModel>();

        foreach (var sourceItem in sourceItems)
        {
            result.Add(sourceItem.Map());
        }

        return result;
    }

    public static IMediaItemModel Map(this IFrMediaItemFull sourceItem)
    {
        var newItem = new MediaItemModel()
        {
            Id = sourceItem.Id,
            MediaType = sourceItem.MediaType,
            NameGerman = sourceItem.NameGerman,
            NameEnglish = sourceItem.NameEnglish,
            DescriptionGerman = sourceItem.DescriptionGerman,
            DescriptionEnglish = sourceItem.DescriptionEnglish,
            OnlyFamily = sourceItem.OnlyFamily,
            CreateDate = sourceItem.CreateDate,
            ChangeDate = sourceItem.ChangeDate
        };

        if (sourceItem.UploadPicture is not null)
        {
            newItem.UploadPicture = sourceItem.UploadPicture.Map();
        }

        if (sourceItem.UploadVideo is not null)
        {
            //newItem.UploadVideo = sourceItem.UploadVideo.Map();
        }

        return newItem;
    }
}
