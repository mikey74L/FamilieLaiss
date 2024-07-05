using FLBackEnd.API.GraphQL.Mutations.FileUpload;
using FlBackEnd.API.Models;
using MediatR;
using Microsoft.Extensions.Options;

namespace FLBackEnd.API.Mediator.Commands.FileUpload;

/// <summary>
/// Mediatr Command for add upload chunk data for picture
/// </summary>
public class MtrAddUploadChunkPictureCmd : IRequest<bool>
{
    /// <summary>
    /// The Model with the chunk data
    /// </summary>
    public required AddPictureUploadChunkInput Data { get; init; }
}

/// <summary>
/// Mediatr Command-Handler for add upload chunk data for picture
/// </summary>
public class MtrAddUploadChunkPictureCmdHandler(
    IOptions<AppSettings> appSettings,
    ILogger<MtrAddUploadChunkPictureCmdHandler> logger) : IRequestHandler<MtrAddUploadChunkPictureCmd, bool>
{
    #region Mediatr-Handler

    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task</returns>
    public async Task<bool> Handle(MtrAddUploadChunkPictureCmd request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Command-Handler for add upload chunk picture was called");

        var extension = System.IO.Path.GetExtension(request.Data.TargetFilename);

        var filenameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(request.Data.TargetFilename);

        var chunkFilename = System.IO.Path.Combine(appSettings.Value.TempDirectoryUploadPicture,
            $"{filenameWithoutExtension}-{request.Data.ChunkNumber}{extension}");

        var status = true;
        var allowedExtensions = new List<string> { ".jpeg", ".jpg", ".png", ".bmp" };
        var extensionFound = allowedExtensions.Any(ext => extension.ToUpper() == ext.ToUpper());
        status = extensionFound;

        if (status)
        {
            try
            {
                logger.LogDebug("Write chunk to disk");

                var fullFilenameOfChunk =
                    System.IO.Path.Combine(appSettings.Value.TempDirectoryUploadPicture, chunkFilename);

                if (!Directory.Exists(System.IO.Path.GetDirectoryName(fullFilenameOfChunk)))
                {
                    Directory.CreateDirectory(System.IO.Path.GetDirectoryName(fullFilenameOfChunk)!);
                }

                var chunkData = Convert.FromBase64String(request.Data.ChunkData);

                using var stream = new FileStream(fullFilenameOfChunk, FileMode.CreateNew);
                await stream.WriteAsync(chunkData, 0, request.Data.ChunkSize, cancellationToken);

                stream.Close();
            }
            catch
            {
                status = false;
            }
        }

        logger.LogDebug("Return result");
        return status;
    }

    #endregion
}