using DomainHelper.DomainEvents;
using FamilieLaissSharedObjects.Enums;

namespace Catalog.Domain.DomainEvents
{
    /// <summary>
    /// Event for media item deleted
    /// </summary>
    public class MtrEventMediaItemDeleted : DomainEventSingle
    {
        #region Properties
        /// <summary>
        /// Media-Type for this media item
        /// </summary>
        public EnumMediaType MediaType { get; init; }

        /// <summary>
        /// Identifier for assigned upload item
        /// </summary>
        public long UploadItemID { get; init; }

        /// <summary>
        /// Delete upload item?
        /// </summary>
        public bool DeleteUploadItem { get; init; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="id">Identifier for media item</param>
        /// <param name="mediaType">Media type for media item</param>
        /// <param name="uploadItemID">Identifier for upload item</param>
        /// <param name="deleteUploadItem">Delete upload item?</param>
        public MtrEventMediaItemDeleted(long id, EnumMediaType mediaType, long uploadItemID, bool deleteUploadItem) : base(id.ToString())
        {
            MediaType = mediaType;
            UploadItemID = uploadItemID;
            DeleteUploadItem = deleteUploadItem;
        }
        #endregion
    }
}
