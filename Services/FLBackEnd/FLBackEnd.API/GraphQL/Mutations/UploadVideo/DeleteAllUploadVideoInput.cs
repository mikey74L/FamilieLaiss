namespace FLBackEnd.API.GraphQL.Mutations.UploadVideo;

[GraphQLDescription("The result for all deleted upload videos")]
public class DeleteAllUploadVideoInput
{
    [GraphQLDescription("The Id list for the upload videos to delete")]
    public List<long> UploadVideoIds { get; set; } = [];
}
