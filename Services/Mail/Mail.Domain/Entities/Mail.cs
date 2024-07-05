using DomainHelper.AbstractClasses;
using FamilieLaissSharedObjects.Enums;
using System.Threading.Tasks;

namespace Mail.Domain.Entities
{
    public class Mail: EntityCreation<long>
    {
        #region Properties
        public string SenderAdress { get; private set; }

        public string SenderName { get; private set; }

        public string ReceiverAdress { get; private set; }

        public string ReceiverName { get; private set; }

        public string Subject { get; private set; }

        public bool IsBodyHTML { get; private set; }

        public string Body { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor without parameters would be used by EF-Core
        /// </summary>
        private Mail()
        {

        }

       /// <summary>
       /// C'tor
       /// </summary>
       /// <param name="senderType">The type of sender for this mail</param>
       /// <param name="receiverAdress">The eMail-Adress for the receiver</param>
       /// <param name="receiverName">The name for the receiver</param>
       /// <param name="subject">The subject for this mail</param>
       /// <param name="isBodyHTML">Is body of this mail in HTML-Format?</param>
       /// <param name="body">The body of this mail</param>
       public Mail(enMailSenderType senderType, string receiverAdress, string receiverName, string subject, bool isBodyHTML, string body)
        {
            SetSender(senderType);
            ReceiverAdress = receiverAdress;
            ReceiverName = receiverName;
            Subject = subject;
            IsBodyHTML = isBodyHTML;
            Body = body;
        }
        #endregion

        #region Private Methods
        private void SetSender(enMailSenderType senderType)
        {
            switch (senderType)
            {
                case enMailSenderType.Account:
                    SenderAdress = "account@familielaiss.de";
                    SenderName = "Account - Familie Laiss";
                    break;
                case enMailSenderType.Administrator:
                    SenderAdress = "administrator@familielaiss.de";
                    SenderName = "Administrator - Familie Laiss";
                    break;
                case enMailSenderType.Notification:
                    SenderAdress = "notification@familielaiss.de";
                    SenderName = "Notification - Familie Laiss";
                    break;
                case enMailSenderType.Support:
                    SenderAdress = "support@familielaiss.de";
                    SenderName = "Support - Familie Laiss";
                    break;
            }
        }
        #endregion

        #region Called from Change Tracker
        public override Task EntityAddedAsync()
        {
            return Task.CompletedTask;
        }

        public override Task EntityDeletedAsync()
        {
            return Task.CompletedTask;
        }
        #endregion

        #region Domain Methods
        #endregion
    }
}
