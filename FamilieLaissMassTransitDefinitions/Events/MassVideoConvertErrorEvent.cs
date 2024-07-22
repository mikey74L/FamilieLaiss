using FamilieLaissMassTransitDefinitions.Contracts.Events;

namespace FamilieLaissMassTransitDefinitions.Events;

public class MassVideoConvertErrorEvent : IMassVideoConvertErrorEvent
{
    #region Implementation of IVideoConvertErrorEvent

    /// <inheritdoc />
    public required long UploadVideoId { get; init; }

    /// <inheritdoc />
    public required long ConvertStatusId { get; init; }

    #endregion
}