using GraphQL;

namespace FamilieLaissGraphQLDataLayer.Requests.Queries.UploadOperations;

public static class UploadOperationsQueries
{
    public static GraphQLRequest GetNewUploadIDQuery()
    {
        return new GraphQLRequest
        {
            Query = @"
                query GetNewUploadID {
                    uploadID {
                        id
                    }
                }
            "
        };
    }
}
