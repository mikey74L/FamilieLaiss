using System;
using System.Collections.Generic;
using System.Text;

namespace FamilieLaissSharedObjects.Enums
{
    /// <summary>
    /// Setting the message category for a message
    /// </summary>
    public enum enMessagePrio : byte
    {
        /// <summary>
        /// Message belongs to category "Errors"
        /// </summary>
        Error = 1,
        /// <summary>
        /// Message belongs to category "Warnings"
        /// </summary>
        Warning = 2,
        /// <summary>
        /// Message belongs to category "Information"
        /// </summary>
        Info = 3
    }
}
