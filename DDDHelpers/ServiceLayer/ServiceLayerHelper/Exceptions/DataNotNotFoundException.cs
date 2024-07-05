using System;

namespace ServiceLayerHelper.Exceptions
{
    public class DataItemNotNotFoundException : Exception
    {
        public DataItemNotNotFoundException(string message) : base(message) { }
    }
}
