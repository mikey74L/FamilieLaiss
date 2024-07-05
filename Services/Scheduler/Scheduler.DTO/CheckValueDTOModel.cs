using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler.DTO
{
    /// <summary>
    /// Class for transferring the values to be checked from client to backend
    /// </summary>
    public class CheckValueDTOModel
    {
        /// <summary>
        /// The ID for a item to be checked in an update scenario
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// Additional information
        /// </summary>
        public long AdditionalType { get; set; }

        /// <summary>
        /// The value to be checked
        /// </summary>
        public string Value { get; set; }
    }
}
