using FamilieLaissMassTransitDefinitions.Contracts.Commands;
using FamilieLaissSharedObjects.Enums;

namespace FamilieLaissMassTransitDefinitions.Commands
{
    public class CreateMessageForUserCmd : iCreateMessageForUserCmd
    {
        #region Properties
        public enMessagePrio MessagePrio { get; private set; }

        public string UserName { get; private set; }

        public string TextGerman { get; private set; }

        public string TextEnglish { get; private set; }

        public string AdditionalData { get; private set; }
        #endregion

        #region C'tor
        public CreateMessageForUserCmd(enMessagePrio messagePrio, string userName, string textGerman, string textEnglish,
            string additionalData)
        {
            MessagePrio = messagePrio;
            UserName = userName;
            TextGerman = textGerman;
            TextEnglish = textEnglish;
            AdditionalData = additionalData;
        }
        #endregion
    }
}
