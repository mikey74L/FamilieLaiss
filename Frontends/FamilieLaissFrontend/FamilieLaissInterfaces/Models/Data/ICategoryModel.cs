using FamilieLaissGraphQlClientLibrary;

namespace FamilieLaissInterfaces.Models.Data;

public interface ICategoryModel : IBaseModel<ICategoryModel>
{
    public long Id { get; set; }

    public EnumCategoryType? CategoryType { get; set; }

    public string? NameGerman { get; set; }

    public string? NameEnglish { get; set; }

    public List<ICategoryValueModel>? CategoryValues { get; set; }

    public DateTimeOffset? CreateDate { get; set; }

    public DateTimeOffset? ChangeDate { get; set; }

    public string? LocalizedName { get; }

    public string CategoryTypeText { get; }
}