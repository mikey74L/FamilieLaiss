using AutoMapper;
using FamilieLaissInterfaces.Enums;
using FamilieLaissInterfaces.Models;
using FamilieLaissServices.Models;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace FamilieLaissServices.Extensions;

internal static class HttpResponseMessageExtension
{
    private static APIResultErrorType GetErrorType(HttpResponseMessage httpResponseMessage)
    {
        return httpResponseMessage.StatusCode switch
        {
            HttpStatusCode.OK => APIResultErrorType.NoError,
            HttpStatusCode.Created => APIResultErrorType.NoError,
            HttpStatusCode.NotFound => APIResultErrorType.NotFound,
            HttpStatusCode.Conflict => APIResultErrorType.Conflict,
            HttpStatusCode.BadRequest => APIResultErrorType.BadRequest,
            HttpStatusCode.Unauthorized => APIResultErrorType.NotAuthorized,
            _ => APIResultErrorType.ServerError
        };
    }

    public static IApiResult AsApiResult(this HttpResponseMessage httpResponseMessage)
    {
        var result = new ApiResult()
        {
            ErrorType = GetErrorType(httpResponseMessage),
        };

        result.IsSuccess = result.ErrorType == APIResultErrorType.NoError;

        return result;
    }

    public static async Task<IApiResult> AsApiResultAsync(this Task<HttpResponseMessage> httpResponseMessage)
    {
        var result = new ApiResult()
        {
            ErrorType = GetErrorType(await httpResponseMessage),
        };

        result.IsSuccess = result.ErrorType == APIResultErrorType.NoError;

        return result;
    }

    public static async Task<IApiResult<T>> AsApiResultSimpleAsync<T>(this HttpResponseMessage httpResponseMessage)
    {
        var result = new ApiResult<T>()
        {
            ErrorType = GetErrorType(httpResponseMessage),
        };

        result.IsSuccess = result.ErrorType == APIResultErrorType.NoError;

        var content = await httpResponseMessage.Content.ReadAsStringAsync();
        result.ResultValue = JsonSerializer.Deserialize<T>(content);

        return result;
    }

    public static async Task<IApiResult<T>> AsApiResultSimpleAsync<T>(
        this Task<HttpResponseMessage> httpResponseMessage)
    {
        var responseMessage = await httpResponseMessage;

        return await responseMessage.AsApiResultSimpleAsync<T>();
    }

    public static async Task<IApiResult<TResult>> AsApiResultAsync<TSource, TResult>(
        this HttpResponseMessage httpResponseMessage, IMapper mapper,
        JsonSerializerOptions? jsonSerializerOptions = null) where TResult : class
    {
        var result = new ApiResult<TResult>()
        {
            ErrorType = GetErrorType(httpResponseMessage),
        };

        result.IsSuccess = result.ErrorType == APIResultErrorType.NoError;

        if (result.IsSuccess)
        {
            var content = await httpResponseMessage.Content.ReadAsStringAsync();

            var sourceValue = JsonSerializer.Deserialize<TSource>(content, jsonSerializerOptions);

            result.ResultValue = mapper.Map<TResult>(sourceValue);
        }

        return result;
    }

    public static async Task<IApiResult<TResult>> AsApiResultAsync<TSource, TResult>(
        this Task<HttpResponseMessage> httpResponseMessage, IMapper mapper,
        JsonSerializerOptions? jsonSerializerOptions = null) where TResult : class
    {
        var responseMessage = await httpResponseMessage;

        return await responseMessage.AsApiResultAsync<TSource, TResult>(mapper, jsonSerializerOptions);
    }

    public static async Task<IApiResult> AsApiResultAsync<TSource, TResult>(
        this HttpResponseMessage httpResponseMessage, TResult modelToUpdate, IMapper mapper,
        JsonSerializerOptions? jsonSerializerOptions = null) where TResult : class
    {
        var result = new ApiResult()
        {
            ErrorType = GetErrorType(httpResponseMessage),
        };

        result.IsSuccess = result.ErrorType == APIResultErrorType.NoError;

        if (result.IsSuccess)
        {
            string content = await httpResponseMessage.Content.ReadAsStringAsync();

            TSource? sourceValue = JsonSerializer.Deserialize<TSource>(content, jsonSerializerOptions);

            mapper.Map(sourceValue, modelToUpdate);
        }

        return result;
    }

    public static async Task<IApiResult> AsApiResultAsync<TSource, TResult>(
        this Task<HttpResponseMessage> httpResponseMessage, TResult modelToUpdate, IMapper mapper,
        JsonSerializerOptions? jsonSerializerOptions = null) where TResult : class
    {
        var responseMessage = await httpResponseMessage;

        return await responseMessage.AsApiResultAsync<TSource, TResult>(modelToUpdate, mapper, jsonSerializerOptions);
    }
}