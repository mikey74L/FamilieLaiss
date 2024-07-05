using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.Extensions;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissSharedObjects.Extensions;

namespace FamilieLaissModels.Models.PictureConvert;

public class PictureConvertStatusModel : IPictureConvertStatusModel
{
    public long Id { get; set; }

    public EnumPictureConvertStatus? Status { get; set; }

    public string? ErrorMessage { get; set; }

    public DateTimeOffset? StartDateInfo { get; set; }

    public DateTimeOffset? FinishDateInfo { get; set; }

    public DateTimeOffset? StartDateExif { get; set; }

    public DateTimeOffset? FinishDateExif { get; set; }

    public DateTimeOffset? StartDateConvert { get; set; }

    public DateTimeOffset? FinishDateConvert { get; set; }

    public IUploadPictureModel? UploadPicture { get; set; }

    public string PictureConvertStatusText
    {
        get
        {
            var targetEnum =
                Status?.MapEnum<EnumPictureConvertStatus, FamilieLaissSharedObjects.Enums.EnumPictureConvertStatus>();

            if (targetEnum is not null)
            {
                return targetEnum.Description();
            }

            return "Missing Enum Description Attribute";
        }
    }

    public IPictureConvertStatusModel Clone()
    {
        throw new NotImplementedException("Cloning this model is not implemented.");
    }

    public void TakeOverValues(IPictureConvertStatusModel sourceModel)
    {
        throw new NotImplementedException("Take over values for this model is not implemented.");
    }
}