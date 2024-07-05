namespace FLBackEnd.API.GraphQL.Mutations.UploadPicture;

[GraphQLDescription("InputData type for deleting upload picture")]
public class DeleteUploadPictureInput
{
    [GraphQLDescription("The Id for the upload picture")]
    public long Id { get; set; }
}