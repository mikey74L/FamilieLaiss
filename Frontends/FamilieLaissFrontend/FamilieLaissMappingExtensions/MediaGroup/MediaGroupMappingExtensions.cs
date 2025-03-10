using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissModels.Models.MediaGroup;

namespace FamilieLaissMappingExtensions.MediaGroup;

public static class MediaGroupMappingExtensions
{
    public static IEnumerable<IMediaGroupModel> Map(this IReadOnlyList<IFrMediaGroupFull> sourceItems)
    {
        return sourceItems.Select(sourceItem => sourceItem.Map()).ToList();
    }

    public static IMediaGroupModel Map(this IFrMediaGroupFull sourceItem)
    {
        var newItem = new MediaGroupModel()
        {
            Id = sourceItem.Id,
            NameGerman = sourceItem.NameGerman,
            NameEnglish = sourceItem.NameEnglish,
            DescriptionGerman = sourceItem.DescriptionGerman,
            DescriptionEnglish = sourceItem.DescriptionEnglish,
            EventDate = sourceItem.EventDate,
            CreateDate = sourceItem.CreateDate,
            ChangeDate = sourceItem.ChangeDate
        };

        return newItem;
    }
}
