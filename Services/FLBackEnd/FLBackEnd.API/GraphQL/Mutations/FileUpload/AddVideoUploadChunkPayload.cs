namespace FLBackEnd.API.GraphQL.Mutations.FileUpload;

[GraphQLDescription("The result for a new added video upload chunk")]
public class AddVideoUploadChunkPayload
{
    [GraphQLDescription("Has the chunk been successfully written to the server")]
    public bool Status { get; set; }
}