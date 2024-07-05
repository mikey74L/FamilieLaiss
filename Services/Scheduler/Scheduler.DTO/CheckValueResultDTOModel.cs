using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler.DTO
{
    /// <summary>
    /// Class for transfering the result for a validation check from backend to client
    /// </summary>
    public class CheckValueResultDTOModel
    {
        /// <summary>
        /// The result for the validation check
        /// </summary>
        public bool Result { get; set; }
    }
}
