using FamilieLaissInterfaces.Enums;
using FamilieLaissInterfaces.Models;
using System;

namespace FamilieLaissServices.Models;

public class ApiResult : IApiResult
{
    public bool IsSuccess { get; set; }

    public APIResultErrorType ErrorType { get; set; }

    public Exception? Exception { get; set; }
}

public class ApiResult<T> : ApiResult, IApiResult<T>
{
    public T? ResultValue { get; set; }
}
