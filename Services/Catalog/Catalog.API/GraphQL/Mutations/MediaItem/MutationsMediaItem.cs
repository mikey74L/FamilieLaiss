//using Catalog.API.Commands.MediaItem;
//using MediatR;

//namespace Catalog.API.GraphQL.Mutations.MediaItem;

//[ExtendObjectType(typeof(Mutation))]
//public class MutationsMediaItem
//{
//    [GraphQLDescription("Add a new media item")]
//    public async Task<AddMediaItemPayload> AddMediaItemAsync(AddMediaItemInput input, [Service] IMediator mediator)
//    {
//        var newMediaItem = await mediator.Send(new MtrMakeNewMediaItemCmd(input));

//        var result = new AddMediaItemPayload
//        {
//            MediaItem = newMediaItem
//        };

//        return result;
//    }

//    [GraphQLDescription("Update media item")]
//    public async Task<UpdateMediaItemPayload> UpdateMediaItemAsync(UpdateMediaItemInput input, [Service] IMediator mediator)
//    {
//        var updatedMediaItem = await mediator.Send(new MtrUpdateMediaItemCmd(input));

//        var result = new UpdateMediaItemPayload
//        {
//            MediaItem = updatedMediaItem
//        };

//        return result;
//    }

//    [GraphQLDescription("Remove media item")]
//    public async Task<RemoveMediaItemPayload> RemoveMediaItemAsync(RemoveMediaItemInput input, [Service] IMediator mediator)
//    {
//        var deletedMediaItem = await mediator.Send(new MtrRemoveMediaItemCmd(input));

//        var result = new RemoveMediaItemPayload
//        {
//            MediaItem = deletedMediaItem
//        };

//        return result;
//    }
//}
