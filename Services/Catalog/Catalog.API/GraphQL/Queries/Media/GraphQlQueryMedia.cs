using Catalog.Domain.Aggregates;
using Catalog.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.GraphQL.Queries.Media;

[ExtendObjectType(typeof(Query))]
public class GraphQlQueryMedia
{
    //[Authorize("MediaGroup.Read")]
    [GraphQLDescription("Returns a list of media groups")]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<MediaGroup> GetMediaGroups(CatalogServiceDbContext context)
    {
        return context.MediaGroups;
    }

    //[Authorize("MediaItem.Read")]
    [GraphQLDescription("Returns a list of media items")]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<MediaItem> GetMediaItems(CatalogServiceDbContext context)
    {
        return context.MediaItems;
    }

    //[Authorize("MediaGroup.Validate")]
    [GraphQLDescription("Validate german media group name ")]
    public async Task<bool> GermanMediaGroupNameExistsAsync(CatalogServiceDbContext context,
        long iD, string germanName)
    {
        var repository = context.MediaGroups;

        long count = await repository.CountAsync(x => x.Id != iD && x.NameGerman == germanName);

        return count > 0;
    }

    //[Authorize("MediaGroup.Validate")]
    [GraphQLDescription("Validate english media group name ")]
    public async Task<bool> EnglishMediaGroupNameExistsAsync(CatalogServiceDbContext context,
        long iD, string englishName)
    {
        var repository = context.MediaGroups;

        long count = await repository.CountAsync(x => x.Id != iD && x.NameEnglish == englishName);

        return count > 0;
    }

    //[Authorize("MediaItem.Validate")]
    [GraphQLDescription("Validate german media item name ")]
    public async Task<bool> GermanMediaItemNameExistsAsync(CatalogServiceDbContext context,
        long iD, long mediaGroupId, string germanName)
    {
        var repository = context.MediaItems;

        var count = await repository.CountAsync(x =>
            x.Id != iD && x.MediaGroupId == mediaGroupId && x.NameGerman == germanName);

        return count > 0;
    }

    //[Authorize("MediaItem.Validate")]
    [GraphQLDescription("Validate english media item name ")]
    public async Task<bool> EnglishMediaItemNameExistsAsync(CatalogServiceDbContext context,
        long iD, long mediaGroupId, string englishName)
    {
        var repository = context.MediaItems;

        var count = await repository.CountAsync(x =>
            x.Id != iD && x.MediaGroupId == mediaGroupId && x.NameEnglish == englishName);

        return count > 0;
    }
}