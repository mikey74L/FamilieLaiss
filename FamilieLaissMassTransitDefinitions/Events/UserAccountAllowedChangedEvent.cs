using FamilieLaissMassTransitDefinitions.Contracts.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace FamilieLaissMassTransitDefinitions.Events
{
    public class UserAccountAllowedChangedEvent : iUserAccountAllowedChangedEvent
    {
        #region Properties
        /// <summary>
        /// The user account name (is unique)
        /// </summary>
        public string UserName { get; private set; }
      
        /// <summary>
        /// Is access to website allowed?
        /// </summary>
        public bool Allowed { get; private set; }
        #endregion

        #region C'tor
        /// <param name="userName">The user name</param>
        /// <param name="allowed">Is eMail for user confirmed</param>
        public UserAccountAllowedChangedEvent(string userName, bool allowed)
        {
            //Übernehmen der Daten
            UserName = userName;
            Allowed = allowed;
        }
        #endregion
    }
}
