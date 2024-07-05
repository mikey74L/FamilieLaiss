using HotChocolate;

namespace Blog.API.GraphQL.Mutations.Blog
{
    [GraphQLDescription("Input type for adding blog entries")]
    public class AddBlogEntryInput
    {
        [GraphQLDescription("German header for this blog entry")]
        public string HeaderGerman { get; set; }

        [GraphQLDescription("English header for this blog entry")]
        public string HeaderEnglish { get; set; }
     
        [GraphQLDescription("German text for this blog entry")]
        public string TextGerman { get; set; }

        [GraphQLDescription("English text for this blog entry")]
        public string TextEnglish { get; set; }
    }
}
