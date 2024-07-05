using System;
using System.Collections.Generic;
using System.Text;

namespace FamilieLaissMassTransitDefinitions.Contracts.Events
{
    /// <summary>
    /// User role changed event (Event-Class for MassTransit)
    /// </summary>
    public interface iUserRoleChangedEvent
    {
        /// <summary>
        /// The user account name (is unique)
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// User role name
        /// </summary>
        string UserRole { get; }
    }
}
