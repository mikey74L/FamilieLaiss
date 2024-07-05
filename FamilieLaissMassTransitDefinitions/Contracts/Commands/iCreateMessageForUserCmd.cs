using FamilieLaissSharedObjects.Enums;

namespace FamilieLaissMassTransitDefinitions.Contracts.Commands
{
    public interface iCreateMessageForUserCmd
    {
        enMessagePrio MessagePrio { get; }

        string UserName { get; }

        string TextGerman { get; }

        string TextEnglish { get; }

        string AdditionalData { get; }
    }
}
