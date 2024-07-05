using DomainHelper.Interfaces;
using FamilieLaissMassTransitDefinitions.Contracts.Events;
using FamilieLaissMassTransitDefinitions.Events;
using MassTransit;
using Microsoft.Extensions.Options;
using Upload.API.Enums;
using Upload.API.Models;
using Upload.Domain.Entities;

namespace Upload.API.Hangfire;

/// <summary>
/// This class is holds the job methods for Hangfire that will be executed
/// </summary>
public class JobExecuter(IOptions<AppSettings> appSettings, iJobOperations jobOperations, iUnitOfWork unitOfWork, IBus bus, ILogger<JobExecuter> logger)
{
    #region Upload
    /// <summary>
    /// Erstellt aus einer Liste von Chunks ein File und verschiebt dieses dann in das Zielverzeichnis.
    /// Danach werden die Chunks gelöscht.
    /// </summary>
    /// <param name="tempUploadFolder">Temporärer Upload-Ordner</param>
    /// <param name="uploadFolder">Upload-Ordner (Zielverzeichnis)</param>
    /// <param name="destinationFilename">Dateiname für die Zieldatei</param>
    /// <param name="lastChunkNumber">The last chunk number used for the upload file</param>
    /// <param name="deleteAlreadyExisting">Löscht eine bereits vorhanden Datei im Zielverzeichnis mit gleichem Namen</param>
    public void UploadMakeFileFromChunks(string tempUploadFolder, string uploadFolder, string destinationFilename, long lastChunkNumber, bool deleteAlreadyExisting)
    {
        logger.LogInformation("Action called with following parameters:");
        logger.LogDebug($"Temporary upload folder: {tempUploadFolder}");
        logger.LogDebug($"Upload-Folder          : {uploadFolder}");
        logger.LogDebug($"Destination filename   : {destinationFilename}");
        logger.LogDebug($"Last chunk number      : {lastChunkNumber}");
        logger.LogDebug($"Delete already existing: {deleteAlreadyExisting}");

        try
        {
            //Zielverzeichnis anlegen wenn es noch nicht existiert
            logger.LogDebug($"Create directory \"{uploadFolder}\" if not exists");
            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            //Erstellen der Datei aus den Chunks
            logger.LogDebug($"Create and open destination stream for \"{Path.Combine(tempUploadFolder, destinationFilename)}\"");
            FileStream destStream = File.Create(Path.Combine(tempUploadFolder, destinationFilename));
            logger.LogDebug("Adding all chuncks to destination stream");
            List<string> listOfChunkFilenames = new();
            for (int i = 1; i <= lastChunkNumber; i++)
            {
                string filenameSource = Path.Combine(tempUploadFolder, $"{Path.GetFileNameWithoutExtension(destinationFilename)}-{i}{Path.GetExtension(destinationFilename)}");
                FileStream sourceStream = File.OpenRead(filenameSource);
                listOfChunkFilenames.Add(filenameSource);
                sourceStream.CopyTo(destStream); // You can pass the buffer size as second argument.
                sourceStream.Close();
                sourceStream.Dispose();
            }
            logger.LogDebug("Close destination stream");
            destStream.Close();
            logger.LogDebug("Dispose destination stream");
            destStream.Dispose();

            //Löschen der Datei im Zielverzeichnis wenn Diese schon existiert und dieses gefordert ist
            if (deleteAlreadyExisting)
            {
                logger.LogDebug($"Check is file \"{Path.Combine(uploadFolder, destinationFilename)}\" already exists");
                if (File.Exists(Path.Combine(uploadFolder, destinationFilename)))
                {
                    logger.LogDebug($"Delete file \"{Path.Combine(uploadFolder, destinationFilename)}\"");
                    File.Delete(Path.Combine(uploadFolder, destinationFilename));
                }
            }

            //Verschieben der fertigen Datei aus dem Temporären Verzeichnis in das Zielverzeichnis
            logger.LogDebug($"Move file from \"{Path.Combine(tempUploadFolder, destinationFilename)}\" to \"{Path.Combine(uploadFolder, destinationFilename)}\"");
            File.Move(Path.Combine(tempUploadFolder, destinationFilename), Path.Combine(uploadFolder, destinationFilename));

            //Löschen der einzelnen Chunk-Files
            logger.LogDebug("Delete all chuncks");
            foreach (string filename in listOfChunkFilenames)
            {
                try
                {
                    File.Delete(filename);
                }
                catch (Exception ex)
                {
                    logger.LogCritical(ex, "Not all chuncks have been deleted");
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Unexpected error occurred");
            throw;
        }
    }

    /// <summary>
    /// Einstellen eines Eintrages in die Upload-Queue. Mit diesem Eintrag laufen dann der Picture bzw. Video Converter Service los.
    /// </summary>
    /// <param name="uploadType">Um welchen Upload-Typ handelt es sich</param>
    /// <param name="ID">Die ID des Upload-Items</param>
    /// <param name="originalName">Der originale Dateiname des hochgeladenen Files</param>
    /// <param name="userName">The user name for user account (only needed when portrait is uploaded)</param>
    public async Task WriteToUploadQueue(UploadType uploadType, long ID, string originalName, string userName)
    {
        logger.LogInformation("Action called with folloowing parameters:");
        logger.LogDebug($"Upload-Type  : {uploadType}");
        logger.LogDebug($"ID           : {ID}");
        logger.LogDebug($"Original-Name: {originalName}");
        if (!string.IsNullOrEmpty(userName))
        {
            logger.LogDebug($"User-Name    : {userName.Substring(1, 3)}");
        }

        try
        {
            //Es handelt sich um ein Video
            if (uploadType == UploadType.Video)
            {
                logger.LogDebug("Upload is a video");

                logger.LogDebug("Create new entity");
                var newEntity = new UploadVideo(ID, originalName);

                logger.LogDebug("Get repository");
                var repository = unitOfWork.GetRepository<UploadVideo>();

                logger.LogDebug("Adding entity to store");
                await repository.AddAsync(newEntity);
            }

            //Es handelt sich um ein Photo
            if (uploadType == UploadType.Picture)
            {
                logger.LogDebug("Upload is a picture");

                logger.LogDebug("Create new entity");
                var newEntity = new UploadPicture(ID, originalName);

                logger.LogDebug("Get repository");
                var repository = unitOfWork.GetRepository<UploadPicture>();

                logger.LogDebug("Adding entity to store");
                await repository.AddAsync(newEntity);
            }

            ////Es handelt sich um ein Portrait
            ////Beim Portrait kann es auch sein, dass es schon einen Eintrag gibt und das Portrait-Bild
            ////ausgetauscht werden soll. In diesem Fall muss der bestehende Eintrag gelöscht, und ein
            ////neuer Eintrag erstellt werden
            //if (uploadType == enUploadType.Portrait)
            //{
            //    //Logging
            //    _Logger.LogDebug("Upload is a portrait");

            //    //Erstellen der Entity
            //    _Logger.LogDebug("Create new entity");
            //    var NewEntity = new UploadPortrait(ID, userName);

            //    //Erstellen des Repository
            //    _Logger.LogDebug("Get repository");
            //    var Repository = _UnitOfWork.GetRepository<UploadPortrait>();

            //    //Überprüfen ob es schon einen Eintrag gibt
            //    _Logger.LogDebug("Check if already a entity exists for the user");
            //    var FoundItem = (await Repository.GetAll(x => x.UserName == userName)).FirstOrDefault();

            //    //Wenn ein Element gefunden wurde, muss dieses zuerst gelöscht werden
            //    if (FoundItem != null)
            //    {
            //        _Logger.LogDebug("A entity for the given user already exists. Delete existing entity before adding new one.");
            //        Repository.Delete(FoundItem);
            //    }

            //    //Hinzufügen der Entity zum Store
            //    _Logger.LogDebug("Adding entity to store");
            //    await Repository.AddAsync(NewEntity);
            //}

            logger.LogDebug("Saving changes");
            await unitOfWork.SaveChangesAsync();

            //Erstellen des Bus-Events
            switch (uploadType)
            {
                case UploadType.Picture:
                    logger.LogDebug("Publish picture uploaded event over service bus");
                    await bus.Publish<IPictureUploadedEvent>(new PictureUploadedEvent(ID, originalName));
                    break;
                case UploadType.Video:
                    logger.LogDebug("Publish video uploaded event over service bus");
                    await bus.Publish<iVideoUploadedEvent>(new VideoUploadedEvent(ID, originalName));
                    break;
                case UploadType.Portrait:
                    break;
            }
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Unexpected error occurred");
            throw;
        }
    }
    #endregion
}
