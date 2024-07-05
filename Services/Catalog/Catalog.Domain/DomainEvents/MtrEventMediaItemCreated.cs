using DomainHelper.DomainEvents;
using FamilieLaissSharedObjects.Enums;

namespace Catalog.Domain.DomainEvents
{
    /// <summary>
    /// Event for media item created
    /// </summary>
    public class MtrEventMediaItemCreated : DomainEventSingle
    {
        #region Properties
        /// <summary>
        /// Type for media item
        /// </summary>
        public EnumMediaType MediaType { get; init; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="id">Identifier for media item</param>
        /// <param name="mediaType">Type for media item</param>
        public MtrEventMediaItemCreated(long id, EnumMediaType mediaType) : base(id.ToString())
        {
            MediaType = mediaType;
        }
        #endregion
    }
}
