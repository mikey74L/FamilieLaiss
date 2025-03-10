using HotChocolate;

namespace Upload.API.GraphQL.Mutations.UploadPicture;

[GraphQLDescription("InputData type for deleting all upload pictures")]
public class DeleteAllUploadPictureInput
{
    [GraphQLDescription("The Id list for the upload pictures to delete")]
    public List<long> UploadPictureIds { get; set; } = [];
}