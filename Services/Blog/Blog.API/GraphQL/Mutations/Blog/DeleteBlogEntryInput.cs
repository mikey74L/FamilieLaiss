using HotChocolate;

namespace Blog.API.GraphQL.Mutations.Blog
{
    [GraphQLDescription("Input type for deleting blog entries")]
    public class DeleteBlogEntryInput
    {
        [GraphQLDescription("The ID for the blog entry to delete")]
        public long ID { get; set; }
    }
}
