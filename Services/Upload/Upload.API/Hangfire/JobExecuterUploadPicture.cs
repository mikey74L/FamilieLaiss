using Azure.Storage.Blobs;
using DomainHelper.Interfaces;
using MediatR;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Upload.API.Interfaces;
using Upload.API.Mediator.Commands.UploadPicture;
using Upload.API.Models;
using Upload.Domain.Entities;
using Directory = MetadataExtractor.Directory;
using Path = System.IO.Path;

namespace Upload.API.Hangfire;

/// <summary>
/// This class is holds the job methods for Hangfire that will be executed
/// </summary>
public class JobExecutorUploadPicture(
    iUnitOfWork unitOfWork,
    ILogger<JobExecutorUploadPicture> logger,
    BlobServiceClient blobServiceClient,
    IMediator mediator,
    IOptions<AppSettings> appSettings,
    IFolderHelperService folderHelperService)
{
    #region Private Classes

    private sealed class URational
    {
        public URational(byte[] bytes)
        {
            var n = new byte[4];
            var d = new byte[4];
            Array.Copy(bytes, 0, n, 0, 4);
            Array.Copy(bytes, 4, d, 0, 4);
            Num = BitConverter.ToUInt32(n, 0);
            Denom = BitConverter.ToUInt32(d, 0);
        }

        public uint Num { private set; get; }
        public uint Denom { private set; get; }
    }

    #endregion

    #region Private Methods

    private (long id, int height, int width) GetWidthAndHeightForImage(long id, string filename)
    {
        Image image = Image.Load(filename);

        var returnValue = (id, image.Height, image.Width);

        image.Dispose();

        return returnValue;
    }

    private void GetExifInfo(string filename, UploadPicture uploadPicture)
    {
        string make = string.Empty;
        string model = string.Empty;
        double? resolutionX = null;
        double? resolutionY = null;
        short? resolutionUnit = null;
        short? orientation = null;
        DateTimeOffset? ddlRecorded = null;
        double? exposureTime = null;
        short? exposureProgram = null;
        short? exposureMode = null;
        double? fNumber = null;
        int? isoSensitivity = null;
        double? shutterSpeed = null;
        short? meteringMode = null;
        short? flashMode = null;
        double? focalLength = null;
        short? sensingMode = null;
        short? whiteBalanceMode = null;
        short? sharpness = null;
        double? gpsLongitude;
        double? gpsLatitude;
        short? contrast = null;
        short? saturation = null;

        IEnumerable<Directory> directories = ImageMetadataReader.ReadMetadata(filename);

        var ifd0Directory = directories.OfType<ExifIfd0Directory>().FirstOrDefault();

        if (ifd0Directory is not null)
        {
            try
            {
                make = ifd0Directory.ContainsTag(ExifDirectoryBase.TagMake)
                    ? ifd0Directory.GetString(ExifDirectoryBase.TagMake) ?? ""
                    : "";
            }
            catch
            {
                make = "";
            }

            try
            {
                model = ifd0Directory.ContainsTag(ExifDirectoryBase.TagModel)
                    ? ifd0Directory.GetString(ExifDirectoryBase.TagModel) ?? ""
                    : "";
            }
            catch
            {
                model = "";
            }

            try
            {
                resolutionX = ifd0Directory.ContainsTag(ExifDirectoryBase.TagXResolution)
                    ? ifd0Directory.GetDouble(ExifDirectoryBase.TagXResolution)
                    : null;
            }
            catch
            {
                resolutionX = null;
            }

            try
            {
                resolutionY = ifd0Directory.ContainsTag(ExifDirectoryBase.TagYResolution)
                    ? ifd0Directory.GetDouble(ExifDirectoryBase.TagYResolution)
                    : null;
            }
            catch
            {
                resolutionY = null;
            }

            try
            {
                resolutionUnit = ifd0Directory.ContainsTag(ExifDirectoryBase.TagResolutionUnit)
                    ? ifd0Directory.GetInt16(ExifDirectoryBase.TagResolutionUnit)
                    : null;
            }
            catch
            {
                resolutionUnit = null;
            }

            try
            {
                orientation = ifd0Directory.ContainsTag(ExifDirectoryBase.TagOrientation)
                    ? ifd0Directory.GetInt16(ExifDirectoryBase.TagOrientation)
                    : null;
            }
            catch
            {
                orientation = null;
            }

            try
            {
                ddlRecorded = ifd0Directory.ContainsTag(ExifDirectoryBase.TagDateTime)
                    ? ifd0Directory.GetDateTime(ExifDirectoryBase.TagDateTime)
                    : null;
            }
            catch
            {
                ddlRecorded = null;
            }
        }

        var subIfdDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();

        if (subIfdDirectory is not null)
        {
            try
            {
                fNumber = subIfdDirectory.ContainsTag(ExifDirectoryBase.TagFNumber)
                    ? subIfdDirectory.GetDouble(ExifDirectoryBase.TagFNumber)
                    : null;

                if (fNumber is not null)
                {
                    fNumber = Math.Round(fNumber.Value, 1, MidpointRounding.AwayFromZero);
                }
            }
            catch
            {
                fNumber = null;
            }

            try
            {
                exposureProgram = subIfdDirectory.ContainsTag(ExifDirectoryBase.TagExposureProgram)
                    ? subIfdDirectory.GetInt16(ExifDirectoryBase.TagExposureProgram)
                    : null;
            }
            catch
            {
                exposureProgram = null;
            }

            try
            {
                isoSensitivity = subIfdDirectory.ContainsTag(ExifDirectoryBase.TagIsoEquivalent)
                    ? subIfdDirectory.GetInt32(ExifDirectoryBase.TagIsoEquivalent)
                    : null;
            }
            catch
            {
                isoSensitivity = null;
            }

            try
            {
                shutterSpeed = subIfdDirectory.ContainsTag(ExifDirectoryBase.TagShutterSpeed)
                    ? subIfdDirectory.GetDouble(ExifDirectoryBase.TagShutterSpeed)
                    : null;
            }
            catch
            {
                shutterSpeed = null;
            }

            try
            {
                meteringMode = subIfdDirectory.ContainsTag(ExifDirectoryBase.TagMeteringMode)
                    ? subIfdDirectory.GetInt16(ExifDirectoryBase.TagMeteringMode)
                    : null;
            }
            catch
            {
                meteringMode = null;
            }

            try
            {
                flashMode = subIfdDirectory.ContainsTag(ExifDirectoryBase.TagFlash)
                    ? subIfdDirectory.GetInt16(ExifDirectoryBase.TagFlash)
                    : null;
            }
            catch
            {
                flashMode = null;
            }

            try
            {
                focalLength = subIfdDirectory.ContainsTag(ExifDirectoryBase.TagFocalLength)
                    ? Math.Round(subIfdDirectory.GetDouble(ExifDirectoryBase.TagFocalLength), 0,
                        MidpointRounding.AwayFromZero)
                    : null;
            }
            catch
            {
                focalLength = null;
            }

            try
            {
                sensingMode = subIfdDirectory.ContainsTag(ExifDirectoryBase.TagSensingMethod)
                    ? subIfdDirectory.GetInt16(ExifDirectoryBase.TagSensingMethod)
                    : null;
            }
            catch
            {
                sensingMode = null;
            }

            try
            {
                exposureMode = subIfdDirectory.ContainsTag(ExifDirectoryBase.TagExposureMode)
                    ? subIfdDirectory.GetInt16(ExifDirectoryBase.TagExposureMode)
                    : null;
            }
            catch
            {
                exposureMode = null;
            }

            try
            {
                whiteBalanceMode = subIfdDirectory.ContainsTag(ExifDirectoryBase.TagWhiteBalanceMode)
                    ? subIfdDirectory.GetInt16(ExifDirectoryBase.TagWhiteBalanceMode)
                    : null;
            }
            catch
            {
                whiteBalanceMode = null;
            }

            try
            {
                sharpness = subIfdDirectory.ContainsTag(ExifDirectoryBase.TagSharpness)
                    ? subIfdDirectory.GetInt16(ExifDirectoryBase.TagSharpness)
                    : null;
            }
            catch
            {
                sharpness = null;
            }

            try
            {
                contrast = subIfdDirectory.ContainsTag(ExifDirectoryBase.TagContrast)
                    ? subIfdDirectory.GetInt16(ExifDirectoryBase.TagContrast)
                    : null;
            }
            catch
            {
                contrast = null;
            }

            try
            {
                exposureTime = subIfdDirectory.ContainsTag(ExifDirectoryBase.TagExposureTime)
                    ? Math.Round(subIfdDirectory.GetDouble(ExifDirectoryBase.TagExposureTime), 2,
                        MidpointRounding.AwayFromZero)
                    : null;
            }
            catch
            {
                exposureTime = null;
            }

            try
            {
                saturation = subIfdDirectory.ContainsTag(ExifDirectoryBase.TagSaturation)
                    ? subIfdDirectory.GetInt16(ExifDirectoryBase.TagSaturation)
                    : null;
            }
            catch
            {
                saturation = null;
            }
        }

        var gpsDirectory = directories.OfType<GpsDirectory>().FirstOrDefault();

        if (gpsDirectory != null)
        {
            try
            {
                var gpsLocation = gpsDirectory.GetGeoLocation();
                if (gpsLocation != null)
                {
                    gpsLongitude = gpsLocation.Longitude;
                    gpsLatitude = gpsLocation.Latitude;
                }
                else
                {
                    gpsLongitude = null;
                    gpsLatitude = null;
                }
            }
            catch
            {
                gpsLongitude = null;
                gpsLatitude = null;
            }
        }
        else
        {
            gpsLongitude = null;
            gpsLatitude = null;
        }

        //Correct the GPS data if the geo position is 0 or -0
        //This occurs in some images because not all Apps write clean GPS positions into the Exif information
        //This needs to be corrected to NULL, as these are not valid GPS positions, and will lead to
        //errors when determining the geo position later
        if (gpsLongitude.HasValue || gpsLatitude.HasValue)
        {
            if (gpsLongitude.HasValue && (gpsLongitude.Value == 0 || gpsLongitude.Value == -0))
            {
                gpsLongitude = null;
                gpsLatitude = null;
            }

            if (gpsLatitude.HasValue && (gpsLatitude.Value == 0 || gpsLatitude.Value == -0))
            {
                gpsLongitude = null;
                gpsLatitude = null;
            }
        }

        logger.LogInformation("Write exif Info to database");
        uploadPicture.SetExifData(make, model, resolutionX, resolutionY, resolutionUnit, orientation, ddlRecorded,
            exposureTime, exposureProgram, exposureMode, fNumber, isoSensitivity, shutterSpeed, meteringMode,
            flashMode, focalLength, sensingMode, whiteBalanceMode, sharpness, gpsLongitude, gpsLatitude,
            contrast, saturation);
    }

    private void DoConvertion(string filename, string mode, int width, int height)
    {
        string fullFilenameTarget;
        string modeOriginal = "";
        string extensionTarget = "";

        var fullFilename = Path.Combine(folderHelperService.GetFolderNameForUploadPicture(),
            Path.GetFileNameWithoutExtension(filename));
        var extensionOriginal = Path.GetExtension(filename);
        var fullFilenameOriginal = fullFilename + extensionOriginal;
        if (extensionOriginal.Equals(".JPG", StringComparison.CurrentCultureIgnoreCase) ||
            extensionOriginal.Equals(".JPEG", StringComparison.CurrentCultureIgnoreCase))
        {
            modeOriginal = "jpeg";
        }

        if (extensionOriginal.Equals(".PNG", StringComparison.CurrentCultureIgnoreCase))
        {
            modeOriginal = "png";
        }

        if (extensionOriginal.Equals(".GIF", StringComparison.CurrentCultureIgnoreCase))
        {
            modeOriginal = "gif";
        }

        if (extensionOriginal.Equals(".BMP", StringComparison.CurrentCultureIgnoreCase))
        {
            modeOriginal = "bmp";
        }

        if (height > 0 || width > 0)
        {
            fullFilenameTarget = fullFilename + "_" + height.ToString() + "_" + width.ToString();
        }
        else
        {
            fullFilenameTarget = fullFilename;
        }

        extensionTarget = mode switch
        {
            "png" => ".png",
            "jpeg" => ".jpg",
            "bmp" => ".bmp",
            _ => extensionTarget
        };

        if (!mode.Equals(modeOriginal, StringComparison.CurrentCultureIgnoreCase))
        {
            fullFilenameTarget = fullFilenameTarget + extensionTarget;
        }
        else
        {
            fullFilenameTarget = fullFilenameTarget + extensionOriginal;
        }

        if (File.Exists(fullFilenameTarget)) return;
        var originalImage = Image.Load(fullFilenameOriginal);

        originalImage.Mutate(x => x.AutoOrient());

        if (height > 0 || width > 0)
        {
            ResizeOptions options = new()
            {
                Size = new Size(width, height),
                Mode = ResizeMode.Max
            };

            originalImage.Mutate(x => x.Resize(options));
        }

        switch (mode)
        {
            case "png":
                using (var ms = new FileStream(fullFilenameTarget, FileMode.OpenOrCreate))
                {
                    originalImage.SaveAsPng(ms);
                }

                break;
            case "jpeg":
                using (var ms = new FileStream(fullFilenameTarget, FileMode.OpenOrCreate))
                {
                    originalImage.SaveAsJpeg(ms);
                }

                break;
            case "bmp":
                using (var ms = new FileStream(fullFilenameTarget, FileMode.OpenOrCreate))
                {
                    originalImage.SaveAsBmp(ms);
                }

                break;
            default:
                using (var ms = new FileStream(fullFilenameTarget, FileMode.OpenOrCreate))
                {
                    originalImage.SaveAsJpeg(ms);
                }

                break;
        }
    }

    private async Task UploadFileToBlobStorage(BlobContainerClient blobContainerClient, string fullFilename)
    {
        var blobClient = blobContainerClient.GetBlobClient(Path.GetFileName(fullFilename));
        await using var uploadFileStream = File.OpenRead(fullFilename);
        await blobClient.UploadAsync(uploadFileStream, true);
        uploadFileStream.Close();
    }

    private async Task UploadFilesToBlobStorage(string destinationFilename)
    {
        var blobContainerClient = blobServiceClient.GetBlobContainerClient("upload-picture");
        await blobContainerClient.CreateIfNotExistsAsync();

        var originalFilename = Path.GetFileNameWithoutExtension(destinationFilename);

        //Upload original picture
        await UploadFileToBlobStorage(blobContainerClient, destinationFilename);

        //Upload picture for card
        await UploadFileToBlobStorage(blobContainerClient,
            folderHelperService.GetFullFilenameForUploadPictureCard(destinationFilename));

        //Upload picture for gallery
        await UploadFileToBlobStorage(blobContainerClient,
            folderHelperService.GetFullFilenameForUploadPictureGallery(destinationFilename));

        //Upload picture for thumbnail
        await UploadFileToBlobStorage(blobContainerClient,
            folderHelperService.GetFullFilenameForUploadPictureThumbnail(destinationFilename));
    }

    private void DeleteFiles(string destinationFilename)
    {
        var fullFilename = folderHelperService.GetFullFilenameForUploadPicture(destinationFilename);
        var fullFilenameCard = folderHelperService.GetFullFilenameForUploadPictureCard(destinationFilename);
        var fullFilenameGallery = folderHelperService.GetFullFilenameForUploadPictureGallery(destinationFilename);
        var fullFilenameThumbnail = folderHelperService.GetFullFilenameForUploadPictureThumbnail(destinationFilename);

        if (File.Exists(fullFilename))
        {
            File.Delete(fullFilename);
        }

        if (File.Exists(fullFilenameCard))
        {
            File.Delete(fullFilenameCard);
        }

        if (File.Exists(fullFilenameGallery))
        {
            File.Delete(fullFilenameGallery);
        }

        if (File.Exists(fullFilenameThumbnail))
        {
            File.Delete(fullFilenameThumbnail);
        }
    }
    #endregion

    #region Public Methods

    public async Task ExtractPictureInfo(long id, string destinationFilename)
    {
        logger.LogInformation("Action called with following parameters:");
        logger.LogDebug($"Destination Filename: {destinationFilename}");

        logger.LogInformation("Set status to read picture info begin");
        var repoConvertStatus =
            unitOfWork.GetRepository<PictureConvertStatus>();
        var entityStatus =
            (await repoConvertStatus.GetAll(x => x.UploadPictureId == id)).FirstOrDefault();
        entityStatus?.SetStatusStartInfo();
        await unitOfWork.SaveChangesAsync();

        try
        {
            logger.LogInformation("Get width and height of picture");
            var tuple = GetWidthAndHeightForImage(id, folderHelperService.GetFullFilenameForUploadPicture(destinationFilename));
            logger.LogDebug($"Width / Height: {tuple.width} / {tuple.height}");

            logger.LogInformation("Get repository for upload picture from unit of work");
            var repoUploadPicture = unitOfWork.GetRepository<UploadPicture>();

            logger.LogInformation($"Get upload picture entity from repository for ID = {id}");
            var entityPicture = await repoUploadPicture.GetOneAsync(id);

            logger.LogInformation($"Set height and width for picture");
            entityPicture.UpdateSize(tuple.height, tuple.width);

            logger.LogInformation("Save changes");
            await unitOfWork.SaveChangesAsync();

            logger.LogInformation("Set status to read picture info end");
            entityStatus?.SetStatusEndInfo();
            await unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Set status to convert with failure");
            entityStatus?.ConversionFinishedWithError(ex.Message);
            await unitOfWork.SaveChangesAsync();
        }
    }

    public async Task ExtractPictureMetadata(long id, string destinationFilename)
    {
        logger.LogInformation("Action called with following parameters:");
        logger.LogDebug($"Destination Filename: {destinationFilename}");

        logger.LogInformation("Set status to extract metadata begin");
        var repoConvertStatus =
            unitOfWork.GetRepository<PictureConvertStatus>();
        var entityStatus =
            (await repoConvertStatus.GetAll(x => x.UploadPictureId == id)).FirstOrDefault();
        entityStatus?.SetStatusStartExif();
        await unitOfWork.SaveChangesAsync();

        try
        {
            logger.LogInformation("Get repository for upload picture from unit of work");
            var repoUploadPicture = unitOfWork.GetRepository<UploadPicture>();

            logger.LogInformation($"Get upload picture entity from repository for ID = {id}");
            var entityPicture = await repoUploadPicture.GetOneAsync(id);

            logger.LogInformation("Extract metadata");
            GetExifInfo(folderHelperService.GetFullFilenameForUploadPicture(destinationFilename), entityPicture);

            logger.LogInformation("Set status to extract metadata end");
            entityStatus?.SetStatusEndExif();
            await unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Set status to convert with failure");
            entityStatus?.ConversionFinishedWithError(ex.Message);
            await unitOfWork.SaveChangesAsync();
        }
    }

    public async Task ConvertPicture(long id, string destinationFilename)
    {
        logger.LogInformation("Action called with following parameters:");
        logger.LogDebug($"Destination Filename: {destinationFilename}");

        logger.LogInformation("Set status to convert begin");
        var repoConvertStatus =
            unitOfWork.GetRepository<PictureConvertStatus>();
        var entityStatus =
            (await repoConvertStatus.GetAll(x => x.UploadPictureId == id)).FirstOrDefault();
        entityStatus?.SetStatusStartConvert();
        await unitOfWork.SaveChangesAsync();

        try
        {
            logger.LogInformation(
                $"Converting picture to card format {appSettings.Value.CardSizeWidth} x {appSettings.Value.CardSizeHeight}");
            DoConvertion(destinationFilename, "png",
                appSettings.Value.CardSizeWidth, appSettings.Value.CardSizeHeight);

            logger.LogInformation(
                $"Converting picture to gallery format {appSettings.Value.GallerySizeWidth} x {appSettings.Value.GallerySizeHeight}");
            DoConvertion(destinationFilename, "png",
                appSettings.Value.GallerySizeWidth, appSettings.Value.GallerySizeHeight);

            logger.LogInformation(
                $"Converting picture to thumbnail gallery format {appSettings.Value.ThumbnailSizeWidth} x {appSettings.Value.ThumbnailSizeHeight}");
            DoConvertion(destinationFilename, "png",
                appSettings.Value.ThumbnailSizeWidth, appSettings.Value.ThumbnailSizeHeight);

            logger.LogInformation("Set status to convert end");
            entityStatus?.SetStatusEndConvert();
            await unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Set status to convert with failure");
            entityStatus?.ConversionFinishedWithError(ex.Message);
            await unitOfWork.SaveChangesAsync();
        }
    }

    public async Task UploadPictureToBlobStorage(long id, string destinationFilename)
    {
        logger.LogInformation("Action called with following parameters:");
        logger.LogDebug($"Destination Filename: {destinationFilename}");

        logger.LogInformation("Set status to upload blob begin");
        var repoConvertStatus =
            unitOfWork.GetRepository<PictureConvertStatus>();
        var entityStatus =
            (await repoConvertStatus.GetAll(x => x.UploadPictureId == id)).FirstOrDefault();
        entityStatus?.SetStatusStartUploadBlob();
        await unitOfWork.SaveChangesAsync();

        try
        {
            logger.LogInformation("Upload pictures to blob storage");
            await UploadFilesToBlobStorage(folderHelperService.GetFullFilenameForUploadPicture(destinationFilename));

            logger.LogInformation("Set status to upload blob end");
            entityStatus?.SetStatusEndUploadBlob();
            await unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Set status to convert with failure");
            entityStatus?.ConversionFinishedWithError(ex.Message);
            await unitOfWork.SaveChangesAsync();
        }
    }

    public async Task DeleteFilesFromPhysicalDrive(long id, string destinationFilename)
    {
        logger.LogInformation("Action called with following parameters:");
        logger.LogDebug($"Destination Filename: {destinationFilename}");

        logger.LogInformation("Set status to delete files begin");
        var repoConvertStatus =
            unitOfWork.GetRepository<PictureConvertStatus>();
        var entityStatus =
            (await repoConvertStatus.GetAll(x => x.UploadPictureId == id)).FirstOrDefault();
        entityStatus?.SetStatusStartDeleteFiles();
        await unitOfWork.SaveChangesAsync();

        try
        {
            logger.LogInformation("Delete files from physical drive");
            DeleteFiles(destinationFilename);

            logger.LogInformation("Set status to delete files end");
            entityStatus?.SetStatusEndDeleteFiles();
            await unitOfWork.SaveChangesAsync();

            logger.LogInformation("Set status to successfully finished");
            entityStatus?.ConversionFinished();
            await unitOfWork.SaveChangesAsync();

            logger.LogInformation("Set status to successfully finished");
            await mediator.Send(new MtrSetPictureStateConvertedCmd()
            {
                UploadPictureId = id
            });
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Set status to convert with failure");
            entityStatus?.ConversionFinishedWithError(ex.Message);
            await unitOfWork.SaveChangesAsync();
        }
    }
    #endregion
}