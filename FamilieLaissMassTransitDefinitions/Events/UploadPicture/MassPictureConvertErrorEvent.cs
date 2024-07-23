using FamilieLaissMassTransitDefinitions.Contracts.Events.UploadPicture;

namespace FamilieLaissMassTransitDefinitions.Events.UploadPicture;

public class MassPictureConvertErrorEvent : IMassPictureConvertErrorEvent
{
    #region Implementation of IPictureConvertErrorEvent

    /// <inheritdoc />
    public required long UploadPictureId { get; init; }

    /// <inheritdoc />
    public required long ConvertStatusId { get; init; }

    #endregion
}