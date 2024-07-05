using System;

namespace DomainHelper.Exceptions;

public enum DomainExceptionType
{
    Undefined,
    WrongParameter,
    NoDataFound
}

public class DomainException : Exception
{
    public DomainExceptionType ExceptionType { get; }

    public DomainException(DomainExceptionType type)
    {
        ExceptionType = type;
    }

    public DomainException(DomainExceptionType type, string message) : base(message)
    {
        ExceptionType = type;
    }

    public DomainException(DomainExceptionType type, string message, Exception innerException) : base(message, innerException)
    {
        ExceptionType = type;
    }

    public DomainException()
    {
        ExceptionType = DomainExceptionType.Undefined;
    }

    public DomainException(string message) : base(message)
    {
        ExceptionType = DomainExceptionType.Undefined;
    }

    public DomainException(string message, Exception innerException) : base(message, innerException)
    {
        ExceptionType = DomainExceptionType.Undefined;
    }
}
