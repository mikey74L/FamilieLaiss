using DomainHelper.AbstractClasses;
using DomainHelper.Exceptions;
using FamilieLaissSharedObjects.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Upload.Domain.DomainEvents.UploadPicture;
using Upload.Domain.ValueObjects;

namespace Upload.Domain.Entities;

/// <summary>
/// Entity for upload picture 
/// </summary>
public class UploadPicture : EntityCreation<long>
{
    #region Properties
    /// <summary>
    /// The original filename for the picture file that was uploaded
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string Filename { get; private set; } = string.Empty;

    /// <summary>
    /// The original height of the picture
    /// </summary>
    public int? Height { get; private set; }

    /// <summary>
    /// The original width of the picture
    /// </summary>
    public int? Width { get; private set; }

    /// <summary>
    /// Status for the upload picture
    /// </summary>
    [Required]
    public EnumUploadStatus Status { get; private set; }

    /// <summary>
    /// The Exif-Info for this Upload-Picture
    /// </summary>
    public UploadPictureExifInfo? UploadPictureExifInfo { get; private set; }

    /// <summary>
    /// Google-Geo-Coding-Addresses for this Upload-Picture as Value-Object
    /// </summary>
    public GoogleGeoCodingAddress? GoogleGeoCodingAddress { get; private set; }
    #endregion

    #region C'tor
    /// <summary>
    /// C'tor without parameters would be used by EF-Core
    /// </summary>
    protected UploadPicture()
    {

    }

    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="id">The ID for upload picture</param>
    /// <param name="filename">The original filename for uploaded picture</param>
    public UploadPicture(long id, string filename) : base()
    {
        //Übernehmen der ID
        Id = id;

        //Übernehmen des Dateinamens für das Upload-Picture
        Filename = filename;

        //Setzen des initialen Status
        Status = EnumUploadStatus.Uploaded;
    }
    #endregion

    #region Domain Methods
    /// <summary>
    /// Set the height and width for upload picture
    /// </summary>
    /// <param name="height">The height in pixel</param>
    /// <param name="width">The width in pixel</param>
    public void UpdateSize(int height, int width)
    {
        //Überprüfen ob die Höhe größer als 0 ist
        if (height <= 0)
        {
            throw new DomainException("Height must be greater than 0");
        }

        //Überprüfen ob die Breite größer als 0 ist
        if (width <= 0)
        {
            throw new DomainException("Width must be greater than 0");
        }

        //Setzen der Daten
        Height = height;
        Width = width;
    }

    /// <summary>
    /// Set EXIF-Data for upload picture
    /// </summary>
    /// <param name="make">Make for the camera</param>
    /// <param name="model">Camera model</param>
    /// <param name="resolution_X">Resolution x</param>
    /// <param name="resolution_Y">Resolution Y</param>
    /// <param name="resolution_Unit">The resolution unit</param>
    /// <param name="orientation">Orientation for picture</param>
    /// <param name="ddl_Recorded">Timestamp when picture was recorded</param>
    /// <param name="exposure_Time">Exposure-Time</param>
    /// <param name="exposure_Program">Exposure program</param>
    /// <param name="exposure_Mode">Exposure mode</param>
    /// <param name="f_Number">F-Number</param>
    /// <param name="iso_Sensitivity">ISO sensitivity</param>
    /// <param name="shutter_Speed">Shutter speed</param>
    /// <param name="metering_Mode">Metering mode</param>
    /// <param name="flash_Mode">Flash mode</param>
    /// <param name="focal_Length">Focal length</param>
    /// <param name="sensing_Mode">Sensing mode</param>
    /// <param name="white_Balance_Mode">White-Balance mode</param>
    /// <param name="sharpness">Sharpness</param>
    /// <param name="gps_Longitute">Longitutde for GPS-Position</param>
    /// <param name="gps_Latitude">Latitude for GPS-Position</param>
    /// <param name="contrast">Contrast</param>
    /// <param name="saturation">Saturation</param>
    public void SetExifData(string make, string model, double? resolution_X, double? resolution_Y,
        string resolution_Unit, Int16? orientation, DateTimeOffset? ddl_Recorded, string exposure_Time,
        Int16? exposure_Program, Int16? exposure_Mode, double? f_Number,
        int? iso_Sensitivity, double? shutter_Speed, Int16? metering_Mode, Int16? flash_Mode, double? focal_Length,
        Int16? sensing_Mode, Int16? white_Balance_Mode, Int16? sharpness, double? gps_Longitute, double? gps_Latitude,
        Int16? contrast, Int16? saturation)
    {
        //Prüfen ob es schon ein Exif-Value-Objekt gibt
        if (UploadPictureExifInfo == null)
        {
            UploadPictureExifInfo = new UploadPictureExifInfo(make, model, resolution_X, resolution_Y, resolution_Unit,
                orientation, ddl_Recorded, exposure_Time, exposure_Program, exposure_Mode,
                f_Number, iso_Sensitivity, shutter_Speed, metering_Mode, flash_Mode, focal_Length, sensing_Mode,
                white_Balance_Mode, sharpness, gps_Longitute, gps_Latitude, contrast, saturation);
        }
        else
        {
            throw new DomainException("There is already an existing EXIF-Data value object");
        }
    }

