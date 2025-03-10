using DomainHelper.DomainEvents;

namespace UserInteraction.Domain.DomainEvents
{
    /// <summary>
    /// Event for comment added
    /// </summary>
    public class MtrEventCommentAdded : DomainEventMultiple
    {
        #region Properties
        /// <summary>
        /// Identifier for user interaction info
        /// </summary>
        public long UserInteractionInfoID { get; init; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="id">Identifier for media item</param>
        /// <param name="userInteractionInfoID">Identifier for user interaction info</param>
        public MtrEventCommentAdded(long id, long userInteractionInfoID) : base(id.ToString())
        {
            UserInteractionInfoID = userInteractionInfoID;
        }
        #endregion
    }
}
