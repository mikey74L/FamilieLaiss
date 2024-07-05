using FLBackEnd.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace FLBackEnd.API.GraphQL.Queries.Media;

[ExtendObjectType(typeof(Query))]
public class GraphQlQueryMedia
{
    //[Authorize("MediaGroup.Read")]
    [GraphQLDescription("Returns a list of media groups")]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Domain.Entities.MediaGroup> GetMediaGroups(FamilieLaissDbContext context)
    {
        return context.MediaGroups;
    }

    //[Authorize("MediaItem.Read")]
    [GraphQLDescription("Returns a list of media items")]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Domain.Entities.MediaItem> GetMediaItems(FamilieLaissDbContext context)
    {
        return context.MediaItems;
    }

    //[Authorize("MediaGroup.Validate")]
    [GraphQLDescription("Validate german media group name ")]
    public async Task<bool> GermanMediaGroupNameExistsAsync(FamilieLaissDbContext context,
        long iD, string germanName)
    {
        var repository = context.MediaGroups;

        long count = await repository.CountAsync(x => x.Id != iD && x.NameGerman == germanName);

        return count > 0;
    }

    //[Authorize("MediaGroup.Validate")]
    [GraphQLDescription("Validate english media group name ")]
    public async Task<bool> EnglishMediaGroupNameExistsAsync(FamilieLaissDbContext context,
        long iD, string englishName)
    {
        var repository = context.MediaGroups;

        long count = await repository.CountAsync(x => x.Id != iD && x.NameEnglish == englishName);

        return count > 0;
    }

    //[Authorize("MediaItem.Validate")]
    [GraphQLDescription("Validate german media item name ")]
    public async Task<bool> GermanMediaItemNameExistsAsync(FamilieLaissDbContext context,
        long iD, long mediaGroupId, string germanName)
    {
        var repository = context.MediaItems;

        var count = await repository.CountAsync(x =>
            x.Id != iD && x.MediaGroupId == mediaGroupId && x.NameGerman == germanName);

        return count > 0;
    }

    //[Authorize("MediaItem.Validate")]
    [GraphQLDescription("Validate english media item name ")]
    public async Task<bool> EnglishMediaItemNameExistsAsync(FamilieLaissDbContext context,
        long iD, long mediaGroupId, string englishName)
    {
        var repository = context.MediaItems;

        var count = await repository.CountAsync(x =>
            x.Id != iD && x.MediaGroupId == mediaGroupId && x.NameEnglish == englishName);

        return count > 0;
    }
}
