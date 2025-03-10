using DomainHelper.AbstractClasses;
using HotChocolate;
using HotChocolate.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserInteraction.Domain.Aggregates;

public class MediaItem : EntityCreation<long>
{
    #region Private Members
    private readonly ILazyLoader lazyLoader;
    #endregion

    #region Properties
    /// <summary>
    /// List of related raitings
    /// </summary>
    [GraphQLDescription("List of related ratings")]
    [UseFiltering]
    [UseSorting]
    public ICollection<Rating> Ratings { get; private set; } = [];

    /// <summary>
    /// List of related comments
    /// </summary>
    [GraphQLDescription("List of related comments")]
    [UseFiltering]
    [UseSorting]
    public ICollection<Comment> Comments { get; private set; } = [];

    /// <summary>s
    /// List of related favorites
    /// </summary>
    [GraphQLDescription("List of related favorites")]
    [UseFiltering]
    [UseSorting]
    public ICollection<Favorite> Favorites { get; private set; } = [];
    #endregion

    #region C'tor
    /// <summary>
    /// C'tor (Called by Graphql)
    /// </summary>
    private MediaItem()
    {

    }

    /// <summary>
    /// C'tor (Called by EF.Core)
    /// </summary>
    /// <param name="lazyLoader">The EF.Core lazy loader. Will be injected by DI-Container</param>
    private MediaItem(ILazyLoader lazyLoader)
    {
        this.lazyLoader = lazyLoader;
    }
    #endregion

    public override Task EntityAddedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
    {
        return Task.CompletedTask;
    }

    public override Task EntityDeletedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
    {
        return Task.CompletedTask;
    }
}
