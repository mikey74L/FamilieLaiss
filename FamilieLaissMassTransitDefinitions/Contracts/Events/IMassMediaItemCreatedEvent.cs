using FamilieLaissSharedObjects.Enums;

namespace FamilieLaissMassTransitDefinitions.Contracts.Events;

/// <summary>
/// Media item created event (Event-Class for MassTransit)
/// </summary>
public interface IMassMediaItemCreatedEvent
{
    /// <summary>
    /// Identifier for media item
    /// </summary>
    long Id { get; }

    /// <summary>
    /// The type for media item
    /// </summary>
    EnumMediaType MediaType { get; }
}