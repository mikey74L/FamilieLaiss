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

namespace Upload.API.Mediator.Commands
{
    /// <summary>
    /// Mediatr Command for finish upload for video
    /// </summary>
    public class MtrFinishVideoUploadCmd : IRequest<bool>
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
    public class MtrFinishVideoUploadCmdHandler(IOptions<AppSettings> appSettings, IBus bus, iJobOperations jobOperations, ILogger<MtrFinishVideoUploadCmdHandler> logger) : IRequestHandler<MtrFinishVideoUploadCmd, bool>
    {
        #region Mediatr-Handler
        /// <summary>
        /// Will be called by Mediatr
        /// </summary>
        /// <param name="request">The request data</param>
        /// <param name="cancellationToken">The cancelation token</param>
        /// <returns>Task</returns>
        public async Task<bool> Handle(MtrFinishVideoUploadCmd request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Mediatr-Handler for finish video upload command was called with following parameters:");

            try
            {
                //Erstellen des Jobs zum Erstellen der Datei aus den Chunks
                logger.LogDebug("Create Hangfire-Job for create file from chuncks");
                string jobIDVater = jobOperations.UploadMakeFileFromChunks(
                    appSettings.Value.Temp_Directory_Upload_Video,
                    appSettings.Value.Directory_Upload_Video,
                    request.Data.TargetFilename, request.Data.LastChunkNumber, false, 15);

                //Erstellen des Jobs für die Upload-Queue
                logger.LogDebug("Create Hangfire-Job for make database entry");
                jobOperations.WriteToUploadQueue(jobIDVater, UploadType.Video,
                    Convert.ToInt64(System.IO.Path.GetFileNameWithoutExtension(
                        request.Data.TargetFilename)), request.Data.OriginalFilename, "");

                //Erstellen der Nachricht
                logger.LogDebug("Create message command");
                string germanText = Resources.Message.VideoUploadedGerman;
                string englishText = Resources.Message.VideoUploadedEnglish;
                germanText = string.Format(germanText, request.Data.OriginalFilename);
                englishText = string.Format(englishText, request.Data.OriginalFilename);
                CreateMessageForUserGroupCmd command = new(enMessagePrio.Info, UserRoleConstants.Administrator, germanText, englishText, "");

                //Versenden des Commands über den Service Bus
                logger.LogDebug("Sending Command over service bus");
                await bus.Send<iCreateMessageForUserGroupCmd>(command);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
