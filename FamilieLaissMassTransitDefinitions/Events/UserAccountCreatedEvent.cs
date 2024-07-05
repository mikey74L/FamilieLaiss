using FamilieLaissMassTransitDefinitions.Contracts.Events;

namespace FamilieLaissMassTransitDefinitions.Events
{
    public class UserAccountCreatedEvent : iUserAccountCreatedEvent
    {
        /// <summary>
        /// The user account name (is unique)
        /// </summary>
        public string UserName { get; init; }

        /// <summary>
        /// The identifier for the user
        /// </summary>
        public string ID { get; init; }

        /// <summary>
        /// eMail-Adress
        /// </summary>
        public string Email { get; init; }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="id">The identifier for the user</param>
        /// <param name="userName">The user account name</param>
        /// <param name="eMail">eMail adress</param>
        public UserAccountCreatedEvent(string id, string userName, string eMail)
        {
            ID = id;
            UserName = userName;
            Email = eMail;
        }
    }
}
