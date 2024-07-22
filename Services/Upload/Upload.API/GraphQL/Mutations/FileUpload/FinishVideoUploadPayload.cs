namespace Upload.API.GraphQL.Mutations.FileUpload;

[GraphQLDescription("The result for a finished video upload")]
public class FinishVideoUploadPayload
{
    [GraphQLDescription("Has the finish operation successfully completed")]
    public bool Status { get; set; }
}