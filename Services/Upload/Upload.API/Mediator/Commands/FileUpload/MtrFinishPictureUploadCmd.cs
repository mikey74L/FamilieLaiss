using MassTransit;
using MediatR;
using Upload.API.GraphQL.Mutations.FileUpload;
using Upload.API.Hangfire;

namespace Upload.API.Mediator.Commands.FileUpload;

/// <summary>
/// Mediatr Command for finish upload for picture
/// </summary>
public class MtrFinishPictureUploadCmd : IRequest<bool>
{
    #region Properties

    /// <summary>
    /// The Model with the data to finish the upload
    /// </summary>
    public required FinishPictureUploadInput Data { get; init; }

    #endregion
}

/// <summary>
/// Mediatr Command-Handler for finish video upload 
/// </summary>
public class MtrFinishPictureUploadCmdHandler(
    IBus bus,
    IJobOperations jobOperations,
    ILogger<MtrFinishPictureUploadCmdHandler> logger) : IRequestHandler<MtrFinishPictureUploadCmd, bool>
{
    #region Mediatr-Handler

    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task</returns>
    public Task<bool> Handle(MtrFinishPictureUploadCmd request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for finish picture upload command was called:");

        try
        {
            logger.LogDebug("Create Hangfire-Job for create file from chunks");
            var jobIdChunks = jobOperations.UploadMakeFileFromChunks(
                request.Data.TargetFilename, request.Data.LastChunkNumber, true, 15);

            var uploadPictureId = Convert.ToInt64(System.IO.Path.GetFileNameWithoutExtension(
                request.Data.TargetFilename));

            logger.LogDebug("Create Hangfire-Job for make database entry");
            var jobIdUpload = jobOperations.WriteToUploadQueue(jobIdChunks, Upload.API.Enums.UploadType.Picture,
                uploadPictureId, request.Data.OriginalFilename, "");

            logger.LogDebug("Create Hangfire-Job for extract picture info");
            var jobIdPictureInfo = jobOperations.ExtractPictureInfo(jobIdUpload, uploadPictureId,
                request.Data.TargetFilename);

            logger.LogDebug("Create Hangfire-Job for extract picture metadata");
            var jobIdPictureMetadata = jobOperations.ExtractPictureMetadata(jobIdPictureInfo, uploadPictureId,
                request.Data.TargetFilename);

            logger.LogDebug("Create Hangfire-Job for convert picture");
            var jobIdPictureConvert = jobOperations.ConvertPicture(jobIdPictureMetadata, uploadPictureId,
                request.Data.TargetFilename);

            logger.LogDebug("Create Hangfire-Job for uploading to blob storage");
            var jobIdBlobStorage = jobOperations.UploadPictureToBlobStorage(jobIdPictureConvert, uploadPictureId,
                request.Data.TargetFilename);

            logger.LogDebug("Create Hangfire-Job for deleting files from physical drive");
            var jobIdDeleteFiles = jobOperations.DeleteFilesFromPhysicalDrive(jobIdBlobStorage, uploadPictureId,
                request.Data.TargetFilename);

            //logger.LogDebug("Create message command"); //TODO: Activate this when message service is ready
            //var germanText = Resources.Message.PictureUploadedGerman;
            //var englishText = Resources.Message.PictureUploadedEnglish;
            //germanText = string.Format(germanText, request.Data.OriginalFilename);
            //englishText = string.Format(englishText, request.Data.OriginalFilename);
            //CreateMessageForUserGroupCmd command = new(enMessagePrio.Info, UserRoleConstants.Administrator, germanText,
            //    englishText, "");

            //logger.LogDebug("Sending Command over service bus"); 
            //await bus.Send<iCreateMessageForUserGroupCmd>(command, cancellationToken: cancellationToken);

            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    #endregion
}