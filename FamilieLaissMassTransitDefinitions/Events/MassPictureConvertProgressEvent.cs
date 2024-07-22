using FamilieLaissMassTransitDefinitions.Contracts.Events;

namespace FamilieLaissMassTransitDefinitions.Events;

public class MassPictureConvertProgressEvent : IMassPictureConvertProgressEvent
{
    public required long UploadPictureId { get; init; }
    public required long ConvertStatusId { get; init; }
}