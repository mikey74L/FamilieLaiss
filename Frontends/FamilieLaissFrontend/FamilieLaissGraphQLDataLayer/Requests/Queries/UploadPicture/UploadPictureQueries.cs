using GraphQL;

namespace FamilieLaissGraphQLDataLayer.Requests.Queries.UploadPicture;

public static class UploadPictureQueries
{
    public static GraphQLRequest GetAllUploadPicturesQuery()
    {
        return new GraphQLRequest
        {
            Query = @"
                query GetUploadPictures {
                    uploadPictures (where: {status: {eq: CONVERTED}}) {
                        id
                        filename
                        height
                        width
                        status
                        createDate   
                    }
                }
            "
        };
    }

    public static GraphQLRequest GetUploadPicturesCountQuery()
    {
        return new GraphQLRequest
        {
            Query = @"
                query GetUploadPictureCount {
                  uploadPictureCount
                }
            "
        };
    }
}
