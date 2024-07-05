namespace FLBackEnd.API.GraphQL.Mutations.UploadVideo;

[GraphQLDescription("The result for a deleted upload video")]
public class DeleteUploadVideoPayload
{
    [GraphQLDescription("The deleted upload video")]
    public Domain.Entities.UploadVideo UploadVideo { get; set; } = default!;
}
