namespace FLBackEnd.API.GraphQL.Mutations.UploadVideo;

[GraphQLDescription("InputData type for deleting upload video")]
public class DeleteUploadVideoInput
{
    [GraphQLDescription("The Id for the upload video")]
    public long Id { get; set; }
}
