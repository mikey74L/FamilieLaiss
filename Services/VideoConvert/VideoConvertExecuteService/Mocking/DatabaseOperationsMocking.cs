using System;
using System.Threading.Tasks;
using FamilieLaissSharedObjects.Enums;
using VideoConvertExecuteService.Interfaces;

namespace VideoConvertExecuteService.Mocking;

public class DatabaseOperationsMocking : IDatabaseOperations
{
    public Task SetErrorAsync(long id, string errorMessage)
    {
        return Task.CompletedTask;
    }

    public Task SetStatusConvert1280X720BeginAsync(long id)
    {
        return Task.CompletedTask;
    }

    public Task SetStatusConvert1280X720EndAsync(long id)
    {
        return Task.CompletedTask;
    }

    public Task SetStatusConvert3840X2160BeginAsync(long id)
    {
        return Task.CompletedTask;
    }

    public Task SetStatusConvert3840X2160EndAsync(long id)
    {
        return Task.CompletedTask;
    }

    public Task SetStatusConvert1920X1080BeginAsync(long id)
    {
        return Task.CompletedTask;
    }

    public Task SetStatusConvert1920X1080EndAsync(long id)
    {
        return Task.CompletedTask;
    }

    public Task SetStatusConvert640X360BeginAsync(long id)
    {
        return Task.CompletedTask;
    }

    public Task SetStatusConvert640X360EndAsync(long id)
    {
        return Task.CompletedTask;
    }

    public Task SetStatusConvert852X480BeginAsync(long id)
    {
        return Task.CompletedTask;
    }

    public Task SetStatusConvert852X480EndAsync(long id)
    {
        return Task.CompletedTask;
    }

    public Task SetStatusConvertMp4BeginAsync(long id)
    {
        return Task.CompletedTask;
    }

    public Task SetStatusConvertMp4EndAsync(long id)
    {
        return Task.CompletedTask;
    }

    public Task SetStatusConvertPictureBeginAsync(long id)
    {
        return Task.CompletedTask;
    }

    public Task SetStatusConvertPictureEndAsync(long id)
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task SetVideoInfoData(long id, EnumVideoType videoType, int height, int width, int hours, int minutes,
        int seconds,
        double? longitude, double? latitude)
    {
        return Task.CompletedTask;
    }

    public Task SetStatusCopyConvertedBeginAsync(long id)
    {
        return Task.CompletedTask;
    }

    public Task SetStatusCopyConvertedEndAsync(long id)
    {
        return Task.CompletedTask;
    }

    public Task SetStatusCreateHlsBeginAsync(long id)
    {
        return Task.CompletedTask;
    }

    public Task SetStatusCreateHlsEndAsync(long id)
    {
        return Task.CompletedTask;
    }

    public Task SetStatusCreateThumbnailBeginAsync(long id)
    {
        return Task.CompletedTask;
    }

    public Task SetStatusCreateThumbnailEndAsync(long id)
    {
        return Task.CompletedTask;
    }

    public Task SetStatusCreateVttBeginAsync(long id)
    {
        return Task.CompletedTask;
    }

    public Task SetStatusCreateVttEndAsync(long id)
    {
        return Task.CompletedTask;
    }

    public Task SetStatusDeleteConvertedBeginAsync(long id)
    {
        return Task.CompletedTask;
    }

    public Task SetStatusDeleteConvertedEndAsync(long id)
    {
        return Task.CompletedTask;
    }

    public Task SetStatusDeleteOriginalBeginAsync(long id)
    {
        return Task.CompletedTask;
    }

    public Task SetStatusDeleteOriginalEndAsync(long id)
    {
        return Task.CompletedTask;
    }

    public Task SetStatusReadMediaInfoBeginAsync(long id)
    {
        return Task.CompletedTask;
    }

    public Task SetStatusReadMediaInfoEndAsync(long id)
    {
        return Task.CompletedTask;
    }

    public Task SetSuccessAsync(long id)
    {
        return Task.CompletedTask;
    }

    public Task SetTransientErrorAsync(long id, string errorMessage)
    {
        return Task.CompletedTask;
    }

    public Task UpdateProgressAsync(long id, int progressCurrent, TimeSpan currentTime, TimeSpan restTime)
    {
        return Task.CompletedTask;
    }
}