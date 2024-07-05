using FLBackEnd.API.GraphQL.Mutations.FileUpload;
using FLBackEnd.API.Hangfire;
using FlBackEnd.API.Models;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Options;

namespace FLBackEnd.API.Mediator.Commands.FileUpload;

/// <summary>
/// Mediatr Command for finish upload for video
/// </summary>
public class MtrFinishVideoUploadCmd : IRequest<bool>
{
    /// <summary>
    /// The Model with the data to finish the upload
    /// </summary>
    public required FinishVideoUploadInput Data { get; init; }
}

/// <summary>
/// Mediatr Command-Handler for finish video upload 
/// </summary>
public class MtrFinishVideoUploadCmdHandler(
    IOptions<AppSettings> appSettings,
    IBus bus,
    IJobOperations jobOperations,
    ILogger<MtrFinishVideoUploadCmdHandler> logger) : IRequestHandler<MtrFinishVideoUploadCmd, bool>
{
    #region Mediatr-Handler

    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task</returns>
    public async Task<bool> Handle(MtrFinishVideoUploadCmd request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for finish video upload command was called with following parameters:");

        try
        {
            logger.LogDebug("Create Hangfire-Job for create file from chunks");
            string jobIdFather = jobOperations.UploadMakeFileFromChunks(
                appSettings.Value.TempDirectoryUploadVideo,
                appSettings.Value.DirectoryUploadVideo,
                request.Data.TargetFilename, request.Data.LastChunkNumber, false, 15);

            logger.LogDebug("Create Hangfire-Job for make database entry");
            jobOperations.WriteToUploadQueue(jobIdFather, FLBackEnd.API.Enums.UploadType.Video,
                Convert.ToInt64(System.IO.Path.GetFileNameWithoutExtension(
                    request.Data.TargetFilename)), request.Data.OriginalFilename, "");

            //logger.LogDebug("Create message command"); //TODO: Activate this when message service is ready
            //string germanText = Resources.Message.VideoUploadedGerman;
            //string englishText = Resources.Message.VideoUploadedEnglish;
            //germanText = string.Format(germanText, request.Data.OriginalFilename);
            //englishText = string.Format(englishText, request.Data.OriginalFilename);
            //CreateMessageForUserGroupCmd command = new(enMessagePrio.Info, UserRoleConstants.Administrator, germanText, englishText, "");

            //logger.LogDebug("Sending Command over service bus");
            //await bus.Send<iCreateMessageForUserGroupCmd>(command);
            return true;
        }
        catch
        {
            return false;
        }
    }

    #endregion
}