using MediatR;
using Microsoft.Extensions.Options;
using Upload.API.Models;
using Upload.DTO.FileUpload;

namespace Upload.API.Mediator.Commands;

/// <summary>
/// Mediatr Command for add upload chunk data for picture
/// </summary>
public class MtrAddUploadChunckPictureCmd : IRequest<bool>
{
    #region Properties
    /// <summary>
    /// The Model with the chunk data
    /// </summary>
    public required AddUploadChunckDto Data { get; init; }
    #endregion
}

/// <summary>
/// Mediatr Command-Handler for add upload chunk data for picture
/// </summary>
public class MtrAddUploadChunckPictureCmdHandler(IOptions<AppSettings> appSettings, ILogger<MtrAddUploadChunckPictureCmdHandler> logger) : IRequestHandler<MtrAddUploadChunckPictureCmd, bool>
{
    #region Mediatr-Handler
    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancelation token</param>
    /// <returns>Task</returns>
    public async Task<bool> Handle(MtrAddUploadChunckPictureCmd request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Command-Handler for add upload chunck picture was called");

        var extension = Path.GetExtension(request.Data.TargetFilename);

        var filenameWithoutExtension = Path.GetFileNameWithoutExtension(request.Data.TargetFilename);

        var chunkFilename = Path.Combine(appSettings.Value.Temp_Directory_Upload_Picture, $"{filenameWithoutExtension}-{request.Data.ChunkNumber}{extension}");

        //Check ob die Dateiendung erlaubt ist
        bool status = true;
        var allowedExtensions = new List<string> { ".jpeg", ".jpg", ".png", ".bmp" };
        bool extensionFound = false;
        foreach (var ext in allowedExtensions)
        {
            if (extension.ToUpper() == ext.ToUpper())
            {
                extensionFound = true;
                break;
            }
        }
        status = extensionFound;

        //Den Chunck speichern
        if (status)
        {
            try
            {
                logger.LogDebug("Write chunck to disk");

                //Den kompletten Dateinamen erstellen
                var fullFilenameOfChunk = Path.Combine(appSettings.Value.Temp_Directory_Upload_Picture, chunkFilename);

                //Das Verzeichnis erstellen falls dieses noch nicht existiert
                if (!Directory.Exists(Path.GetDirectoryName(fullFilenameOfChunk)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(fullFilenameOfChunk)!);
                }

                //Umwandeln des base64 enkodierten Strings in ein Byte-Array
                var chunkData = Convert.FromBase64String(request.Data.ChunkData);

                //Erstellen eines Filestreams und schreiben der Chunk-Daten
                using (var stream = new FileStream(fullFilenameOfChunk, FileMode.CreateNew))
                {
                    await stream.WriteAsync(chunkData, 0, request.Data.ChunkSize);

                    stream.Close();

                    await stream.DisposeAsync();
                }
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
