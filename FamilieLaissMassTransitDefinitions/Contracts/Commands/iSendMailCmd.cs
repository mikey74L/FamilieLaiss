using FamilieLaissSharedObjects.Enums;

namespace FamilieLaissMassTransitDefinitions.Contracts.Commands
{
    public interface iSendMailCmd
    {
        enMailSenderType MailSenderType { get; }

        string Subject { get; }

        string Body { get; }

        string ReceiverAdress { get; }

        string ReceiverName { get; }

        bool IsBodyHTML { get; }
    }
}
