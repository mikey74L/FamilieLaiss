using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FamilieLaissMassTransitDefinitions.Contracts.Events;
using FamilieLaissMassTransitDefinitions.Events;
using Microsoft.Extensions.Logging;
using ServiceHelper.Exceptions;

namespace VideoConvertExecuteService.Services;

public partial class VideoConverterService
{
    private async Task CopyConvertedToDestinationHls(long id, string filename360, string filename480,
        string filename720, string filename1080,
        string filename2160, string filenameStream, string filenameThumbnail, string filenameVtt)
    {
        var filename360Source = "";
        var filename360Target = "";
        var filename480Source = "";
        var filename480Target = "";
        var filename720Source = "";
        var filename720Target = "";
        var filename1080Source = "";
        var filename1080Target = "";
        var filename2160Source = "";
        var filename2160Target = "";

        if (!string.IsNullOrEmpty(filename360))
        {
            filename360Source = filename360;
            filename360Target =
                Path.Combine(appSettings.Value.DirectoryUploadVideo, Path.GetFileName(filename360Source));
        }

        if (!string.IsNullOrEmpty(filename480))
        {
            filename480Source = filename480;
            filename480Target =
                Path.Combine(appSettings.Value.DirectoryUploadVideo, Path.GetFileName(filename480Source));
        }

        if (!string.IsNullOrEmpty(filename720))
        {
            filename720Source = filename720;
            filename720Target =
                Path.Combine(appSettings.Value.DirectoryUploadVideo, Path.GetFileName(filename720Source));
        }

        if (!string.IsNullOrEmpty(filename1080))
        {
            filename1080Source = filename1080;
            filename1080Target = Path.Combine(appSettings.Value.DirectoryUploadVideo,
                Path.GetFileName(filename1080Source));
        }

        if (!string.IsNullOrEmpty(filename2160))
        {
            filename2160Source = filename1080;
            filename2160Target = Path.Combine(appSettings.Value.DirectoryUploadVideo,
                Path.GetFileName(filename2160Source) ?? string.Empty);
        }

        var filenameStreamSource = filenameStream;
        var filenameStreamTarget =
            Path.Combine(appSettings.Value.DirectoryUploadVideo, Path.GetFileName(filenameStreamSource));
        var filenameThumbnailSource = filenameThumbnail;
        var filenameThumbnailTarget = Path.Combine(appSettings.Value.DirectoryUploadPicture,
            Path.GetFileName(filenameThumbnailSource));
        var filenameVttSource = filenameVtt;
        var filenameVttTarget =
            Path.Combine(appSettings.Value.DirectoryUploadVideo, Path.GetFileName(filenameVttSource));
        var filenameSpriteSource = Path.Combine(Path.GetDirectoryName(filenameThumbnailSource),
            Path.GetFileNameWithoutExtension(filenameThumbnailSource) + "_Sprite.jpg");
        var filenameSpriteTarget = Path.Combine(appSettings.Value.DirectoryUploadPicture,
            Path.GetFileName(filenameSpriteSource));

        try
        {
            await databaseOperations.SetStatusCopyConvertedBeginAsync(id);

            var @event = new VideoConvertProgressEvent()
            {
                ConvertStatusId = _consumerContext.Message.ConvertStatusId,
                UploadVideoId = _consumerContext.Message.Id
            };
            await _consumerContext.Publish<IVideoConvertProgressEvent>(@event);

            if (!string.IsNullOrEmpty(filename360))
            {
                logger.LogInformation("Copy converted mp4 HLS file for 640 x 360");
                File.Move(filename360Source, filename360Target);
                logger.LogInformation(
                    $"File \"{Path.GetFileName(filename360Source)}\" successfully copied to target directory");

                var fileList = Directory.GetFiles(appSettings.Value.DirectoryConvertVideo,
                    Path.GetFileNameWithoutExtension(filename360) + "_*.ts");
                foreach (var fileItem in fileList)
                {
                    File.Move(fileItem,
                        Path.Combine(appSettings.Value.DirectoryUploadVideo, Path.GetFileName(fileItem)));
                }

                logger.LogInformation("Successfully copied ts files for 360p to target directory");
            }

            if (!string.IsNullOrEmpty(filename480))
            {
                logger.LogInformation("Copy converted mp4 HLS file for 842 x 480");
                File.Move(filename480Source, filename480Target);
                logger.LogInformation(
                    $"File \"{Path.GetFileName(filename480Source)}\" successfully copied to target directory");

                var fileList = Directory.GetFiles(appSettings.Value.DirectoryConvertVideo,
                    Path.GetFileNameWithoutExtension(filename480) + "_*.ts");
                foreach (var fileItem in fileList)
                {
                    File.Move(fileItem,
                        Path.Combine(appSettings.Value.DirectoryUploadVideo, Path.GetFileName(fileItem)));
                }

                logger.LogInformation("Successfully copied ts files for 480p to target directory");
            }

            if (!string.IsNullOrEmpty(filename720))
            {
                logger.LogInformation("Copy converted mp4 HLS file for 1280 x 720");
                File.Move(filename720Source, filename720Target);
                logger.LogInformation(
                    $"File \"{Path.GetFileName(filename720Source)}\" successfully copied to target directory");

                var fileList = Directory.GetFiles(appSettings.Value.DirectoryConvertVideo,
                    Path.GetFileNameWithoutExtension(filename720) + "_*.ts");
                foreach (var fileItem in fileList)
                {
                    File.Move(fileItem,
                        Path.Combine(appSettings.Value.DirectoryUploadVideo, Path.GetFileName(fileItem)));
                }

                logger.LogInformation("Successfully copied ts files for 720p to target directory");
            }

            if (!string.IsNullOrEmpty(filename1080))
            {
                logger.LogInformation("Copy converted mp4 HLS file for 1920 x 1080");
                File.Move(filename1080Source, filename1080Target);
                logger.LogInformation(
                    $"File \"{Path.GetFileName(filename1080Source)}\" successfully copied to target directory");

                var fileList = Directory.GetFiles(appSettings.Value.DirectoryConvertVideo,
                    Path.GetFileNameWithoutExtension(filename1080) + "_*.ts");
                foreach (var fileItem in fileList)
                {
                    File.Move(fileItem,
                        Path.Combine(appSettings.Value.DirectoryUploadVideo, Path.GetFileName(fileItem)));
                }

                logger.LogInformation("Successfully copied ts files for 1080p to target directory");
            }

            if (!string.IsNullOrEmpty(filename2160))
            {
                logger.LogInformation("Copy converted mp4 HLS file for 3860 x 2160");
                File.Move(filename2160Source, filename2160Target);
                logger.LogInformation(
                    $"File \"{Path.GetFileName(filename2160Source)}\" successfully copied to target directory");

                var fileList = Directory.GetFiles(appSettings.Value.DirectoryConvertVideo,
                    Path.GetFileNameWithoutExtension(filename2160) + "_*.ts");
                foreach (var fileItem in fileList)
                {
                    File.Move(fileItem,
                        Path.Combine(appSettings.Value.DirectoryUploadVideo, Path.GetFileName(fileItem)));
                }

                logger.LogInformation("Successfully copied ts files for 2160p to target directory");
            }

            logger.LogInformation("Copy stream file");
            File.Move(filenameStreamSource, filenameStreamTarget);
            logger.LogInformation(
                $"File \"{Path.GetFileName(filenameStreamSource)}\" successfully copied to target directory");

            logger.LogInformation("Copy thumbnail");
            File.Move(filenameThumbnailSource, filenameThumbnailTarget);
            logger.LogInformation(
                $"File \"{Path.GetFileName(filenameThumbnailSource)}\" successfully copied to target directory");

            logger.LogInformation("Copy vtt");
            File.Move(filenameVttSource, filenameVttTarget);
            logger.LogInformation(
                $"File \"{Path.GetFileName(filenameVttSource)}\" successfully copied to target directory");

            logger.LogInformation("Copy sprite image");
            File.Move(filenameSpriteSource, filenameSpriteTarget);
            logger.LogInformation(
                $"File \"{Path.GetFileName(filenameSpriteSource)}\" successfully copied to target directory");

            await databaseOperations.SetStatusCopyConvertedEndAsync(id);

            await _consumerContext.Publish<IVideoConvertProgressEvent>(@event);
        }
        catch (Exception ex)
        {
            throw new ServiceException("Error!!! Could not copy converted DASH files to target directory.", ex);
        }
    }

