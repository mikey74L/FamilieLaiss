using Blog.API.GraphQL.Types;
using Blog.Domain.Entities;
using Blog.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;

namespace Blog.API.GraphQL.Queries.Blog
{
    [ExtendObjectType(typeof(Query))]
    public class QueriesBlog
    {
        [GraphQLDescription("Returns a list of blog entries")]
        [UseProjection]
        [HotChocolate.Data.UseFiltering]
        [HotChocolate.Data.UseSorting]
        public IQueryable<BlogEntry> GetBlogEntries(BlogServiceDBContext context)
        {
            return context.BlogEntries;
        }

        [GraphQLDescription("Returns maximum and minimum date for allg blog entries")]
        public async Task<BlogMaxMinDateInfo> BlogGetMinAndMaxDate(BlogServiceDBContext context)
        {
            var result = new BlogMaxMinDateInfo();

            try
            {
                result.MinCreateDate = await context.BlogEntries.MinAsync(x => x.CreateDate);
                result.MaxCreateDate = await context.BlogEntries.MaxAsync(x => x.CreateDate);
            }
            catch
            {
                result.MinCreateDate = null;
                result.MaxCreateDate = null;
            }

            return result;
        }
    }
}
