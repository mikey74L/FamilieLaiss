using MediatR;
using Microsoft.Extensions.Options;
using Upload.API.Models;
using Upload.DTO.FileUpload;

namespace Upload.API.Commands
{
    /// <summary>
    /// Mediatr Command for add upload chunk data for video
    /// </summary>
    public class MtrAddUploadChunckVideoCmd : IRequest<bool>
    {
        #region Properties
        /// <summary>
        /// The Model with the chunk data
        /// </summary>
        public required AddUploadChunckDto Data { get; init; }
        #endregion
    }

    /// <summary>
    /// Mediatr Command-Handler for add upload chunk data for video
    /// </summary>
    public class MtrAddUploadChunckVideoCmdHandler(IOptions<AppSettings> appSettings, ILogger<MtrAddUploadChunckVideoCmdHandler> logger) : IRequestHandler<MtrAddUploadChunckVideoCmd, bool>
    {
        #region Mediatr-Handler
        /// <summary>
        /// Will be called by Mediatr
        /// </summary>
        /// <param name="request">The request data</param>
        /// <param name="cancellationToken">The cancelation token</param>
        /// <returns>Task</returns>
        public async Task<bool> Handle(MtrAddUploadChunckVideoCmd request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Mediatr-Command-Handler for add upload chunck video was called");

            var extension = System.IO.Path.GetExtension(request.Data.TargetFilename);

            var filenameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(request.Data.TargetFilename);

            var chunkFilename = System.IO.Path.Combine(appSettings.Value.Temp_Directory_Upload_Video, $"{filenameWithoutExtension}-{request.Data.ChunkNumber}{extension}");

            var allowedExtensions = new List<string> { ".mp4", ".mov", ".m4v", ".bmp", ".avi", ".mpg", ".mpeg", ".mts", ".wmv" };
            bool extensionFound = false;
            foreach (var ext in allowedExtensions)
            {
                if (extension.ToUpper() == ext.ToUpper())
                {
                    extensionFound = true;
                    break;
                }
            }
            //Check ob die Dateiendung erlaubt ist
            bool status = extensionFound;

            //Den Chunck speichern
            if (status)
            {
                try
                {
                    logger.LogDebug("Write chunck to disk");

                    //Den kompletten Dateinamen erstellen
                    var fullFilenameOfChunk = System.IO.Path.Combine(appSettings.Value.Temp_Directory_Upload_Video, chunkFilename);

                    //Das Verzeichnis erstellen falls dieses noch nicht existiert
                    if (!Directory.Exists(System.IO.Path.GetDirectoryName(fullFilenameOfChunk)))
                    {
                        var directoryName = System.IO.Path.GetDirectoryName(fullFilenameOfChunk);

                        if (directoryName is not null)
                            Directory.CreateDirectory(directoryName);
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
}
