using DomainHelper.AbstractClasses;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace VideoConvert.Domain.Entities;

/// <summary>
/// Entity for representing the upload video
/// </summary>
public class UploadVideo : EntityBase<long>
{
    #region Properties

    /// <summary>
    /// Original filename of the uploaded video
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string Filename { get; private set; } = string.Empty;

    /// <summary>
    /// Height of the original video
    /// </summary>
    public int Height { get; private set; }

    /// <summary>
    /// Width of the original video
    /// </summary>
    public int Width { get; private set; }

    /// <summary>
    /// The converting status for this upload video
    /// </summary>
    public VideoConvertStatus ConvertStatus { get; private set; }

    #endregion

    #region C'tor

    /// <summary>
    /// C'tor without parameters would be used by EF-Core
    /// </summary>
    private UploadVideo()
    {
    }

    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="id">The ID for the upload picture</param>
    /// <param name="filename">The original filename for the upload picture with extension</param>
    public UploadVideo(long id, string filename)
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