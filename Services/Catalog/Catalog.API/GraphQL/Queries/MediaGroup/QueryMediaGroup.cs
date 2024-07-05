//using Catalog.API.Models;
//using Catalog.Infrastructure.DBContext;
//using FamilieLaissSharedObjects.Enums;
//using Microsoft.EntityFrameworkCore;

//namespace Catalog.API.GraphQL.Queries.MediaGroup
//{
//    [ExtendObjectType(typeof(Query))]
//    //[Authorize(Policy = "MediaGroup.Read")]
//    public class QueryMediaGroup
//    {
//        [GraphQLDescription("Returns a list of media groups")]
//        [UseProjection]
//        [HotChocolate.Data.UseFiltering]
//        [HotChocolate.Data.UseSorting]
//        public IQueryable<Catalog.Domain.Aggregates.MediaGroup> GetMediaGroups(CatalogServiceDbContext context)
//        {
//            return context.MediaGroups;
//        }

//        [GraphQLDescription("Returns a list for the count of assigned media items per media group")]
//        public IEnumerable<MediaGroupMediaItemCountInfo> GetMediaItemCountForMediaGroups(CatalogServiceDbContext context,
//            EnumMediaType enumMediaType)
//        {
//            var result = new List<MediaGroupMediaItemCountInfo>();

//            var groupedItems = context.MediaItems.Where(x => x.MediaType == enumMediaType ||
//              enumMediaType == EnumMediaType.Both).GroupBy(x => x.MediaGroupID);

//            foreach (var item in groupedItems)
//            {
//                result.Add(new() { MediaGroupId = item.Key, Count = item.Count() });
//            }

//            return result;
//        }

//        [GraphQLDescription("Validate german media group name")]
//        //[Authorize(Policy = "MediaGroup.Validate")]
//        public async Task<bool> GermanMediaGroupNameExistsAsync(CatalogServiceDbContext context,
//        long iD, string germanName)
//        {
//            var repository = context.MediaGroups;

//            var count = await repository.CountAsync(x => x.Id != iD && x.NameGerman == germanName);

//            return count > 0;
//        }

//        [GraphQLDescription("Validate english media group name")]
//        //[Authorize(Policy = "MediaGroup.Validate")]
//        public async Task<bool> EnglishMediaGroupNameExistsAsync(CatalogServiceDbContext context,
//            long iD, string englishName)
//        {
//            var repository = context.MediaGroups;

//            var count = await repository.CountAsync(x => x.Id != iD && x.NameEnglish == englishName);

//            return count > 0;
//        }
//    }
//}
