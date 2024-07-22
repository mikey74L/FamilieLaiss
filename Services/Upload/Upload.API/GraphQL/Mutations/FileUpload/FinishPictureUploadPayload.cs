namespace Upload.API.GraphQL.Mutations.FileUpload;

[GraphQLDescription("The result for a finished picture upload")]
public class FinishPictureUploadPayload
{
    [GraphQLDescription("Has the finish operation successfully completed")]
    public bool Status { get; set; }
}