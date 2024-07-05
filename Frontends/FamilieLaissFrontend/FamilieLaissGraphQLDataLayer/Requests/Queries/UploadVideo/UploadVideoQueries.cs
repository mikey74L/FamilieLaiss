using GraphQL;

namespace FamilieLaissGraphQLDataLayer.Requests.Queries.UploadVideo;

public static class UploadVideoQueries
{
    public static GraphQLRequest GetAllUploadVideosQuery()
    {
        return new GraphQLRequest
        {
            Query = @"
                query GetUploadVideos {
                    uploadVideos (where: {status: {eq: CONVERTED}}) {
                        id
                        filename
                        height
                        width
                        duration_Hour
                        duration_Minute
                        duration_Second
                        status
                        videoType
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
