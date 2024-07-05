using DomainHelper.DomainEvents;

namespace UserInteraction.Domain.DomainEvents
{
    /// <summary>
    /// Event for favorite added
    /// </summary>
    public class MtrEventFavoriteAdded : DomainEventMultiple
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
        /// <param name="userAccountID">Identifier for user account</param>
        public MtrEventFavoriteAdded(long id, long userInteractionInfoID, string userAccountID) : base(id.ToString())
        {
            UserInteractionInfoID = userInteractionInfoID;
            UserAccountID = userAccountID;
        }
        #endregion
    }
}
