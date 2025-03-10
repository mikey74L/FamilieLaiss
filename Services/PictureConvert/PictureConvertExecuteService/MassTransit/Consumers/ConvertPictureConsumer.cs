using FamilieLaissMassTransitDefinitions.Contracts.Commands.UploadPicture;
using MassTransit;
using Microsoft.Extensions.Logging;
using PictureConvertExecuteService.Interfaces;
using System;
using System.Threading.Tasks;

namespace PictureConvertExecuteService.MassTransit.Consumers;

public class ConvertPictureConsumer(
    ILogger<ConvertPictureConsumer> logger,
    IJobExecutor jobExecutor,
    IDatabaseOperations databaseOperations)
    : IConsumer<IMassConvertPictureCmd>, IConsumer<Fault<IMassConvertPictureCmd>>
{
    #region Interface Implementation

    public async Task Consume(ConsumeContext<IMassConvertPictureCmd> context)
    {
        logger.LogInformation("Consumer for ConvertPicture was called with following parameters {@Input}",
            context.Message);

        logger.LogInformation("Starting Job-Executor");
        try
        {
            await jobExecutor.ExecuteJobAsync(context);
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "An error unexpected error occurred. Retry job at later time.");

            try
            {
                logger.LogInformation("Setting job status to error");
                await databaseOperations.SetTransientErrorAsync(context.Message.ConvertStatusId,
                    "Unexpected error occurred. Retrying the job at a later time.");
            }
            catch (Exception ex2)
            {
                logger.LogCritical(ex2, "Could not set job status to error");
            }

            //Throw exception again so MassTransit will make a retry
            throw;
        }
    }

    public async Task Consume(ConsumeContext<Fault<IMassConvertPictureCmd>> context)
    {
        logger.LogInformation("Fault-Consumer for ConvertPicture was called");

        try
        {
            logger.LogInformation("Set job status to error");
            await databaseOperations.SetErrorAsync(context.Message.Message.ConvertStatusId,
                context.Message.Exceptions[0].Message);
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Could not set job status to error in fault consumer");
        }

        try
        {
            logger.LogInformation("Send error event");
            //TODO: Implement event
            //var newEvent = new MassPictureConvertErrorEvent()
            //{
            //    ConvertStatusId = context.Message.Message.ConvertStatusId,
            //    UploadPictureId = context.Message.Message.Id
            //};
            //await context.Publish<IMassPictureConvertErrorEvent>(newEvent);
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Could not send event in fault consumer");
        }

        logger.LogInformation("Fault-Consumer for ConvertPicture is done");
    }

    #endregion
}