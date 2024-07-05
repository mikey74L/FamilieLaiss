using GraphQL;

namespace FamilieLaissGraphQLDataLayer.Requests.Queries.Blog;

public static class BlogQueries
{
    public static GraphQLRequest GetAllBlogEntriesQuery()
    {
        return new GraphQLRequest
        {
            Query = @"
                query GetAllBlogEntries {
                  blogEntries {
                      id
                      headerGerman
                      headerEnglish
                      textGerman
                      textEnglish
                      createDate
                      changeDate
                  }
                }
            ",
            OperationName = "GetAllBlogEntries"
        };
    }

    public static GraphQLRequest GetAllBlogEntriesFilterQuery(DateTimeOffset minDate, DateTimeOffset maxDate)
    {
        return new GraphQLRequest
        {
            Query = @"
                query GetAllBlogEntriesFilter ($minDate: DateTime!, $maxDate: DateTime!) {
                  blogEntries ( where: 
                              { and: [
                              { createDate: { gte: $minDate}},
                              { createDate: { lte: $maxDate}}]}) {
                      id
                      headerGerman
                      headerEnglish
                      textGerman
                      textEnglish
                      createDate
                      changeDate
                  }
                }
            ",
            OperationName = "GetAllBlogEntriesFilter",
            Variables = new
            {
                minDate,
                maxDate
            }
        };
    }

    public static GraphQLRequest GetBlogEntryQuery(long id)
    {
        return new GraphQLRequest
        {
            Query = @"
                query GetBlogEntry ($id: Long!) {
                  blogEntries (where: {id: {eq: $id}}) {
                      id
                      headerGerman
                      headerEnglish
                      textGerman
                      textEnglish
                      createDate
                      changeDate
                  }
                }
            ",
            OperationName = "GetBlogEntry",
            Variables = new
            {
                id
            }
        };
    }

    public static GraphQLRequest GetBlogGetMinMaxDateQuery()
    {
        return new GraphQLRequest
        {
            Query = @"
                query BlogGetMinMaxDate {
                    blogGetMinAndMaxDate {
                        minCreateDate
                        maxCreateDate
                    }
                }
            ",
            OperationName = "BlogGetMinMaxDate"
        };
    }
}
