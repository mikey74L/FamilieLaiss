using DomainHelper.AbstractClasses;
using Message.Domain.DomainEvents;
using System;
using System.Threading.Tasks;

namespace Message.Domain.Aggregates
{
    /// <summary>
    /// Entity for user message
    /// </summary>
    public class MessageUser : EntityBase<long>
    {
        #region Properties
        /// <summary>
        /// The Message this Message-User entry belongs to
        /// </summary>
        public Message Message { get; private set; }

        /// <summary>
        /// The username this Message-User entry belongs to
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// Is the message already readed by the user
        /// </summary>
        public bool Readed { get; private set; }

        /// <summary>
        /// When was the message readed by the user
        /// </summary>
        public DateTimeOffset? DDL_Readed { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor without parameters would be used by EF-Core
        /// </summary>
        private MessageUser()
        {

        }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="message">The message for this user</param>
        /// <param name="userName">The username for this message</param>
        internal MessageUser(Message message, string userName)
        {
            //Übernehmen der Werte
            Message = message;
            UserName = userName;
            Readed = false;
        }
        #endregion

        #region Domain-Methods
        /// <summary>
        /// Set this message for this user to the state readed
        /// </summary>
        internal void SetReaded()
        {
            Readed = true;
            DDL_Readed = DateTimeOffset.Now;
        }
        #endregion

        #region Called from Change-Tracker
        public override Task EntityAddedAsync()
        {
            //Hinzufügen des Domain-Events
            AddDomainEvent(new MtrEventMessageForUserCreated(Message.Id, UserName, Message.Prio, Message.Text_German, Message.Text_English, Message.AdditionalData, Message.CreateDate));

            //Ergebnis zurückliefern
            return Task.CompletedTask;
        }

        public override Task EntityDeletedAsync()
        {
            //Ergebnis zurückliefern
            return Task.CompletedTask;
        }
        #endregion
    }
}
