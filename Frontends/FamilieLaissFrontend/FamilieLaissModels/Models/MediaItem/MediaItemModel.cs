using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.Models.Data;
using System.Globalization;

namespace FamilieLaissModels.Models.MediaItem;

public class MediaItemModel : IMediaItemModel
{
    public long Id { get; set; }
    public IMediaGroupModel? MediaGroup { get; set; }
    public long? MediaGroupId { get; set; }
    public EnumMediaType? MediaType { get; set; }
    public string? NameGerman { get; set; }
    public string? NameEnglish { get; set; }
    public string? DescriptionGerman { get; set; }
    public string? DescriptionEnglish { get; set; }
    public bool? OnlyFamily { get; set; }
    public IUploadPictureModel? UploadPicture { get; set; }
    public IUploadVideoModel? UploadVideo { get; set; }
    public List<IMediaItemCategoryValueModel>? MediaItemCategoryValues { get; set; }
    public DateTimeOffset? CreateDate { get; set; }
    public DateTimeOffset? ChangeDate { get; set; }

    public string? LocalizedName
    {
        get
        {
            return CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "de" ? NameGerman : NameEnglish;
        }
    }

    public string? LocalizedDescription
    {
        get
        {
            return CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "de" ? DescriptionGerman : DescriptionEnglish;
        }
    }

    public IMediaItemModel Clone()
    {
        throw new NotImplementedException();
    }

    public void TakeOverValues(IMediaItemModel sourceModel)
    {
        throw new NotImplementedException();
    }
}
