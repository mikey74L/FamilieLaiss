using System;
using System.Threading.Tasks;

namespace PictureConvertExecuteService.Interfaces;

public interface IDatabaseOperations
{
    Task SetStatusReadInfoBeginAsync(long id);

    Task SetStatusReadInfoEndAsync(long id);

    Task SetStatusReadExifBeginAsync(long id);

    Task SetStatusReadExifEndAsync(long id);

    Task SetStatusConvertBeginAsync(long id);

    Task SetStatusConvertEndAsync(long id);

    Task SetSizeForPicture(long id, int width, int height);

    Task SetExifInfoForPicture(long id, string? make, string? model, double? resolutionX, double? resolutionY,
        short? resolutionUnit, short? orientation, DateTimeOffset? ddlRecorded, double? exposureTime,
        short? exposureProgram, short? exposureMode, double? fNumber,
        int? isoSensitivity, double? shutterSpeed, short? meteringMode, short? flashMode, double? focalLength,
        short? sensingMode, short? whiteBalanceMode, short? sharpness, double? gpsLongitude, double? gpsLatitude,
        short? contrast, short? saturation);

    Task SetSuccessAsync(long id);

    Task SetTransientErrorAsync(long id, string errorMessage);

    Task SetErrorAsync(long id, string errorMessage);
}