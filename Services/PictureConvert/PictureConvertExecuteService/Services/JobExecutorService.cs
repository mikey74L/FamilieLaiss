using System.Threading.Tasks;
using FamilieLaissMassTransitDefinitions.Contracts.Commands;
using FamilieLaissMassTransitDefinitions.Contracts.Events;
using FamilieLaissMassTransitDefinitions.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PictureConvertExecuteService.Interfaces;
using PictureConvertExecuteService.Models;

namespace PictureConvertExecuteService.Services;

public class JobExecutorService(
    ILogger<JobExecutorService> logger,
    IOptions<AppSettings> appSettings,
    IPictureInfoExtractor pictureInfoExtractor,
    IMetaExtractor metaExtractor,
    IConvertPicture convertPicture,
    IDatabaseOperations databaseOperations) : IJobExecutor
{
    #region Private Methods

    private string GetFullFilename(long id, string originalName)
    {
        string filename = $"{id}{System.IO.Path.GetExtension(originalName)}";

        string returnValue = System.IO.Path.Combine(appSettings.Value.DirectoryUploadPicture, filename);

        return returnValue;
    }

    #endregion

    #region Interface Implementation

    public async Task ExecuteJobAsync(ConsumeContext<IConvertPictureCmd> consumerContext)
    {
        logger.LogInformation("Get filename for picture file");

        string filename = GetFullFilename(consumerContext.Message.Id, consumerContext.Message.OriginalName);
        logger.LogDebug($"Filename: {filename}");

        logger.LogInformation("Extracting Picture-Info");
        await pictureInfoExtractor.ExtractInfoAsync(consumerContext, filename);

        logger.LogInformation("Send PictureConvertProgressEvent with bus");
        var @event = new PictureConvertProgressEvent()
        {
            ConvertStatusId = consumerContext.Message.ConvertStatusId,
            UploadPictureId = consumerContext.Message.Id
        };
        await consumerContext.Publish<IPictureConvertProgressEvent>(@event);

        logger.LogInformation("Extracting Exif-Info");
        await metaExtractor.ExtractMetadataAsync(consumerContext, filename);

        logger.LogInformation("Send PictureConvertProgressEvent with bus");
        await consumerContext.Publish<IPictureConvertProgressEvent>(@event);

        logger.LogInformation("Converting Picture");
        await convertPicture.ConvertPictureAsync(consumerContext, filename);

        logger.LogInformation("Send PictureConvertProgressEvent with bus");
        await consumerContext.Publish<IPictureConvertProgressEvent>(@event);

        logger.LogInformation("Set status to successfully converted");
        await databaseOperations.SetSuccessAsync(consumerContext.Message.ConvertStatusId);

        logger.LogInformation("Send PictureConvertedEvent with bus");
        var eventConverted = new PictureConvertedEvent()
        {
            ConvertStatusId = consumerContext.Message.ConvertStatusId,
            UploadPictureId = consumerContext.Message.Id
        };
        await consumerContext.Publish<IPictureConvertedEvent>(eventConverted);

        logger.LogInformation($"Conversion for file \"{filename}\" successfully completed");
    }

    #endregion
}