using FamilieLaissMassTransitDefinitions.Contracts.Events.UploadVideo;

namespace FamilieLaissMassTransitDefinitions.Events.UploadVideo;

public class MassVideoUploadedEvent : IMassVideoUploadedEvent
{
    #region Implementation of IMassPictureUploadedEvent

    /// <inheritdoc />
    public required long Id { get; init; }

    /// <inheritdoc />
    public required string Filename { get; init; }

    #endregion
}