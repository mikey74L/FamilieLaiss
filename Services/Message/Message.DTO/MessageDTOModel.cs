using FamilieLaissSharedObjects.Enums;
using System;
using System.Collections.Generic;

namespace Message.DTO
{
    public class MessageDTOModel
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// The priority of the message
        /// </summary>
        public enMessagePrio Prio { get; set; }

        /// <summary>
        /// The german message text
        /// </summary>
        public string Text_German { get; set; }

        /// <summary>
        /// The english message text
        /// </summary>
        public string Text_English { get; set; }

        /// <summary>
        /// Additional data for the message
        /// </summary>
        public string AdditionalData { get; set; }

        /// <summary>
        /// Timestamp for creation of this message
        /// </summary>
        public DateTimeOffset CreateDate { get; set; }

        /// <summary>
        /// List of related users for this message
        /// </summary>
        public List<MessageUserDTOModel> MessageUsers;
    }
}
