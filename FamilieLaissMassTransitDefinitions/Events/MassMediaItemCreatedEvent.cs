using FamilieLaissMassTransitDefinitions.Contracts.Events;
using FamilieLaissSharedObjects.Enums;

namespace FamilieLaissMassTransitDefinitions.Events;

public class MassMediaItemCreatedEvent : IMassMediaItemCreatedEvent
{
    #region Properties

    /// <inheritdoc />
    public required long Id { get; init; }

    /// <inheritdoc />
    public required EnumMediaType MediaType { get; init; }

    /// <inheritdoc />
    public required long UploadItemId { get; init; }

    #endregion
}