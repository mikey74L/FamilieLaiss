namespace FLBackEnd.API.GraphQL.Mutations.UploadPicture;

[GraphQLDescription("The result for all deleted upload pictures")]
public class DeleteAllUploadPicturePayload
{
    [GraphQLDescription("The list of deleted upload pictures")]
    public List<Domain.Entities.UploadPicture> UploadPictures { get; set; } = [];
}
