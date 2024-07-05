namespace FLBackEnd.API.GraphQL.Mutations.UploadVideo;

[GraphQLDescription("InputData type for deleting all upload videos")]
public class DeleteAllUploadVideoPayload
{
    [GraphQLDescription("The list of deleted upload videos")]
    public List<Domain.Entities.UploadVideo> UploadVideos { get; set; } = [];
}
