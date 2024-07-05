using System;
using System.Threading.Tasks;
using FamilieLaissMassTransitDefinitions.Contracts.Commands;
using FamilieLaissMassTransitDefinitions.Contracts.Events;
using FamilieLaissMassTransitDefinitions.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using PictureConvertExecuteService.Interfaces;

namespace PictureConvertExecuteService.MassTransit.Consumers;

public class ConvertPictureConsumer(
    ILogger<ConvertPictureConsumer> logger,
    IJobExecutor jobExecutor,
    IDatabaseOperations databaseOperations) : IConsumer<IConvertPictureCmd>, IConsumer<Fault<IConvertPictureCmd>>
{
    #region Interface Implementation

    public async Task Consume(ConsumeContext<IConvertPictureCmd> context)
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

    public async Task Consume(ConsumeContext<Fault<IConvertPictureCmd>> context)
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
            var newEvent = new PictureConvertErrorEvent()
            {
                ConvertStatusId = context.Message.Message.ConvertStatusId,
                UploadPictureId = context.Message.Message.Id
            };
            await context.Publish<IPictureConvertErrorEvent>(newEvent);
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Could not send event in fault consumer");
        }

        logger.LogInformation("Fault-Consumer for ConvertPicture is done");
    }

    #endregion
}