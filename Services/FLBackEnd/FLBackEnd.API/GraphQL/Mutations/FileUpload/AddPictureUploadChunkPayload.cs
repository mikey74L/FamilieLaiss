namespace FLBackEnd.API.GraphQL.Mutations.FileUpload;

[GraphQLDescription("The result for a new added picture upload chunk")]
public class AddPictureUploadChunkPayload
{
    [GraphQLDescription("Has the chunk been successfully written to the server")]
    public bool Status { get; set; }
}