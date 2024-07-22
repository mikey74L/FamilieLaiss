using System;
using System.Threading.Tasks;

namespace VideoConvertExecuteService.Interfaces;

public interface IDatabaseOperations
{
    Task SetStatusReadMediaInfoBeginAsync(long id);
    Task SetStatusReadMediaInfoEndAsync(long id);

    Task SetStatusConvertMp4BeginAsync(long id);
    Task SetStatusConvertMp4EndAsync(long id);

    Task SetStatusConvert640X360BeginAsync(long id);
    Task SetStatusConvert640X360EndAsync(long id);

    Task SetStatusConvert852X480BeginAsync(long id);
    Task SetStatusConvert852X480EndAsync(long id);

    Task SetStatusConvert1280X720BeginAsync(long id);
    Task SetStatusConvert1280X720EndAsync(long id);

    Task SetStatusConvert1920X1080BeginAsync(long id);
    Task SetStatusConvert1920X1080EndAsync(long id);

    Task SetStatusConvert3840X2160BeginAsync(long id);
    Task SetStatusConvert3840X2160EndAsync(long id);

    Task SetStatusCreateThumbnailBeginAsync(long id);
    Task SetStatusCreateThumbnailEndAsync(long id);

    Task SetStatusCreateVttBeginAsync(long id);
    Task SetStatusCreateVttEndAsync(long id);

    Task SetStatusCreateHlsBeginAsync(long id);
    Task SetStatusCreateHlsEndAsync(long id);

    Task SetStatusCopyConvertedBeginAsync(long id);
    Task SetStatusCopyConvertedEndAsync(long id);

    Task SetStatusDeleteOriginalBeginAsync(long id);
    Task SetStatusDeleteOriginalEndAsync(long id);

    Task SetStatusDeleteConvertedBeginAsync(long id);
    Task SetStatusDeleteConvertedEndAsync(long id);

    Task SetStatusConvertPictureBeginAsync(long id);
    Task SetStatusConvertPictureEndAsync(long id);

    Task UpdateProgressAsync(long id, int progressCurrent, TimeSpan currentTime, TimeSpan restTime);

    Task SetSuccessAsync(long id);

    Task SetTransientErrorAsync(long id, string errorMessage);

    Task SetErrorAsync(long id, string errorMessage);
}