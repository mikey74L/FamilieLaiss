using Blog.Domain.Entities;
using HotChocolate;

namespace Blog.API.GraphQL.Mutations.Blog
{
    [GraphQLDescription("The result for the deleted blog entry")]
    public class DeleteBlogEntryPayload
    {
        [GraphQLDescription("The deleted blog entry")]
        public BlogEntry BlogEntry { get; set; }
    }
}
