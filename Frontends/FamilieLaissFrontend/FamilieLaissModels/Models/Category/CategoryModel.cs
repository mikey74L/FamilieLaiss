using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.Extensions;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissSharedObjects.Extensions;
using System.Globalization;

namespace FamilieLaissModels.Models.Category;

public class CategoryModel : ICategoryModel
{
    public long Id { get; set; } = -1;
    public EnumCategoryType? CategoryType { get; set; } = EnumCategoryType.Picture;
    public string? NameGerman { get; set; }
    public string? NameEnglish { get; set; }
    public DateTimeOffset? CreateDate { get; set; }
    public DateTimeOffset? ChangeDate { get; set; }

    public List<ICategoryValueModel>? CategoryValues { get; set; }

    public string? LocalizedName =>
        CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "de" ? NameGerman : NameEnglish;

    public string CategoryTypeText
    {
        get
        {
            var targetEnum =
                CategoryType?.MapEnum<EnumCategoryType, FamilieLaissSharedObjects.Enums.EnumCategoryType>();

            if (targetEnum is not null)
            {
                return targetEnum.Description();
            }

            return "Missing Enum Description Attribute";
        }
    }

    public ICategoryModel Clone()
    {
        return new CategoryModel()
        {
            Id = Id,
            CategoryType = CategoryType,
            NameGerman = NameGerman,
            NameEnglish = NameEnglish,
            CreateDate = CreateDate,
            ChangeDate = ChangeDate
        };
    }

    public void TakeOverValues(ICategoryModel sourceModel)
    {
        CategoryType = sourceModel.CategoryType;
        NameGerman = sourceModel.NameGerman;
        NameEnglish = sourceModel.NameEnglish;
    }
}