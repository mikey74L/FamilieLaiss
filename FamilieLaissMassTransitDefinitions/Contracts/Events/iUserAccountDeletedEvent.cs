using System;
using System.Collections.Generic;
using System.Text;

namespace FamilieLaissMassTransitDefinitions.Contracts.Events
{
    /// <summary>
    /// User account deleted event (Event-Class for MassTransit)
    /// </summary>
    public interface iUserAccountDeletedEvent
    {
        /// <summary>
        /// The identifier for the user
        /// </summary>
        string ID { get; }
    }
}
