using FamilieLaissMassTransitDefinitions.Contracts.Events;

namespace FamilieLaissMassTransitDefinitions.Events
{
    public class UploadPictureDeletedEvent : iUploadPictureDeletedEvent
    {
        #region Properties
        /// <summary>
        /// The primary key for upload picture
        /// </summary>
        public long ID { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="id">ID for upload picture</param>
        public UploadPictureDeletedEvent(long id)
        {
            ID = id;
        }
        #endregion
    }
}
