using FamilieLaissMassTransitDefinitions.Contracts.Events;

namespace FamilieLaissMassTransitDefinitions.Events
{
    public class MediaItemDeletedEvent : iMediaItemDeletedEvent
    {
        #region Properties
        /// <summary>
        /// Identifier for media item
        /// </summary>
        public long ID { get; private set; }

        /// <summary>
        /// Identifier for assigned upload item
        /// </summary>
        public long UploadItemID { get; private set; }

        /// <summary>
        /// Should upload item be deleted
        /// </summary>
        public bool DeleteUploadItem { get; private set; }

        /// <summary>
        /// Is media item a picture
        /// </summary>
        public bool IsPicture { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="id">Identifier for media item</param>
        /// <param name="uploadItemID">Identifier for upload item</param>
        /// <param name="isPicture">Is media item a picture</param>
        /// <param name="deleteUploadItem">Should upload item be deleted</param>
        public MediaItemDeletedEvent(long id, long uploadItemID, bool isPicture, bool deleteUploadItem)
        {
            ID = id;
            UploadItemID = uploadItemID;
            IsPicture = isPicture;
            DeleteUploadItem = deleteUploadItem;
        }
        #endregion
    }
}
