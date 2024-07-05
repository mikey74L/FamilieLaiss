using Microsoft.Extensions.Logging;
using ServiceLayerHelper.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace Scheduler.API.Logging
{
    /// <summary>
    /// Class for Implementing general logging for global Exception-Handling
    /// </summary>
    public class LogSerilog: ILog
    {
        /// <summary>
        /// C*tor
        /// </summary>
        public LogSerilog()
        {
        }

        /// <summary>
        /// Logging Message for level information
        /// </summary>
        /// <param name="message"></param>
        public void Information(string message)
        {
            Log.Information(message);
        }

        /// <summary>
        /// Logging message for level warning
        /// </summary>
        /// <param name="message"></param>
        public void Warning(string message)
        {
            Log.Warning(message);
        }

        /// <summary>
        /// Logging message for level debug
        /// </summary>
        /// <param name="message"></param>
        public void Debug(string message)
        {
            Log.Debug(message);
        }

        /// <summary>
        /// Logging message for level error
        /// </summary>
        /// <param name="message"></param>
        public void Error(string message)
        {
            Log.Error(message);
        }
    }
}
