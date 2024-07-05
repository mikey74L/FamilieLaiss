using GraphQL;

namespace FamilieLaissGraphQLDataLayer.Requests.Mutations.Blog;

public static class BlogMutations
{
    public static GraphQLRequest GetAddBlogEntryMutation(string headerGerman, string headerEnglish, string textGerman, string textEnglish)
    {
        return new GraphQLRequest()
        {
            Query = @"
                mutation AddBlogEntry ($headerGerman: String!, $headerEnglish: String!, $textGerman: String!, $textEnglish: String!) {
                  addBlogEntry (input: {headerGerman: $headerGerman, headerEnglish: $headerEnglish, textGerman: $textGerman, textEnglish: $textEnglish}) {
                    blogEntry {
                      id
                      createDate
                    }
                  }
                }
            ",
            OperationName = "AddBlogEntry",
            Variables = new
            {
                headerGerman,
                headerEnglish,
                textGerman,
                textEnglish
            }
        };
    }

    public static GraphQLRequest GetUpdateBlogEntryMutation(long id, string headerGerman, string headerEnglish,
        string textGerman, string textEnglish)
    {
        return new GraphQLRequest()
        {
            Query = @"
                mutation UpdateBlogEntry ($id: Long!, $headerGerman: String!, $headerEnglish: String!, $textGerman: String!, $textEnglish: String!) {
                  updateBlogEntry (input: {id: $id, headerGerman: $headerGerman, headerEnglish: $headerEnglish, textGerman: $textGerman, textEnglish: $textEnglish}) {
                    blogEntry {
                      id
                      changeDate
                    }
                  }
                }
            ",
            OperationName = "UpdateBlogEntry",
            Variables = new
            {
                id,
                headerGerman,
                headerEnglish,
                textGerman,
                textEnglish
            }
        };
    }

    public static GraphQLRequest GetDeleteBlogEntryMutation(long id)
    {
        return new GraphQLRequest()
        {
            Query = @"
                mutation DeleteBlogEntry($id: Long!) {
                  deleteBlogEntry (input: {id: $id}) {
                    blogEntry {
                      id
                    }
                  }
                }            
            ",
            OperationName = "DeleteBlogEntry",
            Variables = new
            {
                id
            }
        };
    }
}
