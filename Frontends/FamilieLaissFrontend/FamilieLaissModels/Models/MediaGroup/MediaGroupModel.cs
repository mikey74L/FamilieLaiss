using FamilieLaissInterfaces.Models.Data;
using System.Globalization;

namespace FamilieLaissModels.Models.MediaGroup;

public class MediaGroupModel : IMediaGroupModel
{
    public long Id { get; set; } = -1;
    public string? NameGerman { get; set; }
    public string? NameEnglish { get; set; }
    public string? DescriptionGerman { get; set; }
    public string? DescriptionEnglish { get; set; }
    public DateTimeOffset? EventDate { get; set; }

    public DateTime? EventDateForInput
    {
        get
        {
            if (EventDate is null)
            {
                return null;
            }
            else
            {
                return EventDate.Value.LocalDateTime;
            }
        }
        set
        {
            if (value is null)
            {
                EventDate = null;
            }
            else
            {
                EventDate = new DateTimeOffset(value.Value);
            }
        }
    }

    public DateTimeOffset? CreateDate { get; set; }
    public DateTimeOffset? ChangeDate { get; set; }

    public List<IMediaItemModel> MediaItems { get; set; } = [];

    public string? LocalizedName =>
        CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "de" ? NameGerman : NameEnglish;

    public string? LocalizedDescription =>
        CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "de" ? DescriptionGerman : DescriptionEnglish;


    public IMediaGroupModel Clone()
    {
        return new MediaGroupModel()
        {
            Id = Id,
            NameGerman = NameGerman,
            NameEnglish = NameEnglish,
            DescriptionGerman = DescriptionGerman,
            DescriptionEnglish = DescriptionEnglish,
            EventDate = EventDate,
            CreateDate = CreateDate,
            ChangeDate = ChangeDate
        };
    }

    public void TakeOverValues(IMediaGroupModel sourceModel)
    {
        NameGerman = sourceModel.NameGerman;
        NameEnglish = sourceModel.NameEnglish;
        DescriptionGerman = sourceModel.DescriptionGerman;
        DescriptionEnglish = sourceModel.DescriptionEnglish;
        EventDate = sourceModel.EventDate;
    }
}
