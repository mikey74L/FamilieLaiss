using MediatR;
using Upload.API.Mediator.Commands.UploadVideo;

namespace Upload.API.GraphQL.Mutations.UploadVideo;

[ExtendObjectType(typeof(Mutation))]
public class GraphQlMutationUploadVideo
{
    [GraphQLDescription("Delete video vom upload area")]
    public async Task<DeleteUploadVideoPayload> DeleteUploadVideoAsync(DeleteUploadVideoInput input,
        [Service] IMediator mediator)
    {
        var deletedUploadVideo = await mediator.Send(new MtrDeleteUploadVideoCmd() { InputData = input });

        var result = new DeleteUploadVideoPayload
        {
            UploadVideo = deletedUploadVideo
        };

        return result;
    }

    [GraphQLDescription("Delete all videos vom upload area that are not assigned")]
    public async Task<DeleteAllUploadVideoPayload> DeleteAllUploadVideosAsync(DeleteAllUploadVideoInput input,
        [Service] IMediator mediator)
    {
        var deletedUploadVideos = await mediator.Send(new MtrDeleteAllUploadVideosCmd() { InputData = input });

        var result = new DeleteAllUploadVideoPayload
        {
            UploadVideos = deletedUploadVideos.ToList()
        };

        return result;
    }
}