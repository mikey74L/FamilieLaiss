using FLBackEnd.API.Mediator.Commands.FileUpload;
using MediatR;

namespace FLBackEnd.API.GraphQL.Mutations.FileUpload;

[ExtendObjectType(typeof(Mutation))]
public class GraphQlMutationFileUpload
{
    [GraphQLDescription("Add a new picture upload chunk")]
    public async Task<AddPictureUploadChunkPayload> AddPictureUploadChunkAsync(AddPictureUploadChunkInput input,
        [Service] IMediator mediator)
    {
        var currentStatus = await mediator.Send(new MtrAddUploadChunkPictureCmd() { Data = input });

        var result = new AddPictureUploadChunkPayload
        {
            Status = currentStatus
        };

        return result;
    }

    [GraphQLDescription("Finish picture upload")]
    public async Task<FinishPictureUploadPayload> PictureUploadFinishAsync(FinishPictureUploadInput input,
        [Service] IMediator mediator)
    {
        var currentStatus = await mediator.Send(new MtrFinishPictureUploadCmd() { Data = input });

        var result = new FinishPictureUploadPayload
        {
            Status = currentStatus
        };

        return result;
    }

    [GraphQLDescription("Add a new video upload chunk")]
    public async Task<AddVideoUploadChunkPayload> AddVideoUploadChunkAsync(AddVideoUploadChunkInput input,
        [Service] IMediator mediator)
    {
        var currentStatus = await mediator.Send(new MtrAddUploadChunkVideoCmd() { Data = input });

        var result = new AddVideoUploadChunkPayload
        {
            Status = currentStatus
        };

        return result;
    }

    [GraphQLDescription("Finish video upload")]
    public async Task<FinishVideoUploadPayload> VideoUploadFinishAsync(FinishVideoUploadInput input,
        [Service] IMediator mediator)
    {
        var currentStatus = await mediator.Send(new MtrFinishVideoUploadCmd() { Data = input });

        var result = new FinishVideoUploadPayload
        {
            Status = currentStatus
        };

        return result;
    }
}