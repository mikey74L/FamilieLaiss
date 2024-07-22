using FamilieLaissMassTransitDefinitions.Commands;
using FamilieLaissMassTransitDefinitions.Contracts.Commands;
using MassTransit;
using Microsoft.Extensions.Logging;
using PictureConvertExecuteService.Interfaces;
using SixLabors.ImageSharp;
using System.Threading.Tasks;

namespace PictureConvertExecuteService.Services;

public class PictureInfoExtractorService(
    ILogger<PictureInfoExtractorService> logger,
    IDatabaseOperations databaseOperations,
    IBus massTransit) : IPictureInfoExtractor
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

    public async Task ExtractInfoAsync(ConsumeContext<IMassConvertPictureCmd> consumerContext, string filename)
    {
        logger.LogInformation("Set status to read picture info begin");
        await databaseOperations.SetStatusReadInfoBeginAsync(consumerContext.Message.ConvertStatusId);

        logger.LogInformation("Get width and height of picture");
        var tuple = GetWidthAndHeightForImage(consumerContext.Message.Id, filename);
        logger.LogDebug($"Width / Height: {tuple.width} / {tuple.height}");

        logger.LogInformation("Send command over message bus to set picture dimensions");
        var newCommand = new MassSetUploadPictureDimensionsCmd()
        {
            Id = tuple.id,
            Height = tuple.height,
            Width = tuple.width
        };
        await massTransit.Send<IMassSetUploadPictureDimensionsCmd>(newCommand);

        logger.LogInformation("Set status to read picture info end");
        await databaseOperations.SetStatusReadInfoEndAsync(consumerContext.Message.ConvertStatusId);
    }

    #endregion
}