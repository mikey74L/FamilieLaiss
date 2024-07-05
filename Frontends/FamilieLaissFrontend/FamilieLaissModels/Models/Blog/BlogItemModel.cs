using FamilieLaissInterfaces.Models.Data;
using System.Globalization;

namespace FamilieLaissModels.Models.Blog;

public class BlogItemModel : IBlogItemModel
{
    public long Id { get; set; }
    public string? HeaderGerman { get; set; }
    public string? HeaderEnglish { get; set; }
    public string? TextGerman { get; set; }
    public string? TextEnglish { get; set; }
    public DateTimeOffset? CreateDate { get; set; }
    public DateTimeOffset? ChangeDate { get; set; }

    public BlogItemModel()
    {
        Id = -1;
        HeaderGerman = string.Empty;
        HeaderEnglish = string.Empty;
        TextGerman = string.Empty;
        TextEnglish = string.Empty;
    }

    public string? LocalizedHeader => CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "de" ? HeaderGerman : HeaderEnglish;

    public string? LocalizedText => CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "de" ? TextGerman : TextEnglish;

    public IBlogItemModel Clone()
    {
        throw new NotImplementedException();
    }

    public void TakeOverValues(IBlogItemModel sourceModel)
    {
        throw new NotImplementedException();
    }
}
