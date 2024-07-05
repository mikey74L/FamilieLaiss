using FamilieLaissMassTransitDefinitions.Contracts.Events;

namespace FamilieLaissMassTransitDefinitions.Events
{
    /// <summary>
    /// MassTransit-Event-Class for upload picture was assigned event
    /// </summary>
    public class UploadPictureAssignedEvent : iUploadPictureAssignedEvent
    {
        #region Properties
        /// <summary>
        /// Identifier for upload picture
        /// </summary>
        public long ID { get; init; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="id">Identifier for upload picture</param>
        public UploadPictureAssignedEvent(long id)
        {
            ID = id;
        }
        #endregion
    }
}
