using FLBackEnd.API.Mediator.Commands.MediaItem;
using MediatR;

namespace FLBackEnd.API.GraphQL.Mutations.MediaItem;

[ExtendObjectType(typeof(Mutation))]
public class GraphQlMutationMediaItem
{
    //[Authorize(Policy = "MediaItem.Add")]
    [GraphQLDescription("Add a new media item")]
    public async Task<AddMediaItemPayload> AddMediaItemAsync(AddMediaItemInput input, [Service] IMediator mediator)
    {
        var newMediaItem = await mediator.Send(new MtrAddMediaItemCommand() { InputData = input });

        var result = new AddMediaItemPayload
        {
            MediaItem = newMediaItem
        };

        return result;
    }

    //[Authorize("MediaItem.Change")]
    [GraphQLDescription("Update existing media item")]
    public async Task<UpdateMediaItemPayload> UpdateMediaItemAsync(UpdateMediaItemInput input,
        [Service] IMediator mediator)
    {
        var updatedMediaItem = await mediator.Send(new MtrUpdateMediaItemCommand() { InputData = input });

        var result = new UpdateMediaItemPayload
        {
            MediaItem = updatedMediaItem
        };

        return result;
    }

    //[Authorize(Policy = "MediaItem.Delete")]
    [GraphQLDescription("Delete existing media item")]
    public async Task<DeleteMediaItemPayload> DeleteMediaItemAsync(DeleteMediaItemInput input,
        [Service] IMediator mediator)
    {
        var deletedMediaItem = await mediator.Send(new MtrDeleteMediaItemCommand() { InputData = input });

        var result = new DeleteMediaItemPayload
        {
            MediaItem = deletedMediaItem
        };

        return result;
    }
}
