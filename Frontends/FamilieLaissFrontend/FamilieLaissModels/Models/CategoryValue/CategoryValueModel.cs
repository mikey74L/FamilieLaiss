using FamilieLaissInterfaces.Models.Data;
using System.Globalization;

namespace FamilieLaissModels.Models.CategoryValue;

public class CategoryValueModel : ICategoryValueModel
{
    public long Id { get; set; } = -1;
    public long? CategoryId { get; set; }

    public ICategoryModel? Category { get; set; }

    public string? NameGerman { get; set; }
    public string? NameEnglish { get; set; }
    public DateTimeOffset? CreateDate { get; set; }
    public DateTimeOffset? ChangeDate { get; set; }

    public string? LocalizedName =>
        CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "de" ? NameGerman : NameEnglish;

    public ICategoryValueModel Clone()
    {
        return new CategoryValueModel()
        {
            Id = Id,
            CategoryId = CategoryId,
            NameGerman = NameGerman,
            NameEnglish = NameEnglish,
            CreateDate = CreateDate,
            ChangeDate = ChangeDate
        };
    }

    public void TakeOverValues(ICategoryValueModel sourceModel)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return LocalizedName ?? this.GetType().Name;
    }
}