using System;

namespace Message.DTO
{
    public class MessageUserDTOModel
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// The Message this Message-User entry belongs to
        /// </summary>
        public MessageDTOModel Message { get; set; }

        /// <summary>
        /// The username this Message-User entry belongs to
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Is the message already readed by the user
        /// </summary>
        public bool Readed { get; set; }

        /// <summary>
        /// When was the message readed by the user
        /// </summary>
        public DateTimeOffset? DDL_Readed { get; set; }
    }
}
