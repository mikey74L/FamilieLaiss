using FamilieLaissMassTransitDefinitions.Contracts.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace FamilieLaissMassTransitDefinitions.Events
{
    public class UserAccountConfirmedChangedEvent : iUserAccountConfirmedChangedEvent
    {
        #region Properties
        /// <summary>
        /// The user account name (is unique)
        /// </summary>
        public string UserName { get; private set; }
      
        /// <summary>
        /// Is eMail for user confirmed?
        /// </summary>
        public bool EmailConfirmed { get; private set; }
        #endregion

        #region C'tor
        /// <param name="userName">The user name</param>
        /// <param name="eMailConfirmed">Is eMail for user confirmed</param>
        public UserAccountConfirmedChangedEvent(string userName, bool eMailConfirmed)
        {
            //Übernehmen der Daten
            UserName = userName;
            EmailConfirmed = eMailConfirmed;
        }
        #endregion
    }
}
