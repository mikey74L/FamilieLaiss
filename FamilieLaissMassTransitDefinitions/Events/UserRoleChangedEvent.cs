using FamilieLaissMassTransitDefinitions.Contracts.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace FamilieLaissMassTransitDefinitions.Events
{
    public class UserRoleChangedEvent : iUserRoleChangedEvent
    {
        #region Properties
        /// <summary>
        /// The user account name (is unique)
        /// </summary>
        public string UserName { get; private set; }
      
        /// <summary>
        /// User role name
        /// </summary>
        public string UserRole { get; private set; }
        #endregion

        #region C'tor
        /// <param name="userName">The user name</param>
        /// <param name="userRole">User role name</param>
        public UserRoleChangedEvent(string userName, string userRole)
        {
            //Übernehmen der Daten
            UserName = userName;
            UserRole = userRole;
        }
        #endregion
    }
}
