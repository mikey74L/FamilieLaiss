using FamilieLaissMassTransitDefinitions.Contracts.Commands;
using FamilieLaissSharedObjects.Enums;

namespace FamilieLaissMassTransitDefinitions.Commands
{
    public class CreateMessageForUserGroupCmd : iCreateMessageForUserGroupCmd
    {
        #region Properties
        public enMessagePrio MessagePrio { get; private set; }

        public string GroupName { get; private set; }

        public string TextGerman { get; private set; }

        public string TextEnglish { get; private set; }

        public string AdditionalData { get; private set; }
        #endregion

        #region C'tor
        public CreateMessageForUserGroupCmd(enMessagePrio messagePrio, string groupName, string textGerman, string textEnglish,
            string additionalData)
        {
            MessagePrio = messagePrio;
            GroupName = groupName;
            TextGerman = textGerman;
            TextEnglish = textEnglish;
            AdditionalData = additionalData;
        }
        #endregion
    }
}
