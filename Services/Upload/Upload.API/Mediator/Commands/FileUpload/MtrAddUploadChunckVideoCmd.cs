using MediatR;
using Microsoft.Extensions.Options;
using Upload.API.GraphQL.Mutations.FileUpload;
using Upload.API.Models;

namespace Upload.API.Mediator.Commands.FileUpload;

/// <summary>
/// Mediatr Command for add upload chunk data for video
/// </summary>
public class MtrAddUploadChunkVideoCmd : IRequest<bool>
{
    /// <summary>
    /// The Model with the chunk data
    /// </summary>
    public required AddVideoUploadChunkInput Data { get; init; }
}

/// <summary>
/// Mediatr Command-Handler for add upload chunk data for video
/// </summary>
public class MtrAddUploadChunkVideoCmdHandler(
    IOptions<AppSettings> appSettings,
    ILogger<MtrAddUploadChunkVideoCmdHandler> logger) : IRequestHandler<MtrAddUploadChunkVideoCmd, bool>
{
    #region Mediatr-Handler

    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task</returns>
    public async Task<bool> Handle(MtrAddUploadChunkVideoCmd request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Command-Handler for add upload chunk video was called");

        var extension = System.IO.Path.GetExtension(request.Data.TargetFilename);

        var filenameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(request.Data.TargetFilename);

        var chunkFilename = System.IO.Path.Combine(appSettings.Value.TempDirectoryUploadVideo,
            $"{filenameWithoutExtension}-{request.Data.ChunkNumber}{extension}");

        var allowedExtensions = new List<string>
            { ".mp4", ".mov", ".m4v", ".bmp", ".avi", ".mpg", ".mpeg", ".mts", ".wmv" };
        var extensionFound = allowedExtensions.Any(ext => extension.ToUpper() == ext.ToUpper());
        var status = extensionFound;

        if (status)
        {
            try
            {
                logger.LogDebug("Write chunk to disk");

                var fullFilenameOfChunk =
                    System.IO.Path.Combine(appSettings.Value.TempDirectoryUploadVideo, chunkFilename);

                if (!Directory.Exists(System.IO.Path.GetDirectoryName(fullFilenameOfChunk)))
                {
                    var directoryName = System.IO.Path.GetDirectoryName(fullFilenameOfChunk);

                    if (directoryName is not null)
                        Directory.CreateDirectory(directoryName);
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