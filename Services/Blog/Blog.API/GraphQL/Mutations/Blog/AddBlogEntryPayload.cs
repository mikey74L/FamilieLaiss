using Blog.Domain.Entities;
using HotChocolate;

namespace Blog.API.GraphQL.Mutations.Blog
{
    [GraphQLDescription("The result for a new added blog entry")]
    public class AddBlogEntryPayload
    {
        [GraphQLDescription("The new added blog entry")]
        public BlogEntry BlogEntry { get; set; }
    }
}
