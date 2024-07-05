using System;
using System.Collections.Generic;
using System.Text;

namespace FamilieLaissMassTransitDefinitions.Contracts.Events
{
    /// <summary>
    /// User account confirmed changed event (Event-Class for MassTransit)
    /// </summary>
    public interface iUserAccountConfirmedChangedEvent
    {
        /// <summary>
        /// The user account name (is unique)
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// Is eMail for user confirmed?
        /// </summary>
        bool EmailConfirmed { get; }
    }
}
