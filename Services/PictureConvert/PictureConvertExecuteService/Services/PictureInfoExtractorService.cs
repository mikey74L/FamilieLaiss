using System.Threading.Tasks;
using FamilieLaissMassTransitDefinitions.Contracts.Commands;
using MassTransit;
using Microsoft.Extensions.Logging;
using PictureConvertExecuteService.Interfaces;
using SixLabors.ImageSharp;

namespace PictureConvertExecuteService.Services;

public class PictureInfoExtractorService(
    ILogger<PictureInfoExtractorService> logger,
    IDatabaseOperations databaseOperations) : IPictureInfoExtractor
{
    #region Private Methods

    private (long id, int height, int width) GetWidthAndHeightForImage(long id, string filename)
    {
        Image image = Image.Load(filename);

        var returnValue = (id, image.Height, image.Width);

        image.Dispose();

        return returnValue;
    }

    #endregion

    #region Interface iPictureInfoExtractor

    public async Task ExtractInfoAsync(ConsumeContext<IConvertPictureCmd> consumerContext, string filename)
    {
        logger.LogInformation("Set status to read picture info begin");
        await databaseOperations.SetStatusReadInfoBeginAsync(consumerContext.Message.ConvertStatusId);

        logger.LogInformation("Get width and height of picture");
        var tuple = GetWidthAndHeightForImage(consumerContext.Message.Id, filename);
        logger.LogDebug($"Width / Height: {tuple.width} / {tuple.height}");

        logger.LogInformation("Set width and height in database");
        await databaseOperations.SetSizeForPicture(tuple.id, tuple.width, tuple.height);

        logger.LogInformation("Set status to read picture info end");
        await databaseOperations.SetStatusReadInfoEndAsync(consumerContext.Message.ConvertStatusId);
    }

    #endregion
}