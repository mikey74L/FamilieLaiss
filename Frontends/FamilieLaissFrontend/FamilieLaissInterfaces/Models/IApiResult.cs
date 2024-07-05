using FamilieLaissInterfaces.Enums;

namespace FamilieLaissInterfaces.Models;

public interface IApiResult
{
    public bool IsSuccess { get; set; }
    public APIResultErrorType ErrorType { get; set; }
}

public interface IApiResult<T> : IApiResult
{
    public T? ResultValue { get; set; }
}
