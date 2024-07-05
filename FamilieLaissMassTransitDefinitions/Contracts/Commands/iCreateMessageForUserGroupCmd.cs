using FamilieLaissSharedObjects.Enums;

namespace FamilieLaissMassTransitDefinitions.Contracts.Commands
{
    public interface iCreateMessageForUserGroupCmd
    {
        enMessagePrio MessagePrio { get; }

        string GroupName { get; }

        string TextGerman { get; }

        string TextEnglish { get; }

        string AdditionalData { get; }
    }
}
