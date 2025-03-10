using FamilieLaissInterfaces.Enums;
using FamilieLaissInterfaces.Models;
using System;
using System.Threading.Tasks;

namespace FamilieLaissServices.Extensions;

public static class IApiResultExtensions
{
    public static async Task<IApiResult> HandleStatus(this Task<IApiResult> apiResult, APIResultErrorType errorType,
        Func<Task> action)
    {
        var apiResultAwaited = await apiResult;

        if (apiResultAwaited.ErrorType == errorType)
        {
            await action();
        }

        return apiResultAwaited;
    }

    public static async Task<IApiResult<T>> HandleStatus<T>(this Task<IApiResult<T>> apiResult,
        APIResultErrorType errorType, Func<T?, Task> action)
    {
        var apiResultAwaited = await apiResult;

        if (apiResultAwaited.ErrorType == errorType)
        {
            await action(apiResultAwaited.ResultValue);
        }

        return apiResultAwaited;
    }

    public static async Task<IApiResult<T>> HandleErrors<T>(this Task<IApiResult<T>> apiResult, Func<T?, Task> action)
    {
        var apiResultAwaited = await apiResult;

        if (apiResultAwaited.ErrorType != APIResultErrorType.NoError)
        {
            await action(apiResultAwaited.ResultValue);
        }

        return apiResultAwaited;
    }

    public static async Task<IApiResult> HandleErrors(this Task<IApiResult> apiResult, Func<Task> action)
    {
        var apiResultAwaited = await apiResult;

        if (apiResultAwaited.ErrorType != APIResultErrorType.NoError)
        {
            await action();
        }

        return apiResultAwaited;
    }

    public static async Task<IApiResult<T>> HandleSuccess<T>(this Task<IApiResult<T>> apiResult, Func<T?, Task> action)
    {
        var apiResultAwaited = await apiResult;

        if (apiResultAwaited.ErrorType == APIResultErrorType.NoError)
        {
            await action(apiResultAwaited.ResultValue);
        }

        return apiResultAwaited;
    }

    public static async Task<IApiResult> HandleSuccess(this Task<IApiResult> apiResult, Func<Task> action)
    {
        var apiResultAwaited = await apiResult;

        if (apiResultAwaited.ErrorType == APIResultErrorType.NoError)
        {
            await action();
        }

        return apiResultAwaited;
    }
}