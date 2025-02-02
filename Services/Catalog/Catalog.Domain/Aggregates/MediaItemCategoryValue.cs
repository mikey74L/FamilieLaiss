using DomainHelper.AbstractClasses;
using HotChocolate;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Domain.Aggregates;

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
    public MediaItem MediaItem { get; private set; } = null!;

    /// <summary>
    /// Identifier for the media item
    /// </summary>
    [GraphQLIgnore]
    public long CategoryValueId { get; private set; }

    /// <summary>
    /// The media item this category value entry belongs to
    /// </summary>
    [GraphQLDescription("The category value this media item belongs to")]
    public CategoryValue CategoryValue { get; private set; } = null!;

    #endregion

    #region C'tor

    /// <summary>
    /// Default constructor for GraphQL
    /// </summary>
    private MediaItemCategoryValue()
    {
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mediaItem">The media item the category value belongs to</param>
    /// <param name="valueId">The identifier for the category value</param>
    internal MediaItemCategoryValue(MediaItem mediaItem, long valueId)
    {
        MediaItem = mediaItem;
        CategoryValueId = valueId;
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