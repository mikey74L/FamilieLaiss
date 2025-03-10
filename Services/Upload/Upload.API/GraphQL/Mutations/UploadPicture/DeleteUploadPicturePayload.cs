using HotChocolate;

namespace Upload.API.GraphQL.Mutations.UploadPicture;

[GraphQLDescription("The result for a deleted upload picture")]
public class DeleteUploadPicturePayload
{
    [GraphQLDescription("The deleted upload picture")]
    public Domain.Entities.UploadPicture UploadPicture { get; set; } = default!;
}