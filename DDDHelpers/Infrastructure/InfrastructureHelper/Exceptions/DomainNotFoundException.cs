using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace InfrastructureHelper.Exceptions
{
    public class DomainNotFoundException : Exception
    {
        public DomainNotFoundException()
        {
        }

        public DomainNotFoundException(string message) : base(message)
        {
        }

        public DomainNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DomainNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
