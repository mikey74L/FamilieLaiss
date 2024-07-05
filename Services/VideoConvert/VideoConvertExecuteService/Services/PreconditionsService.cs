using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using ServiceHelper.Exceptions;
using ServiceHelper.Interfaces;
using VideoConvertExecuteService.Models;
using Xabe.FFmpeg.Downloader;

namespace VideoConvertExecuteService.Services;

public class PreconditionsService(IOptions<AppSettings> appSettings)
    : IPreconditions
{
    #region Interface IPreconditions

    public async Task DownloadFfmpeg()
    {
        await FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official);
    }

    public void CheckPreconditions()
    {
        if (string.IsNullOrEmpty(appSettings.Value.RabbitMqConnection))
        {
            throw new ServiceException("Settings Error!!!: URL for RabbitMQ is not well formed");
        }

        if (string.IsNullOrEmpty(appSettings.Value.PostgresHost))
        {
            throw new ServiceException("Settings Error!!!: Database server not set");
        }

        if (string.IsNullOrEmpty(appSettings.Value.PostgresDatabase))
        {
            throw new ServiceException("Settings Error!!!: Database not set");
        }

        if (appSettings.Value.PostgresPort == 0)
        {
            throw new ServiceException("Settings Error!!!: Database port not set");
        }

        if (string.IsNullOrEmpty(appSettings.Value.PostgresUser))
        {
            throw new ServiceException("Settings Error!!!: Database user not set");
        }

        if (string.IsNullOrEmpty(appSettings.Value.PostgresPassword_FILE))
        {
            throw new ServiceException("Settings Error!!!: Database password not set");
        }

        if (string.IsNullOrEmpty(appSettings.Value.EndpointNameExecutor))
        {
            throw new ServiceException("Settings Error!!!: Endpoint name for executor not set");
        }

        if (string.IsNullOrEmpty(appSettings.Value.DirectoryUploadVideo))
        {
            throw new ServiceException("Settings Error!!!: Folder for upload video not set");
        }

        if (string.IsNullOrEmpty(appSettings.Value.DirectoryConvertVideo))
        {
            throw new ServiceException("Settings Error!!!: Folder for video conversion not set");
        }

        if (string.IsNullOrEmpty(appSettings.Value.DirectoryUploadPicture))
        {
            throw new ServiceException("Settings Error!!!: Folder for upload picture not set");
        }
    }

    #endregion
}