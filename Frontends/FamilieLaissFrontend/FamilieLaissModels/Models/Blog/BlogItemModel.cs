using FamilieLaissInterfaces.Models.Data;
using System.Globalization;

namespace FamilieLaissModels.Models.Blog;

public class BlogItemModel : IBlogItemModel
{
    public long Id { get; set; } = -1;
    public string? HeaderGerman { get; set; } = string.Empty;
    public string? HeaderEnglish { get; set; } = string.Empty;
    public string? TextGerman { get; set; } = string.Empty;
    public string? TextEnglish { get; set; } = string.Empty;
    public DateTimeOffset? CreateDate { get; set; }
    public DateTimeOffset? ChangeDate { get; set; }

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
