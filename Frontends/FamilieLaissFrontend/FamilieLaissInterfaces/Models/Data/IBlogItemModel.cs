namespace FamilieLaissInterfaces.Models.Data;

public interface IBlogItemModel : IBaseModel<IBlogItemModel>
{
    public long Id { get; set; }

    public string? HeaderGerman { get; set; }

    public string? HeaderEnglish { get; set; }

    public string? TextGerman { get; set; }

    public string? TextEnglish { get; set; }

    public DateTimeOffset? CreateDate { get; set; }

    public DateTimeOffset? ChangeDate { get; set; }

    public string? LocalizedHeader { get; }

    public string? LocalizedText { get; }
}
