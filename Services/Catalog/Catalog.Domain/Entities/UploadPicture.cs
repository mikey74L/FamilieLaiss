using Catalog.Domain.Aggregates;
using Catalog.Domain.DomainEvents.UploadPicture;
using DomainHelper.AbstractClasses;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Catalog.Domain.Entities;

/// <summary>
/// Entity for representing the upload picture
/// </summary>
public class UploadPicture : EntityCreation<long>
{
    #region Properties

    /// <summary>
    /// The original filename of the upload picture with file extension
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string Filename { get; private set; } = string.Empty;

    /// <summary>
    /// Is the upload picture assigned to a media item
    /// </summary>
    [Required]
    public bool IsAssigned { get; private set; }

    /// <summary>
    /// The original filename of the upload picture with file extension
    /// </summary>
    public long? MediaItemId { get; private set; }

    public MediaItem? MediaItem { get; private set; }

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

    #region Domain Methods

    public void SetPictureStateToAssigned(long mediaItemId)
    {
        MediaItemId = mediaItemId;
        IsAssigned = true;
    }

    public void SetPictureStateToUnAssigned()
    {
        MediaItemId = null;
        IsAssigned = false;
    }

    #endregion

    #region Overrides

    public override Task EntityAddedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
    {
        AddDomainEvent(new DomainEventUploadPictureCreated(Id));

        return Task.CompletedTask;
    }

    public override Task EntityDeletedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
    {
        AddDomainEvent(new DomainEventUploadPictureDeleted(Id));

        return Task.CompletedTask;
    }

    #endregion
}