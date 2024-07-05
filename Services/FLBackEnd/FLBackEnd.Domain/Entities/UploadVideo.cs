using DomainHelper.AbstractClasses;
using DomainHelper.Exceptions;
using FamilieLaissSharedObjects.Enums;
using FLBackEnd.Domain.ValueObjects;
using HotChocolate;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FLBackEnd.Domain.Entities;

/// <summary>
/// Entity for upload video 
/// </summary>
[GraphQLDescription("Upload video")]
public class UploadVideo : EntityCreation<long>
{
    #region Properties

    /// <summary>
    /// Original filename of the uploaded video
    /// </summary>
    [Required]
    [MaxLength(255)]
    [GraphQLDescription("The original filename for the video file that was uploaded")]
    public string Filename { get; private set; } = string.Empty;

    /// <summary>
    /// Status for the upload video
    /// </summary>
    [Required]
    [GraphQLDescription("The status for the upload video")]
    public EnumUploadStatus Status { get; private set; }

    /// <summary>
    /// Is video a streaming video (MPEG-DASH)
    /// </summary>
    [GraphQLDescription("The video type for the upload video")]
    public EnumVideoType VideoType { get; private set; }

    /// <summary>
    /// Height of the original video
    /// </summary>
    [GraphQLDescription("The height of the video")]
    public int Height { get; private set; }

    /// <summary>
    /// Width of the original video
    /// </summary>
    [GraphQLDescription("The width of the video")]
    public int Width { get; private set; }

    /// <summary>
    /// The hour part for the duration of this video
    /// </summary>
    [GraphQLDescription("The hour part for the duration of the video")]
    public int DurationHour { get; private set; }

    /// <summary>
    /// The minute part for the duration of this video
    /// </summary>
    [GraphQLDescription("The minute part for the duration of the video")]
    public int DurationMinute { get; private set; }

    /// <summary>
    /// The second part for the duration of this video
    /// </summary>
    [GraphQLDescription("The second part for the duration of the video")]
    public int DurationSecond { get; private set; }

    /// <summary>
    /// Longitude for GPS-Position
    /// </summary>
    [GraphQLDescription("The longitude for the gps position")]
    public double? GpsLongitude { get; private set; }

    /// <summary>
    /// Latitude for GPS-Position
    /// </summary>
    [GraphQLDescription("The latitude for the gps position")]
    public double? GpsLatitude { get; private set; }

    /// <summary>
    /// Google-Geo-Coding-Addresses for this Upload-Video as Value-Object
    /// </summary>
    [GraphQLDescription("The google geo coding address for this video")]
    public GoogleGeoCodingAddress? GoogleGeoCodingAddress { get; private set; }

    /// <summary>
    /// The assigned media item id
    /// </summary>
    [GraphQLIgnore]
    public long? MediaItemId { get; private set; }

    /// <summary>
    /// The assigned media item
    /// </summary>
    [GraphQLDescription("The assigned media item")]
    public MediaItem? MediaItem { get; private set; }


    /// <summary>
    /// The converting status for this upload video
    /// </summary>
    [GraphQLDescription("The converting status for this upload video")]
    public VideoConvertStatus ConvertStatus { get; private set; }

    #endregion

    #region C'tor

    /// <summary>
    /// Constructor 
    /// </summary>
    private UploadVideo()
    {
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="id">The ID for upload video</param>
    /// <param name="filename">The original filename for uploaded video</param>
    public UploadVideo(long id, string filename) : base()
    {
        Id = id;
        Filename = filename;
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
    /// <param name="longitude">GPS-Longitude for the video</param>
    /// <param name="latitude">GPS-Latitude for the video</param>
    [GraphQLIgnore]
    public void UpdateVideoInfo(EnumVideoType videoType, int height, int width, int durationHour, int durationMinute,
        int durationSecond,
        double? longitude, double? latitude)
    {
        if (height <= 0)
        {
            throw new DomainException("Height must be greater than 0");
        }

        if (width <= 0)
        {
            throw new DomainException("Width must be greater than 0");
        }

        if (durationHour == 0 && durationMinute == 0 && durationSecond == 0)
        {
            throw new DomainException("The duration must be at least one second");
        }

        if ((longitude.HasValue && !latitude.HasValue) || (!longitude.HasValue && latitude.HasValue))
        {
            throw new DomainException("GPS-Position exists of latitude and longitude");
        }

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
    /// Set state to successfully converted
    /// </summary>
    [GraphQLIgnore]
    public void SetVideoStateToConverted()
    {
        if (Height <= 0)
        {
            throw new DomainException("There must be a height before the status can be set to converted.");
        }

        if (Width <= 0)
        {
            throw new DomainException("There must be a width before the status can be set to converted.");
        }

        if (DurationHour == 0 && DurationMinute == 0 && DurationSecond == 0)
        {
            throw new DomainException("There must be a runtime hour before the status can be set to converted.");
        }

        if (VideoType == EnumVideoType.NotSet)
        {
            throw new DomainException("There must be a video type info before the status can be set to converted.");
        }

        Status = EnumUploadStatus.Converted;
    }

    /// <summary>
    /// Set state to assigned
    /// </summary>
    [GraphQLIgnore]
    public void SetVideoStateToAssigned()
    {
        if (Status != EnumUploadStatus.Converted)
        {
            throw new DomainException("Could only set the status to assigned from status converted");
        }

        Status = EnumUploadStatus.Assigned;
    }

    /// <summary>
    /// Set state to unassigned
    /// </summary>
    [GraphQLIgnore]
    public void SetVideoStateToUnAssigned()
    {
        if (Status != EnumUploadStatus.Assigned)
        {
            throw new DomainException("Could only set the status to unassigned from status assigned");
        }

        Status = EnumUploadStatus.Converted;
    }

    /// <summary>
    /// Set Google-Geo-Coding-Address data for upload video
    /// </summary>
    /// <param name="longitude">Longitude for GPS-Position</param>
    /// <param name="latitude">Latitude for GPS-Position</param>
    /// <param name="streetName">Street-Name</param>
    /// <param name="hnr">The house number</param>
    /// <param name="plz">The ZIP-Code</param>
    /// <param name="city">City-Name</param>
    /// <param name="country">Country-Name</param>
    [GraphQLIgnore]
    public void SetGeoCodingAddress(double longitude, double latitude, string streetName, string hnr, string plz,
        string city, string country)
    {
        if (GoogleGeoCodingAddress == null)
        {
            GoogleGeoCodingAddress =
                new GoogleGeoCodingAddress(longitude, latitude, streetName, hnr, plz, city, country);
        }
        else
        {
            throw new DomainException("There is already an existing Google-Geo-Coding-Address object");
        }
    }

    #endregion

    #region Called from Change-Tracker

    [GraphQLIgnore]
    public override Task EntityAddedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
    {
        return Task.CompletedTask;
    }

    [GraphQLIgnore]
    public override Task EntityDeletedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
    {
        return Task.CompletedTask;
    }

    #endregion
}