using DomainHelper.DomainEvents;

namespace User.Domain.DomainEvents
{
    /// <summary>
    /// Event for user created
    /// </summary>
    public class MtrEventUserCreated : DomainEventSingle
    {
        #region Properties
        /// <summary>
        /// E-Mail-Adress for user
        /// </summary>
        public string EMail { get; init; }

        /// <summary>
        /// User-Name for user
        /// </summary>
        public string UserName { get; init; }
        #endregion

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="id">Identifier for media item</param>
        /// <param name="eMail">E-Mail-Adress for user</param>
        /// <param name="userName">User-Name for user</param>
        public MtrEventUserCreated(string id, string eMail, string userName) : base(id)
        {
            EMail = eMail;
            UserName = userName;
        }
    }
}
