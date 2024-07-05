using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissModels.Models.CategoryValue;

namespace FamilieLaissMappingExtensions.CategoryValue;

public static class CategoryValueMappingExtensions
{
    public static ICategoryValueModel Map(this IFrCategoryValueFull sourceItem)
    {
        var newItem = new CategoryValueModel()
        {
            Id = sourceItem.Id,
            NameGerman = sourceItem.NameGerman,
            NameEnglish = sourceItem.NameEnglish,
            CreateDate = sourceItem.CreateDate,
            ChangeDate = sourceItem.ChangeDate
        };

        return newItem;
    }

    public static IEnumerable<ICategoryValueModel> Map(this IReadOnlyList<IFrCategoryValueFull> sourceItems)
    {
        var result = new List<ICategoryValueModel>();

        foreach (var sourceItem in sourceItems)
        {
            result.Add(sourceItem.Map());
        }

        return result;
    }
}
