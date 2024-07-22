using FamilieLaissMassTransitDefinitions.Contracts.Events;

namespace FamilieLaissMassTransitDefinitions.Events;

public class MassPictureUploadedEvent : IMassPictureUploadedEvent
{
    #region Implementation of IMassPictureUploadedEvent

    /// <inheritdoc />
    public required long Id { get; init; }

    /// <inheritdoc />
    public required string Filename { get; init; }

    #endregion
}