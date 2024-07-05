using System;
using System.Runtime.Serialization;

namespace ServiceHelper.Exceptions
{
    public class PermanentServiceException : Exception
    {
        public PermanentServiceException()
        {
        }

        public PermanentServiceException(string message) : base(message)
        {
        }

        public PermanentServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PermanentServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
