using System;
using System.Collections.Generic;
using System.Text;

namespace FamilieLaissMassTransitDefinitions.Contracts.Events
{
    /// <summary>
    /// User user group changed event (Event-Class for MassTransit)
    /// </summary>
    public interface iUserGroupChangedEvent
    {
        /// <summary>
        /// The user account name (is unique)
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// User group
        /// </summary>
        string UserGroup { get; }
    }
}
