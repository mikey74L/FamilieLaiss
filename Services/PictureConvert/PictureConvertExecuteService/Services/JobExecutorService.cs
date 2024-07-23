using FamilieLaissMassTransitDefinitions.Contracts.Commands.UploadPicture;
using FamilieLaissMassTransitDefinitions.Contracts.Events.UploadPicture;
using FamilieLaissMassTransitDefinitions.Events.UploadPicture;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PictureConvertExecuteService.Interfaces;
using PictureConvertExecuteService.Models;
using System.Threading.Tasks;

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

    public async Task ExecuteJobAsync(ConsumeContext<IMassConvertPictureCmd> consumerContext)
    {
        logger.LogInformation("Get filename for picture file");

        string filename = GetFullFilename(consumerContext.Message.Id, consumerContext.Message.OriginalName);
        logger.LogDebug($"Filename: {filename}");

        logger.LogInformation("Extracting Picture-Info");
        await pictureInfoExtractor.ExtractInfoAsync(consumerContext, filename);

        logger.LogInformation("Extracting Exif-Info");
        await metaExtractor.ExtractMetadataAsync(consumerContext, filename);

        logger.LogInformation("Converting Picture");
        await convertPicture.ConvertPictureAsync(consumerContext, filename);

        logger.LogInformation("Set status to successfully converted");
        await databaseOperations.SetSuccessAsync(consumerContext.Message.ConvertStatusId);

        logger.LogInformation("Send PictureConvertedEvent with bus");
        var eventConverted = new MassPictureConvertedEvent()
        {
            ConvertStatusId = consumerContext.Message.ConvertStatusId,
            UploadPictureId = consumerContext.Message.Id
        };
        await consumerContext.Publish<IMassPictureConvertedEvent>(eventConverted);

        logger.LogInformation($"Conversion for file \"{filename}\" successfully completed");
    }

    #endregion
}