using FamilieLaissMassTransitDefinitions.Contracts.Events;
using FamilieLaissSharedObjects.Enums;

namespace FamilieLaissMassTransitDefinitions.Events;

public class MassMediaItemCreatedEvent : IMassMediaItemCreatedEvent
{
    #region Properties

    /// <summary>
    /// Identifier for media item
    /// </summary>
    public long Id { get; private set; }

    /// <summary>
    /// Type for media item
    /// </summary>
    public EnumMediaType MediaType { get; private set; }

    #endregion

    #region C'tor

    public MassMediaItemCreatedEvent(long id, EnumMediaType mediaType)
    {
        Id = id;
        MediaType = mediaType;
    }

    #endregion
}