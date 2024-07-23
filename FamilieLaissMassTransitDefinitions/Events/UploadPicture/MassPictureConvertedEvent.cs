using FamilieLaissMassTransitDefinitions.Contracts.Events.UploadPicture;

namespace FamilieLaissMassTransitDefinitions.Events.UploadPicture;

public class MassPictureConvertedEvent : IMassPictureConvertedEvent
{
    #region Interface iPictureConvertedEvent

    public required long UploadPictureId { get; init; }

    public required long ConvertStatusId { get; init; }

    #endregion
}