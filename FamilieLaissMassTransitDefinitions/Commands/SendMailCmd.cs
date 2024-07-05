using FamilieLaissMassTransitDefinitions.Contracts.Commands;
using FamilieLaissSharedObjects.Enums;

namespace FamilieLaissMassTransitDefinitions.Commands
{
    public class SendMailCmd : iSendMailCmd
    {
        public enMailSenderType MailSenderType { get; private set; }

        public string Subject { get; private set; }

        public string Body { get; private set; }

        public string ReceiverAdress { get; private set; }

        public string ReceiverName { get; private set; }

        public bool IsBodyHTML { get; private set; }

        public SendMailCmd(enMailSenderType mailSenderType, string subject, string body, string receiverAdress, string receiverName, bool isBodyHTML)
        {
            MailSenderType = mailSenderType;
            Subject = subject;
            Body = body;
            ReceiverAdress = receiverAdress;
            ReceiverName = receiverName;
            IsBodyHTML = isBodyHTML;
        }
    }
}
