using FamilieLaissMassTransitDefinitions.Contracts.Commands;
using FamilieLaissMassTransitDefinitions.Contracts.Events;
using FamilieLaissMassTransitDefinitions.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using VideoConvertExecuteService.Interfaces;
using VideoConvertExecuteService.Models;

namespace VideoConvertExecuteService.Services;

public class JobExecutorService(
    ILogger<JobExecutorService> logger,
    IOptions<AppSettings> appSettings,
    IMetadataExtractor metadataExtractor,
    IVideoConverter videoConverter,
    IConvertPicture pictureConverter,
    IDatabaseOperations databaseOperations)
    : IJobExecutor
{
    #region Private Methods

    private string GetFullFilename(long id, string originalName)
    {
        var filename = $"{id}{System.IO.Path.GetExtension(originalName)}";

        var returnValue = System.IO.Path.Combine(appSettings.Value.DirectoryUploadVideo, filename);

        return returnValue;
    }

    #endregion

    #region Interface iJobExecuter

    public async Task ExecuteJob(ConsumeContext<IMassConvertVideoCmd> consumerContext)
    {
        logger.LogInformation("Get filename for picture file");
        var sourceFilename = GetFullFilename(consumerContext.Message.Id, consumerContext.Message.OriginalName);
        logger.LogDebug($"Filename: {sourceFilename}");

        logger.LogInformation("Extracting media info from file");
        var mediaInfo = await metadataExtractor.ExtractMetadata(consumerContext, sourceFilename);

        logger.LogInformation("Converting video");
        await videoConverter.ConvertVideo(consumerContext, sourceFilename, mediaInfo);

        logger.LogInformation("Converting preview image to needed formats");
        await pictureConverter.ConvertPicture(consumerContext.Message.ConvertStatusId, sourceFilename, consumerContext);

        logger.LogInformation("Set status to successfully converted");
        await databaseOperations.SetSuccessAsync(consumerContext.Message.ConvertStatusId);

        logger.LogInformation("Send PictureConvertedEvent with bus");
        var publishEvent = new MassVideoConvertedEvent()
        {
            ConvertStatusId = consumerContext.Message.ConvertStatusId,
            UploadVideoId = consumerContext.Message.Id
        };
        await consumerContext.Publish<IMassVideoConvertedEvent>(publishEvent);

        logger.LogInformation($"Conversion for file \"{sourceFilename}\" successfully completed");
    }

    #endregion
}