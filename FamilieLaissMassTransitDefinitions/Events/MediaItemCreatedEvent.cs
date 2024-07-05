using FamilieLaissMassTransitDefinitions.Contracts.Events;
using FamilieLaissSharedObjects.Enums;

namespace FamilieLaissMassTransitDefinitions.Events
{
    public class MediaItemCreatedEvent: iMediaItemCreatedEvent
    {
        #region Properties
        /// <summary>
        /// Identifier for media item
        /// </summary>
        public long ID { get; private set; }

        /// <summary>
        /// Type for media item
        /// </summary>
        public EnumMediaType MediaType { get; private set; }
        #endregion

        #region C'tor
        public MediaItemCreatedEvent(long id, EnumMediaType mediaType)
        {
            ID = id;
            MediaType = mediaType;
        }
        #endregion
    }
}
