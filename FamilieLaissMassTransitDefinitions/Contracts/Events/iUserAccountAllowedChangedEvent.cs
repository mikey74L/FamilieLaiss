using System;
using System.Collections.Generic;
using System.Text;

namespace FamilieLaissMassTransitDefinitions.Contracts.Events
{
    /// <summary>
    /// User account allowed changed event (Event-Class for MassTransit)
    /// </summary>
    public interface iUserAccountAllowedChangedEvent
    {
        /// <summary>
        /// The user account name (is unique)
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// Is access to website allowed?
        /// </summary>
        bool Allowed { get; }
    }
}
