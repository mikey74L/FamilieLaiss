using HotChocolate;

namespace Upload.API.GraphQL.Mutations.UploadPicture
{
    [GraphQLDescription("The result for all deleted upload pictures")]
    public class DeleteAllUploadPicturesPayload
    {
        [GraphQLDescription("The count of deleted upload pictures")]
        public int Count { get; set; }
    }
}
