using FamilieLaissMassTransitDefinitions.Contracts.Events;

namespace FamilieLaissMassTransitDefinitions.Events
{
    public class UploadVideoDeletedEvent : iUploadVideoDeletedEvent
    {
        #region Properties
        /// <summary>
        /// The primary key for upload video
        /// </summary>
        public long ID { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="id">ID for upload video</param>
        public UploadVideoDeletedEvent(long id)
        {
            ID = id;
        }
        #endregion
    }
}
