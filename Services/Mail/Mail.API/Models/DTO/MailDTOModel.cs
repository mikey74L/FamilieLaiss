using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mail.API.Models.DTO
{
    /// <summary>
    /// DTO-Model for Mail
    /// </summary>
    public class MailDTOModel
    {
        #region Properties
        /// <summary>
        /// eMail-Adress for sender of this mail
        /// </summary>
        public string SenderAdress { get; set; }

        /// <summary>
        /// The name for sender of this mail
        /// </summary>
        public string SenderName { get; set; }

        /// <summary>
        /// The eMail-Adress for receiver of this mail
        /// </summary>
        public string ReceiverAdress { get; set; }

        /// <summary>
        /// The name for receiver of this mail
        /// </summary>
        public string ReceiverName { get; set; }

        /// <summary>
        /// The subject for this mail
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Is the body of this mail in HTML-Format
        /// </summary>
        public bool IsBodyHTML { get; set; }

        /// <summary>
        /// The mail content for this mail
        /// </summary>
        public string Body { get; set; }
        #endregion
    }
}