    /// <summary>
    /// Set Google-Geo-Coding-Adress data for upload picture
    /// </summary>
    /// <param name="longitude">Longitude for GPS-Position</param>
    /// <param name="latitude">Latitude for GPS-Position</param>
    /// <param name="streetName">Street-Name</param>
    /// <param name="hnr">The house number</param>
    /// <param name="plz">The ZIP-Code</param>
    /// <param name="city">City-Name</param>
    /// <param name="country">Country-Name</param>
    public void SetGeoCodingAdress(double longitude, double latitude, string streetName, string hnr, string plz, string city, string country)
    {
        if (GoogleGeoCodingAddress == null)
        {
            GoogleGeoCodingAddress = new GoogleGeoCodingAddress(longitude, latitude, streetName, hnr, plz, city, country);
        }
        else
        {
            throw new DomainException("There is already an existing Google-Geo-Coding-Adress object");
        }
    }

    /// <summary>
    /// Set status to converted
    /// </summary>
    public void SetPictureStateToConverted()
    {
        //Überprüfen ob eine Höhe gesetzt wurde bevor das Bild als konvertiert eingestuft wird
        if (!Height.HasValue || Height.Value <= 0)
        {
            throw new DomainException("There must be a height before the status can be set to converted.");
        }

        //Überprüfen ob eine Breite gesetzt wurde bevor das Bild als konvertiert eingestuft wird
        if (!Width.HasValue || Width.Value <= 0)
        {
            throw new DomainException("There must be a width before the status can be set to converted.");
        }

        //Setzen des Status auf converted
        Status = EnumUploadStatus.Converted;
    }

    /// <summary>
    /// Set status to assigned
    /// </summary>
    public void SetPictureStateToAssigned()
    {
        //Überprüfen ob der Status auf Converted sitzt
        if (Status != EnumUploadStatus.Converted)
        {
            throw new DomainException("Could only set the status to assigned from status converted");
        }

        //Setzen des Status auf assigned
        Status = EnumUploadStatus.Assigned;
    }

    /// <summary>
    /// Set status to unassigned
    /// </summary>
    public void SetPictureStateToUnAssigned()
    {
        //Überprüfen ob der Status auf Assigned sitzt
        if (Status != EnumUploadStatus.Assigned)
        {
            throw new DomainException("Could only set the status to unassigned from status assigned");
        }

        //Setzen des Status auf assigned
        Status = EnumUploadStatus.Converted;
    }
    #endregion

    #region Called from Change-Tracker
    public override Task EntityAddedAsync()
    {
        AddDomainEvent(new DomainEventUploadPictureCreated(Id, Filename, Status));

        return Task.CompletedTask;
    }

    public override Task EntityDeletedAsync()
    {
        AddDomainEvent(new DomainEventUploadPictureDeleted(Id));

        return Task.CompletedTask;
    }
    #endregion
}
