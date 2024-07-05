using PictureConvertExecuteService.Interfaces;
using System;
using System.Threading.Tasks;

namespace PictureConvertExecuteService.Mocking;

public class DatabaseOperationsMocking : IDatabaseOperations
{
    public Task SetErrorAsync(long id, string errorMessage)
    {
        return Task.CompletedTask;
    }

    public Task SetStatusConvertBeginAsync(long id)
    {
        return Task.CompletedTask;
    }

    public Task SetStatusConvertEndAsync(long id)
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task SetSizeForPicture(long id, int width, int height)
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task SetExifInfoForPicture(long id, string? make, string? model, double? resolutionX, double? resolutionY,
        short? resolutionUnit, short? orientation, DateTimeOffset? ddlRecorded, double? exposureTime,
        short? exposureProgram, short? exposureMode, double? fNumber, int? isoSensitivity, double? shutterSpeed,
        short? meteringMode, short? flashMode, double? focalLength, short? sensingMode, short? whiteBalanceMode,
        short? sharpness, double? gpsLongitude, double? gpsLatitude, short? contrast, short? saturation)
    {
        return Task.CompletedTask;
    }

    public Task SetStatusReadExifBeginAsync(long id)
    {
        return Task.CompletedTask;
    }

    public Task SetStatusReadExifEndAsync(long id)
    {
        return Task.CompletedTask;
    }

    public Task SetStatusReadInfoBeginAsync(long id)
    {
        return Task.CompletedTask;
    }

    public Task SetStatusReadInfoEndAsync(long id)
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
}