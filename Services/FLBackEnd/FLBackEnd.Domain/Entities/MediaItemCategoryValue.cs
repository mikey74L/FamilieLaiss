using DomainHelper.AbstractClasses;
using HotChocolate;
using Microsoft.EntityFrameworkCore;

namespace FLBackEnd.Domain.Entities;

/// <summary>
/// Entity for media item assigned category value
/// </summary>
[GraphQLDescription("Media item assigned category value")]
public class MediaItemCategoryValue : EntityCreation<long>
{
    #region Properties

    /// <summary>
    /// Identifier for the media item
    /// </summary>
    [GraphQLIgnore]
    public long MediaItemId { get; private set; }

    /// <summary>
    /// The media item this category value entry belongs to
    /// </summary>
    [GraphQLDescription("The media item this category value entry belongs to")]
    public MediaItem MediaItem { get; private set; }

    /// <summary>
    /// Identifier for the media item
    /// </summary>
    [GraphQLIgnore]
    public long CategoryValueId { get; private set; }

    /// <summary>
    /// The media item this category value entry belongs to
    /// </summary>
    [GraphQLDescription("The category value this media item belongs to")]
    public CategoryValue CategoryValue { get; private set; }

    #endregion

    #region C'tor

    /// <summary>
    /// Constructor
    /// </summary>
    protected MediaItemCategoryValue()
    {
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mediaItem">The media item the category value belongs to</param>
    /// <param name="valueID">The identifier for the category value</param>
    internal MediaItemCategoryValue(MediaItem mediaItem, long valueID)
    {
        MediaItem = mediaItem;
        CategoryValueId = valueID;
    }

    #endregion

    #region Overrides

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