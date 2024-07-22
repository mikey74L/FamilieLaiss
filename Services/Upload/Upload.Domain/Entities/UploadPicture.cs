using DomainHelper.AbstractClasses;
using DomainHelper.Exceptions;
using FamilieLaissSharedObjects.Enums;
using HotChocolate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Upload.Domain.DomainEvents.UploadPicture;
using Upload.Domain.ValueObjects;

namespace Upload.Domain.Entities;

/// <summary>
/// Entity for upload picture 
/// </summary>
[GraphQLDescription("Upload picture")]
public class UploadPicture : EntityCreation<long>
{
    #region Private Properties

    private readonly ILazyLoader _lazyLoader;

    #endregion

    #region Properties

    /// <summary>
    /// The original filename for the picture file that was uploaded
    /// </summary>
    [Required]
    [MaxLength(255)]
    [GraphQLDescription("The original filename for the picture file that was uploaded")]
    public string Filename { get; private set; } = string.Empty;

    /// <summary>
    /// The original height of the picture
    /// </summary>
    [GraphQLDescription("The height of the picture")]
    public int Height { get; private set; }

    /// <summary>
    /// The original width of the picture
    /// </summary>
    [GraphQLDescription("The width of the picture")]
    public int Width { get; private set; }

    /// <summary>
    /// Status for the upload picture
    /// </summary>
    [Required]
    [GraphQLDescription("The status for the upload picture")]
    public EnumUploadStatus Status { get; private set; }

    /// <summary>
    /// The Exif-Info for this Upload-Picture
    /// </summary>
    [GraphQLDescription("The Exif-Info for the upload picture")]
    public UploadPictureExifInfo? UploadPictureExifInfo { get; private set; }

    /// <summary>
    /// Google-Geo-Coding-Addresses for this Upload-Picture as Value-Object
    /// </summary>
    [GraphQLDescription("Google geo coding address for this upload picture")]
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
    /// Constructor would be used by EF-Core
    /// </summary>
    // ReSharper disable once UnusedMember.Local
    private UploadPicture(ILazyLoader lazyLoader)
    {
        _lazyLoader = lazyLoader;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="id">The ID for upload picture</param>
    /// <param name="filename">The original filename for uploaded picture</param>
    public UploadPicture(long id, string filename)
    {
        Id = id;
        Filename = filename;
        Status = EnumUploadStatus.Uploaded;
    }

    #endregion

    #region Domain Methods

    /// <summary>
    /// Set the height and width for upload picture
    /// </summary>
    /// <param name="height">The height in pixel</param>
    /// <param name="width">The width in pixel</param>
    [GraphQLIgnore]
    public void UpdateSize(int height, int width)
    {
        if (height <= 0)
        {
            throw new DomainException("Height must be greater than 0");
        }

        if (width <= 0)
        {
            throw new DomainException("Width must be greater than 0");
        }

        Height = height;
        Width = width;
    }

    /// <summary>
    /// Set EXIF-Data for upload picture
    /// </summary>
    /// <param name="make">Make for the camera</param>
    /// <param name="model">Camera model</param>
    /// <param name="resolutionX">Resolution x</param>
    /// <param name="resolutionY">Resolution Y</param>
    /// <param name="resolutionUnit">The resolution unit</param>
    /// <param name="orientation">Orientation for picture</param>
    /// <param name="ddlRecorded">Timestamp when picture was recorded</param>
    /// <param name="exposureTime">Exposure-Time</param>
    /// <param name="exposureProgram">Exposure program</param>
    /// <param name="exposureMode">Exposure mode</param>
    /// <param name="fNumber">F-Number</param>
    /// <param name="isoSensitivity">ISO sensitivity</param>
    /// <param name="shutterSpeed">Shutter speed</param>
    /// <param name="meteringMode">Metering mode</param>
    /// <param name="flashMode">Flash mode</param>
    /// <param name="focalLength">Focal length</param>
    /// <param name="sensingMode">Sensing mode</param>
    /// <param name="whiteBalanceMode">White-Balance mode</param>
    /// <param name="sharpness">Sharpness</param>
    /// <param name="gpsLongitude">Longitude for GPS-Position</param>
    /// <param name="gpsLatitude">Latitude for GPS-Position</param>
    /// <param name="contrast">Contrast</param>
    /// <param name="saturation">Saturation</param>
    [GraphQLIgnore]
    public void SetExifData(string make, string model, double? resolutionX, double? resolutionY,
        short? resolutionUnit, short? orientation, DateTimeOffset? ddlRecorded, double? exposureTime,
        short? exposureProgram, short? exposureMode, double? fNumber,
        int? isoSensitivity, double? shutterSpeed, short? meteringMode, short? flashMode, double? focalLength,
        short? sensingMode, short? whiteBalanceMode, short? sharpness, double? gpsLongitude, double? gpsLatitude,
        short? contrast, short? saturation)
    {
        if (UploadPictureExifInfo == null)
        {
            UploadPictureExifInfo = new UploadPictureExifInfo(make, model, resolutionX, resolutionY, resolutionUnit,
                orientation, ddlRecorded, exposureTime, exposureProgram, exposureMode,
                fNumber, isoSensitivity, shutterSpeed, meteringMode, flashMode, focalLength, sensingMode,
                whiteBalanceMode, sharpness, gpsLongitude, gpsLatitude, contrast, saturation);
        }
        else
        {
            throw new DomainException("There is already an existing EXIF-Data value object");
        }
    }

    /// <summary>
    /// Set Google-Geo-Coding-Address data for upload picture
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

    /// <summary>
    /// Set status to converted
    /// </summary>
    [GraphQLIgnore]
    public void SetPictureStateToConverted()
    {
        if (Height is <= 0)
        {
            throw new DomainException("There must be a height before the status can be set to converted.");
        }

        if (Width is <= 0)
        {
            throw new DomainException("There must be a width before the status can be set to converted.");
        }

        Status = EnumUploadStatus.Converted;
    }

    /// <summary>
    /// Set status to assigned
    /// </summary>
    [GraphQLIgnore]
    public void SetPictureStateToAssigned()
    {
        if (Status != EnumUploadStatus.Converted)
        {
            throw new DomainException("Could only set the status to assigned from status converted");
        }

        Status = EnumUploadStatus.Assigned;
    }

    /// <summary>
    /// Set status to unassigned
    /// </summary>
    [GraphQLIgnore]
    public void SetPictureStateToUnAssigned()
    {
        if (Status != EnumUploadStatus.Assigned)
        {
            throw new DomainException("Could only set the status to unassigned from status assigned");
        }

        Status = EnumUploadStatus.Converted;
    }

    #endregion

    #region Called from Change-Tracker

    [GraphQLIgnore]
    public override Task EntityAddedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
    {
        AddDomainEvent(new DomainEventUploadPictureCreated(Id, Filename, Status));

        return Task.CompletedTask;
    }

    [GraphQLIgnore]
    public override Task EntityDeletedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
    {
        AddDomainEvent(new DomainEventUploadPictureDeleted(Id));

        return Task.CompletedTask;
    }

    #endregion
}