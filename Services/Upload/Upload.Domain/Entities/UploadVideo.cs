using System.ComponentModel.DataAnnotations;
using DomainHelper.AbstractClasses;
using DomainHelper.Exceptions;
using FamilieLaissSharedObjects.Enums;
using System.Threading.Tasks;
using Upload.Domain.DomainEvents.UploadVideo;
using Upload.Domain.ValueObjects;

namespace Upload.Domain.Entities;

/// <summary>
/// Entity for upload video 
/// </summary>
public class UploadVideo : EntityCreation<long>
{
    #region Properties
    /// <summary>
    /// Original filename of the uploaded video
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string Filename { get; private set; } = string.Empty;

    /// <summary>
    /// Status for the upload video
    /// </summary>
    [Required]
    public EnumUploadStatus Status { get; private set; }

    /// <summary>
    /// Is video a streaming video (MPEG-DASH)
    /// </summary>
    public EnumVideoType? VideoType { get; private set; }

    /// <summary>
    /// Height of the original video
    /// </summary>
    public int? Height { get; private set; }

    /// <summary>
    /// Width of the original video
    /// </summary>
    public int? Width { get; private set; }

    /// <summary>
    /// The hour part for the duration of this video
    /// </summary>
    public int? DurationHour { get; private set; }

    /// <summary>
    /// The minute part for the duration of this video
    /// </summary>
    public int? DurationMinute { get; private set; }

    /// <summary>
    /// The second part for the duration of this video
    /// </summary>
    public int? DurationSecond { get; private set; }

    /// <summary>
    /// Longitude for GPS-Position
    /// </summary>
    public double? GpsLongitude { get; private set; }

    /// <summary>
    /// Latitude for GPS-Position
    /// </summary>
    public double? GpsLatitude { get; private set; }

    /// <summary>
    /// Google-Geo-Coding-Addresses for this Upload-Video as Value-Object
    /// </summary>
    public GoogleGeoCodingAddress? GoogleGeoCodingAdress { get; private set; }
    #endregion

    #region C'tor
    /// <summary>
    /// C'tor without parameters would be used by EF-Core
    /// </summary>
    protected UploadVideo()
    {

    }

    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="id">The ID for upload video</param>
    /// <param name="filename">The original filename for uploaded video</param>
    public UploadVideo(long id, string filename) : base()
    {
        //Übernehmen der ID
        Id = id;

        //Übernehmen des Dateinamens für das Upload-Video
        Filename = filename;

        //Setzen des initialen Status
        Status = EnumUploadStatus.Uploaded;
    }
    #endregion

    #region Methods
    /// <summary>
    /// Set the info for upload video
    /// </summary>
    /// <param name="videoType">The type of video after conversion</param>
    /// <param name="height">Height of the video</param>
    /// <param name="width">Width of the video</param>
    /// <param name="durationHour">Duration part for hours</param>
    /// <param name="durationMinute">Duration part for minutes</param>
    /// <param name="durationSecond">Duration part for seconds</param>
    /// <param name="GPS_Latitude"></param>
    /// <param name="GPS_Longitude"></param>
    public void UpdateVideoInfo(EnumVideoType videoType, int height, int width, int durationHour, int durationMinute, int durationSecond,
        double? longitude, double? latitude)
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

        //Überprüfen ob eine Laufzeit übermittelt wurde
        if (durationHour == 0 && durationMinute == 0 && durationSecond == 0)
        {
            throw new DomainException("The duration must be at least one second");
        }

        //Wenn eine GPS-Position angegeben ist, dann müssen Breiten und Längengrad angegeben sein
        if ((longitude.HasValue && !latitude.HasValue) || (!longitude.HasValue && latitude.HasValue))
        {
            throw new DomainException("GPS-Position exists of latitude and longitude");
        }

        //Setzen der Daten
        VideoType = videoType;
        Height = height;
        Width = width;
        DurationHour = durationHour;
        DurationMinute = durationMinute;
        DurationSecond = durationSecond;
        GpsLongitude = longitude;
        GpsLatitude = latitude;
    }

    /// <summary>
    /// Setzt den Status auf Video wurde convertiert
    /// </summary>
    public void SetVideoStateToConverted()
    {
        //Überprüfen ob eine Höhe gesetzt wurde bevor das Bild als konvertiert eingestuft wird
        if (Height <= 0)
        {
            throw new DomainException("There must be a height before the status can be set to converted.");
        }

        //Überprüfen ob eine Breite gesetzt wurde bevor das Bild als konvertiert eingestuft wird
        if (Width <= 0)
        {
            throw new DomainException("There must be a width before the status can be set to converted.");
        }

        //Überprüfen ob eine Laufzeit-Stunde gesetzt wurde
        if (!DurationHour.HasValue)
        {
            throw new DomainException("There must be a runtime hour before the status can be set to converted.");
        }

        //Überprüfen ob eine Laufzeit-Minute gesetzt wurde
        if (!DurationMinute.HasValue)
        {
            throw new DomainException("There must be a runtime minute before the status can be set to converted.");
        }

        //Überprüfen ob eine Laufzeit-Sekunde gesetzt wurde
        if (!DurationSecond.HasValue)
        {
            throw new DomainException("There must be a runtime second before the status can be set to converted.");
        }

        //Überprüfen ob die Information für Streaming Video gesetzt wurde
        if (!VideoType.HasValue)
        {
            throw new DomainException("There must be a video type info before the status can be set to converted.");
        }

        //Setzen des Status auf converted
        Status = EnumUploadStatus.Converted;
    }

    /// <summary>
    /// Set status to assigned
    /// </summary>
    public void SetVideoStateToAssigned()
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
    public void SetVideoStateToUnAssigned()
    {
        //Überprüfen ob der Status auf Converted sitzt
        if (Status != EnumUploadStatus.Assigned)
        {
            throw new DomainException("Could only set the status to unassigned from status assigned");
        }

        //Setzen des Status auf assigned
        Status = EnumUploadStatus.Converted;
    }

    /// <summary>
    /// Set Google-Geo-Coding-Adress data for upload video
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
        if (GoogleGeoCodingAdress == null)
        {
            GoogleGeoCodingAdress = new GoogleGeoCodingAddress(longitude, latitude, streetName, hnr, plz, city, country);
        }
        else
        {
            throw new DomainException("There is already an existing Google-Geo-Coding-Adress object");
        }
    }
    #endregion

    #region Called from Change-Tracker
    public override Task EntityAddedAsync()
    {
        AddDomainEvent(new DomainEventUploadVideoCreated(Id, Filename, Status));

        return Task.CompletedTask;
    }

    public override Task EntityDeletedAsync()
    {
        AddDomainEvent(new DomainEventUploadVideoDeleted(Id, VideoType));

        return Task.CompletedTask;
    }
    #endregion
}
