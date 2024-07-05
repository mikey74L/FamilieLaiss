using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissMappingExtensions.CategoryValue;
using FamilieLaissModels.Models.Category;

namespace FamilieLaissMappingExtensions.Category;

public static class CategoryMappingExtensions
{
    public static IEnumerable<ICategoryModel> Map(this IReadOnlyList<IFrCategoryFull> sourceItems)
    {
        var result = new List<ICategoryModel>();

        foreach (var sourceItem in sourceItems)
        {
            result.Add(sourceItem.Map());
        }

        return result;
    }

    public static ICategoryModel Map(this IFrCategoryFull sourceItem)
    {
        var newItem = new CategoryModel()
        {
            Id = sourceItem.Id,
            CategoryType = sourceItem.CategoryType,
            NameGerman = sourceItem.NameGerman,
            NameEnglish = sourceItem.NameEnglish,
            CreateDate = sourceItem.CreateDate,
            ChangeDate = sourceItem.ChangeDate
        };

        return newItem;
    }

    public static IEnumerable<ICategoryModel> Map(this IReadOnlyList<IFrGetCategoryValuesForCategory> sourceItems)
    {
        var result = new List<ICategoryModel>();

        foreach (var sourceItem in sourceItems)
        {
            result.Add(sourceItem.Map());
        }

        return result;
    }

    public static ICategoryModel Map(this IFrGetCategoryValuesForCategory sourceItem)
    {
        var newItem = new CategoryModel()
        {
            Id = sourceItem.Id,
            CategoryType = sourceItem.CategoryType,
            NameGerman = sourceItem.NameGerman,
            NameEnglish = sourceItem.NameEnglish,
            CategoryValues = sourceItem.CategoryValues.Map().ToList()
        };

        return newItem;
    }


}
