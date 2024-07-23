using FamilieLaissMassTransitDefinitions.Contracts.Events.UploadPicture;

namespace FamilieLaissMassTransitDefinitions.Events.UploadPicture;

public class MassPictureConvertProgressEvent : IMassPictureConvertProgressEvent
{
    public required long UploadPictureId { get; init; }
    public required long ConvertStatusId { get; init; }
}