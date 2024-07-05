using System;
using System.Collections.Generic;
using System.Text;

namespace InfrastructureHelper.Exceptions
{
    /// <summary>
    /// Exception when a duplicated key was violated
    /// </summary>
    public class DataDuplicatedValueException : Exception
    {
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="message">The exception message</param>
        public DataDuplicatedValueException(string message) : base(message)
        {

        }
    }
}
