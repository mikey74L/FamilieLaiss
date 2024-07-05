using GraphQL;

namespace FamilieLaissGraphQLDataLayer.Requests.Mutations.UploadVideo;

public static class UploadVideoMutations
{
    public static GraphQLRequest GetDeleteUploadVideoMutation(long id)
    {
        return new GraphQLRequest()
        {
            Query = @"
                mutation DeleteUploadVideo ($id: Long!) {
                    deleteUploadVideo (input: {id: $id}) {
                        uploadVideo {
                            id
                        }
                    }
                }
            ",
            OperationName = "GetDeleteUploadVideoMutation",
            Variables = new
            {
                id
            }
        };
    }

    public static GraphQLRequest GetDeleteAllUploadVideosMutation()
    {
        return new GraphQLRequest()
        {
            Query = @"
                mutation DeleteAllUploadVideos {
                    deleteAllUploadVideos {
                        count
                    }
                }            
            ",
            OperationName = "DeleteAllUploadVideos"
        };
    }
}
