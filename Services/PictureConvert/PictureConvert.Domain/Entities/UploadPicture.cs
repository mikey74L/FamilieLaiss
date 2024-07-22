using DomainHelper.AbstractClasses;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PictureConvert.Domain.Entities;

/// <summary>
/// Entity for representing the upload picture
/// </summary>
public class UploadPicture : EntityBase<long>
{
    #region Properties

    /// <summary>
    /// The original filename of the upload picture with file extension
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string Filename { get; private set; } = string.Empty;

    /// <summary>
    /// The converting status for this upload picture
    /// </summary>
    public PictureConvertStatus? Status { get; private set; }

    #endregion

    #region C'tor

    /// <summary>
    /// C'tor without parameters would be used by EF-Core
    /// </summary>
    private UploadPicture()
    {
    }

    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="id">The ID for the upload picture</param>
    /// <param name="filename">The original filename for the upload picture with extension</param>
    public UploadPicture(long id, string filename)
    {
        Id = id;
        Filename = filename;
    }

    #endregion

    #region Called from Change-Tracker

    public override Task EntityAddedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
    {
        return Task.CompletedTask;
    }

    public override Task EntityDeletedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
    {
        return Task.CompletedTask;
    }

    #endregion
}