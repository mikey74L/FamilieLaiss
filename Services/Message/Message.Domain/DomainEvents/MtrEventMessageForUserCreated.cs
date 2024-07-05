using DomainHelper.DomainEvents;
using FamilieLaissSharedObjects.Enums;
using System;

namespace Message.Domain.DomainEvents
{
    /// <summary>
    /// Event for message for user added
    /// </summary>
    public class MtrEventMessageForUserCreated : DomainEventMultiple
    {
        #region Properties
        /// <summary>
        /// Username for user this message belongs to
        /// </summary>
        public string UserName { get; init; }

        /// <summary>
        /// Priority for message
        /// </summary>
        public enMessagePrio Prio { get; init; }

        /// <summary>
        /// Deutscher Text der Nachricht
        /// </summary>
        public string Text_German { get; init; }

        /// <summary>
        /// Englischer Text der Nachricht
        /// </summary>
        public string Text_English { get; init; }

        /// <summary>
        /// Additional data for message
        /// </summary>
        public string AdditionalData { get; init; }

        /// <summary>
        /// Create date for message
        /// </summary>
        public DateTimeOffset CreateDate { get; init; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="messageID">Identifier for message</param>
        /// <param name="userName">Username for user this message belongs to</param>
        /// <param name="prio">Priority for message</param>
        /// <param name="textGerman">Deutscher Text der Nachricht</param>
        /// <param name="textEnglish">Englischer Text der Nachricht</param>
        /// <param name="additionalData">Additional data for message</param>
        /// <param name="createDate">Create date for message</param>
        public MtrEventMessageForUserCreated(long messageID, string userName, enMessagePrio prio, string textGerman, string textEnglish,
            string additionalData, DateTimeOffset createDate) : base(messageID.ToString())
        {
            UserName = userName;
            Prio = prio;
            Text_German = textGerman;
            Text_English = textEnglish;
            AdditionalData = additionalData;
            CreateDate = createDate;
        }
        #endregion
    }
}
