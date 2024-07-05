using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissModels.Models.UploadVideo;

public class UploadVideoModel : IUploadVideoModel
{
    public long Id { get; set; }
    public string? Filename { get; set; }
    public int? Height { get; set; }
    public int? Width { get; set; }
    public int? DurationHour { get; set; }
    public int? DurationMinute { get; set; }
    public int? DurationSecond { get; set; }
    public EnumUploadStatus? Status { get; set; }
    public EnumVideoType? VideoType { get; set; }
    public DateTimeOffset? CreateDate { get; set; }

    public UploadVideoModel()
    {
    }

    public string? VideoSize
    {
        get
        {
            if (Height is not null && Width is not null)
            {
                return $"{Width} x {Height}";
            }
            else
            {
                return null;
            }
        }
    }

    public string? VideoLength
    {
        get
        {
            var hour = DurationHour != null ? DurationHour.Value.ToString("D2") : "00";
            var minute = DurationMinute != null ? DurationMinute.Value.ToString("D2") : "00";
            var second = DurationSecond != null ? DurationSecond.Value.ToString("D2") : "00";

            return $"{hour}:{minute}:{second}";
        }
    }

    public bool IsSelected { get; set; }

    public IUploadVideoModel Clone()
    {
        throw new NotImplementedException("Cloning this model is not implemented.");
    }

    public void TakeOverValues(IUploadVideoModel sourceModel)
    {
        throw new NotImplementedException("Take over values for this model is not implemented.");
    }
}