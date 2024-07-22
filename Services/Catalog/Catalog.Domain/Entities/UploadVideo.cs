using Catalog.Domain.Aggregates;
using Catalog.Domain.DomainEvents.UploadVideo;
using DomainHelper.AbstractClasses;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Catalog.Domain.Entities;

/// <summary>
/// Entity for representing the upload video
/// </summary>
public class UploadVideo : EntityBase<long>
{
    #region Properties

    /// <summary>
    /// The original filename of the upload video with file extension
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string Filename { get; private set; }

    /// <summary>
    /// Is the upload video assigned to a media item
    /// </summary>
    [Required]
    public bool IsAssigned { get; private set; }

    public long? MediaItemId { get; private set; }

    public MediaItem? MediaItem { get; private set; }

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
    /// <param name="id">The ID for the upload video</param>
    /// <param name="filename">The original filename for the upload video with extension</param>
    public UploadVideo(long id, string filename)
    {
        Id = id;
        Filename = filename;
        IsAssigned = false;
    }

    #endregion

    #region Domain Methods

    public void SetVideoStateToAssigned(long mediaItemId)
    {
        MediaItemId = mediaItemId;
        IsAssigned = true;
    }

    public void SetVideoStateToUnAssigned()
    {
        MediaItemId = null;
        IsAssigned = false;
    }

    #endregion

    #region Overrides

    public override Task EntityAddedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
    {
        AddDomainEvent(new DomainEventUploadVideoCreated(Id));

        return Task.CompletedTask;
    }

    public override Task EntityDeletedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
    {
        AddDomainEvent(new DomainEventUploadVideoDeleted(Id));

        return Task.CompletedTask;
    }

    #endregion
}