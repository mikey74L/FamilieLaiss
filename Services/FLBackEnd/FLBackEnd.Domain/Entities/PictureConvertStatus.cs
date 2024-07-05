using DomainHelper.AbstractClasses;
using DomainHelper.Exceptions;
using FamilieLaissSharedObjects.Enums;
using HotChocolate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace FLBackEnd.Domain.Entities;

/// <summary>
/// Entity for representing the convert status of an upload picture
/// </summary>
[GraphQLDescription("Picture convert status")]
public class PictureConvertStatus : EntityBase<long>
{
    #region Properties

    /// <summary>
    /// The current state for the conversion
    /// </summary>
    [GraphQLDescription("The current state for the conversion")]
    public EnumPictureConvertStatus Status { get; private set; }

    /// <summary>
    /// Error message when conversion is in state "Finished with errors"
    /// </summary>
    [GraphQLDescription("Error message when conversion is in state \"Finished with errors\"")]
    [MaxLength(2000)]
    public string ErrorMessage { get; private set; } = string.Empty;

    /// <summary>
    /// Timestamp for "Read Picture Info from file - Begin"
    /// </summary>
    [GraphQLDescription("Timestamp for \"Read Picture Info from file - Begin\"")]
    public DateTimeOffset? StartDateInfo { get; private set; }

    /// <summary>
    /// Timestamp for "Read Picture Info from file - End"
    /// </summary>
    [GraphQLDescription("Timestamp for \"Read Picture Info from file - End\"")]
    public DateTimeOffset? FinishDateInfo { get; private set; }

    /// <summary>
    /// Timestamp for "Read Exif-Information from file - Begin"
    /// </summary>
    [GraphQLDescription("Timestamp for \"Read Exif-Information from file - Begin\"")]
    public DateTimeOffset? StartDateExif { get; private set; }

    /// <summary>
    /// Timestamp for "Read Exif-Information from file - End"
    /// </summary>
    [GraphQLDescription("Timestamp for \"Read Exif-Information from file - End\"")]
    public DateTimeOffset? FinishDateExif { get; private set; }

    /// <summary>
    /// Timestamp for "Convert picture - Begin"
    /// </summary>
    [GraphQLDescription("Timestamp for \"Convert picture - Begin\"")]
    public DateTimeOffset? StartDateConvert { get; private set; }

    /// <summary>
    /// Timestamp for "Convert picture - End"
    /// </summary>
    [GraphQLDescription("Timestamp for \"Convert picture - End\"")]
    public DateTimeOffset? FinishDateConvert { get; private set; }

    /// <summary>
    /// The related upload picture for this status
    /// </summary>
    [GraphQLDescription("The upload picture that is converted")]
    public UploadPicture UploadPicture { get; private set; } = default!;

    [GraphQLIgnore] public long UploadPictureId { get; private set; }

    #endregion

    #region Private Members

    private readonly ILazyLoader? _lazyLoader;

    #endregion

    #region C'tor

    /// <summary>
    /// Constructor without parameters would be used by EF-Core
    /// </summary>
    private PictureConvertStatus(ILazyLoader lazyLoader)
    {
        _lazyLoader = lazyLoader;
    }

    private PictureConvertStatus()
    {
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="uploadPicture">The Upload-Picture this status item belongs too</param>
    public PictureConvertStatus(UploadPicture uploadPicture)
    {
        UploadPicture = uploadPicture;

        Status = EnumPictureConvertStatus.WaitingForConversion;
    }

    #endregion

    #region Domain Methods

    [GraphQLIgnore]
    public void SetStatusStartInfo()
    {
        Status = EnumPictureConvertStatus.ReadInfoBegin;

        StartDateInfo = DateTimeOffset.UtcNow;
    }

    [GraphQLIgnore]
    public void SetStatusEndInfo()
    {
        if (!StartDateInfo.HasValue)
        {
            throw new DomainException("No start date for info exists.");
        }

        Status = EnumPictureConvertStatus.ReadInfoEnd;

        FinishDateInfo = DateTimeOffset.UtcNow;
    }

    [GraphQLIgnore]
    public void SetStatusStartExif()
    {
        Status = EnumPictureConvertStatus.ReadExifBegin;

        StartDateExif = DateTimeOffset.UtcNow;
    }

    [GraphQLIgnore]
    public void SetStatusEndExif()
    {
        if (!StartDateExif.HasValue)
        {
            throw new DomainException("No start date for exif exists");
        }

        Status = EnumPictureConvertStatus.ReadExifEnd;

        FinishDateExif = DateTimeOffset.UtcNow;
    }

    [GraphQLIgnore]
    public void SetStatusStartConvert()
    {
        Status = EnumPictureConvertStatus.ConvertBegin;

        StartDateConvert = DateTimeOffset.UtcNow;
    }

    [GraphQLIgnore]
    public void SetStatusEndConvert()
    {
        if (!StartDateConvert.HasValue)
        {
            throw new DomainException("No start date for convert exists");
        }

        Status = EnumPictureConvertStatus.ConvertEnd;

        FinishDateConvert = DateTimeOffset.UtcNow;
    }

    [GraphQLIgnore]
    public async Task ConversionFinished()
    {
        if (!StartDateInfo.HasValue)
        {
            throw new DomainException("No start date for info exists.");
        }

        if (!FinishDateInfo.HasValue)
        {
            throw new DomainException("No finish date for info exists.");
        }

        if (!StartDateExif.HasValue)
        {
            throw new DomainException("No start date for exif exists");
        }

        if (!FinishDateExif.HasValue)
        {
            throw new DomainException("No finish date for exif exists");
        }

        if (!StartDateConvert.HasValue)
        {
            throw new DomainException("No start date for convert exists");
        }

        if (!FinishDateConvert.HasValue)
        {
            throw new DomainException("No finish date for convert exists");
        }

        Status = EnumPictureConvertStatus.SucessfullyConverted;
    }

    [GraphQLIgnore]
    public async Task ConversionFinishedWithError(string errorMessage)
    {
        Status = EnumPictureConvertStatus.ConvertedWithErrors;

        ErrorMessage = errorMessage;
    }

    [GraphQLIgnore]
    public void TransientError(string errorMessage)
    {
        Status = EnumPictureConvertStatus.TransientError;

        ErrorMessage = errorMessage;
    }

    #endregion

    #region called from Change-Tracker

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