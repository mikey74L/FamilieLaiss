using DomainHelper.DomainEvents;

namespace UserInteraction.Domain.DomainEvents
{
    /// <summary>
    /// Event for rating deleted
    /// </summary>
    public class MtrEventRatingDeleted : DomainEventMultiple
    {
        #region Properties
        /// <summary>
        /// Identifier for user interaction info
        /// </summary>
        public long UserInteractionInfoID { get; init; }

        /// <summary>
        /// Identifier for user account
        /// </summary>
        public string UserAccountID { get; init; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="id">Identifier for media item</param>
        /// <param name="userInteractionInfoID">Identifier for user interaction info</param>
        public MtrEventRatingDeleted(long id, long userInteractionInfoID) : base(id.ToString())
        {
            UserInteractionInfoID = userInteractionInfoID;
        }
        #endregion
    }
}
