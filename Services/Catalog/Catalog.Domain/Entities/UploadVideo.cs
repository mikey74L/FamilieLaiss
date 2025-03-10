using Catalog.Domain.Aggregates;
using Catalog.Domain.DomainEvents.UploadVideo;
using DomainHelper.AbstractClasses;
using HotChocolate;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Catalog.Domain.Entities;

/// <summary>
/// Entity for representing the upload video
/// </summary>
public class UploadVideo : EntityCreation<long>
{
    #region Properties
    /// <summary>
    /// The original filename of the upload video with file extension
    /// </summary>
    [Required]
    [MaxLength(255)]
    [GraphQLIgnore]
    public string Filename { get; private set; } = string.Empty;

    /// <summary>
    /// Is the upload video assigned to a media item
    /// </summary>
    [Required]
    [GraphQLDescription("Is the upload video assigned to a media item")]
    public bool IsAssigned { get; private set; }

    /// <summary>
    /// The id of the media item the upload video is assigned to
    /// </summary>
    [GraphQLIgnore]
    public long? MediaItemId { get; private set; }

    /// <summary>
    /// The media item the upload videom is assigned to
    /// </summary>
    [GraphQLDescription("The media item the upload video is assigned to")]
    public MediaItem? MediaItem { get; private set; }

    #endregion

    #region C'tor

    /// <summary>
    /// Default constructor for GraphQL
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