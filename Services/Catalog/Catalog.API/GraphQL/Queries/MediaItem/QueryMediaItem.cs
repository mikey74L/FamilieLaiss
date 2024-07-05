//using Catalog.Infrastructure.DBContext;
//using Microsoft.EntityFrameworkCore;

//namespace Catalog.API.GraphQL.Queries.MediaItem;

//[ExtendObjectType(typeof(Query))]
//public class QueryMediaItem
//{
//    [GraphQLDescription("Returns a list of media items")]
//    [UseProjection]
//    [HotChocolate.Data.UseFiltering]
//    [HotChocolate.Data.UseSorting]
//    public IQueryable<Catalog.Domain.Aggregates.MediaItem> GetMediaItems(CatalogServiceDbContext context)
//    {
//        return context.MediaItems;
//    }

//    [GraphQLDescription("Validate german media item name")]
//    public async Task<bool> GermanMediaItemNameExistsAsync(CatalogServiceDbContext context,
//        long iD, long mediaGroupId, string germanName)
//    {
//        var repository = context.MediaItems;

//        return await repository.AnyAsync(x => x.Id != iD && x.MediaGroupID == mediaGroupId && x.NameGerman == germanName);
//    }

//    [GraphQLDescription("Validate english media item name")]
//    public async Task<bool> EnglishMediaItemNameExistsAsync(CatalogServiceDbContext context,
//        long iD, long mediaGroupId, string englishName)
//    {
//        var repository = context.MediaItems;

//        return await repository.AnyAsync(x => x.Id != iD && x.MediaGroupID == mediaGroupId && x.NameEnglish == englishName);
//    }
//}
