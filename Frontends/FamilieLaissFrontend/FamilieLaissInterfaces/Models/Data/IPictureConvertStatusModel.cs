using FamilieLaissGraphQlClientLibrary;

namespace FamilieLaissInterfaces.Models.Data;

public interface IPictureConvertStatusModel : IBaseModel<IPictureConvertStatusModel>
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

    public string PictureConvertStatusText { get; }
}