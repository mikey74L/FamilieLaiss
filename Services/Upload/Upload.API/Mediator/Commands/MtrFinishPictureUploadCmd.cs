using FamilieLaissMassTransitDefinitions.Commands;
using FamilieLaissMassTransitDefinitions.Contracts.Commands;
using FamilieLaissSharedObjects.Constants;
using FamilieLaissSharedObjects.Enums;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Options;
using Upload.API.Enums;
using Upload.API.Hangfire;
using Upload.API.Models;
using Upload.DTO.FileUpload;

namespace Upload.API.Mediator.Commands;

/// <summary>
/// Mediatr Command for finish upload for picture
/// </summary>
public class MtrFinishPictureUploadCmd : IRequest<bool>
{
    #region Properties

    /// <summary>
    /// The Model with the data to finish the upload
    /// </summary>
    public required FinishUploadDto Data { get; init; }

    #endregion
}

/// <summary>
/// Mediatr Command-Handler for finish video upload 
/// </summary>
public class MtrFinishPictureUploadCmdHandler(
    IOptions<AppSettings> appSettings,
    IBus bus,
    iJobOperations jobOperations,
    ILogger<MtrFinishPictureUploadCmdHandler> logger) : IRequestHandler<MtrFinishPictureUploadCmd, bool>
{
    #region Mediatr-Handler

    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task</returns>
    public async Task<bool> Handle(MtrFinishPictureUploadCmd request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for finish picture upload command was called:");

        try
        {
            logger.LogDebug("Create Hangfire-Job for create file from chunks");
            var jobIdFather = jobOperations.UploadMakeFileFromChunks(
                appSettings.Value.Temp_Directory_Upload_Picture,
                appSettings.Value.Directory_Upload_Picture,
                request.Data.TargetFilename, request.Data.LastChunkNumber, false, 15);

            logger.LogDebug("Create Hangfire-Job for make database entry");
            jobOperations.WriteToUploadQueue(jobIdFather, UploadType.Picture,
                Convert.ToInt64(Path.GetFileNameWithoutExtension(
                    request.Data.TargetFilename)), request.Data.OriginalFilename, "");

            logger.LogDebug("Create message command");
            var germanText = Resources.Message.PictureUploadedGerman;
            var englishText = Resources.Message.PictureUploadedEnglish;
            germanText = string.Format(germanText, request.Data.OriginalFilename);
            englishText = string.Format(englishText, request.Data.OriginalFilename);
            CreateMessageForUserGroupCmd command = new(enMessagePrio.Info, UserRoleConstants.Administrator, germanText,
                englishText, "");

            logger.LogDebug("Sending Command over service bus");
            await bus.Send<iCreateMessageForUserGroupCmd>(command, cancellationToken: cancellationToken);

            return true;
        }
        catch
        {
            return false;
        }
    }

    #endregion
}