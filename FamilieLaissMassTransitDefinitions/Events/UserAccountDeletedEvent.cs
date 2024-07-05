using FamilieLaissMassTransitDefinitions.Contracts.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace FamilieLaissMassTransitDefinitions.Events
{
    public class UserAccountDeletedEvent : iUserAccountDeletedEvent
    {
        /// <summary>
        /// The identifier for the user
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="id">The identifier for the user</param>
        public UserAccountDeletedEvent(string id)
        {
            ID = id;
        }
    }
}
