using FamilieLaissMassTransitDefinitions.Contracts.Events;

namespace FamilieLaissMassTransitDefinitions.Events;

public class PictureConvertErrorEvent : IPictureConvertErrorEvent
{
    #region Implementation of IPictureConvertErrorEvent

    /// <inheritdoc />
    public required long UploadPictureId { get; init; }

    /// <inheritdoc />
    public required long ConvertStatusId { get; init; }

    #endregion
}