using Blog.Domain.Entities;
using HotChocolate;

namespace Blog.API.GraphQL.Mutations.Blog
{
    [GraphQLDescription("The result for the updated blog entry")]
    public class UpdateBlogEntryPayload
    {
        [GraphQLDescription("The updated blog entry")]
        public BlogEntry BlogEntry { get; set; }
    }
}
