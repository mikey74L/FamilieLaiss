using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissModels.Models.UploadPicture;

public class UploadPictureModel : IUploadPictureModel
{
    public long Id { get; set; }
    public string? Filename { get; set; }
    public int? Height { get; set; }
    public int? Width { get; set; }
    public EnumUploadStatus? Status { get; set; }
    public IUploadPictureExifInfoModel? UploadPictureExifInfo { get; set; }
    public IGoogleGeoCodingAddressModel? GoogleGeoCodingAddress { get; set; }

    public DateTimeOffset? CreateDate { get; set; }

    public bool IsSelected { get; set; }

    public UploadPictureModel()
    {
    }

    public string? PictureSize
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

    public IUploadPictureModel Clone()
    {
        throw new NotImplementedException("Cloning this model is not implemented.");
    }

    public void TakeOverValues(IUploadPictureModel sourceModel)
    {
        throw new NotImplementedException("Take over values for this model is not implemented.");
    }
}
