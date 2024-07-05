using FamilieLaissMassTransitDefinitions.Contracts.Events;

namespace FamilieLaissMassTransitDefinitions.Events
{
    /// <summary>
    /// MassTransit-Event-Class for upload video was assigned event
    /// </summary>
    public class UploadVideoAssignedEvent : iUploadVideoAssignedEvent
    {
        #region Properties
        /// <summary>
        /// Identifier for upload video
        /// </summary>
        public long ID { get; init; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="id">Identifier for upload video</param>
        public UploadVideoAssignedEvent(long id)
        {
            ID = id;
        }
        #endregion
    }
}