    private async Task CopyConvertedToDestination(long id, string filenameVideo, string filenameThumbnail,
        string filenameVtt)
    {
        var filenameVideoSource = filenameVideo;
        var filenameThumbnailSource = filenameThumbnail;
        var filenameVideoTarget =
            Path.Combine(appSettings.Value.DirectoryUploadVideo, Path.GetFileName(filenameVideoSource));
        var filenameThumbnailTarget = Path.Combine(appSettings.Value.DirectoryUploadPicture,
            Path.GetFileName(filenameThumbnailSource));
        var filenameVttSource = filenameVtt;
        var filenameVttTarget =
            Path.Combine(appSettings.Value.DirectoryUploadVideo, Path.GetFileName(filenameVttSource));
        var filenameSpriteSource = Path.Combine(Path.GetDirectoryName(filenameThumbnailSource) ?? string.Empty,
            Path.GetFileNameWithoutExtension(filenameThumbnailSource) + "_Sprite.jpg");
        var filenameSpriteTarget = Path.Combine(appSettings.Value.DirectoryUploadPicture,
            Path.GetFileName(filenameSpriteSource));

        try
        {
            await databaseOperations.SetStatusCopyConvertedBeginAsync(id);

            var @event = new VideoConvertProgressEvent()
            {
                ConvertStatusId = _consumerContext.Message.ConvertStatusId,
                UploadVideoId = _consumerContext.Message.Id
            };
            await _consumerContext.Publish<IVideoConvertProgressEvent>(@event);

            logger.LogInformation("Copy converted mp4 file");
            File.Move(filenameVideoSource, filenameVideoTarget);
            logger.LogInformation(
                $"File \"{Path.GetFileName(filenameVideoSource)}\" successfully copied to target directory");

            logger.LogInformation("Copy thumbnail");
            File.Move(filenameThumbnailSource, filenameThumbnailTarget);
            logger.LogInformation(
                $"File \"{Path.GetFileName(filenameThumbnailSource)}\" successfully copied to target directory");

            logger.LogInformation("Copy vtt");
            File.Move(filenameVttSource, filenameVttTarget);
            logger.LogInformation(
                $"File \"{Path.GetFileName(filenameVttSource)}\" successfully copied to target directory");

            logger.LogInformation("Copy sprite image");
            File.Move(filenameSpriteSource, filenameSpriteTarget);
            logger.LogInformation(
                $"File \"{Path.GetFileName(filenameSpriteSource)}\" successfully copied to target directory");

            logger.LogInformation("Delete temporary image files");
            var filteredFiles = Directory.EnumerateFiles(Path.GetDirectoryName(filenameVideoSource) ?? string.Empty)
                .Where(x => x.Contains("-Sprite-"))
                .ToList();
            foreach (var filename in filteredFiles)
            {
                File.Delete(filename);
            }

            logger.LogInformation("Temporary image files successfully deleted");

            await databaseOperations.SetStatusCopyConvertedEndAsync(id);

            await _consumerContext.Publish<IVideoConvertProgressEvent>(@event);
        }
        catch (Exception ex)
        {
            throw new ServiceException("Error!!! Could not copy converted files to target directory.", ex);
        }
    }
}