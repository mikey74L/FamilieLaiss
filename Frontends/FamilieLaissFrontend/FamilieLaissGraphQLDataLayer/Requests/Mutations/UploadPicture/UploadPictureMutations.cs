using GraphQL;

namespace FamilieLaissGraphQLDataLayer.Requests.Mutations.UploadPicture;

public static class UploadPictureMutations
{
    public static GraphQLRequest GetDeleteUploadPictureMutation(long id)
    {
        return new GraphQLRequest()
        {
            Query = @"
                mutation DeleteUploadPicture ($id: Long!) {
                    deleteUploadPicture (input: {id: $id}) {
                        uploadPicture {
                            id
                        }
                    }
                }
            ",
            OperationName = "DeleteUploadPicture",
            Variables = new
            {
                id
            }
        };
    }

    public static GraphQLRequest GetDeleteAllUploadPicturesMutation()
    {
        return new GraphQLRequest()
        {
            Query = @"
                mutation DeleteAllUploadPictures {
                    deleteAllUploadPictures {
                        count
                    }
                }
            ",
            OperationName = "DeleteAllUploadPictures"
        };
    }
}
