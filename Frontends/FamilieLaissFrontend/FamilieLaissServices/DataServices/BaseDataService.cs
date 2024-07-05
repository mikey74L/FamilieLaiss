using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.Enums;
using FamilieLaissServices.Models;
using StrawberryShake;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FamilieLaissServices.DataServices;

public abstract class BaseDataService(
    IFamilieLaissClient familieLaissClient)
{
    protected IFamilieLaissClient Client { get; } = familieLaissClient;

    private APIResultErrorType GetErrorType(IReadOnlyList<IClientError> errors)
    {
        var errorCode = errors.FirstOrDefault()?.Code ?? "NoErrorCode";

        return errorCode switch
        {
            "DuplicatedValue" => APIResultErrorType.Conflict,
            "NoDataFound" => APIResultErrorType.NotFound,
            "WrongParameter" => APIResultErrorType.BadRequest,
            "NotAuthorized" => APIResultErrorType.NotAuthorized,
            _ => APIResultErrorType.ServerError,
        };
    }

    protected ApiResult CreateSimpleApiResultForError(IReadOnlyList<IClientError> errors)
    {
        var result = new ApiResult()
        {
            ErrorType = GetErrorType(errors),
            IsSuccess = false,
        };

        return result;
    }

    protected ApiResult<TResult> CreateApiResultForError<TResult>(IReadOnlyList<IClientError> errors)
    {
        var result = new ApiResult<TResult>()
        {
            ErrorType = GetErrorType(errors),
            IsSuccess = false,
        };

        return result;
    }

    protected ApiResult CreateSimpleApiResultForCommunicationError(Exception ex)
    {
        var result = new ApiResult()
        {
            ErrorType = APIResultErrorType.CommunicationError,
            IsSuccess = false,
            Exception = ex
        };

        return result;
    }

    protected ApiResult<T> CreateApiResultForCommunicationError<T>(Exception ex)
    {
        var result = new ApiResult<T>()
        {
            ErrorType = APIResultErrorType.CommunicationError,
            IsSuccess = false,
            Exception = ex
        };

        return result;
    }

    protected ApiResult CreateSimpleApiResult()
    {
        var result = new ApiResult()
        {
            ErrorType = APIResultErrorType.NoError,
            IsSuccess = true
        };

        return result;
    }

    protected ApiResult<TResult> CreateApiResult<TResult>(TResult sourceModel)
    {
        var result = new ApiResult<TResult>()
        {
            ErrorType = APIResultErrorType.NoError,
            IsSuccess = true,
            ResultValue = sourceModel
        };

        return result;
    }

    protected ApiResult CreateSimpleApiResultForBadRequest()
    {
        var result = new ApiResult()
        {
            ErrorType = APIResultErrorType.BadRequest,
            IsSuccess = false
        };

        return result;
    }

    protected ApiResult<TResult> CreateApiResultForBadRequest<TResult>()
    {
        var result = new ApiResult<TResult>()
        {
            ErrorType = APIResultErrorType.BadRequest,
            IsSuccess = false
        };

        return result;
    }
}