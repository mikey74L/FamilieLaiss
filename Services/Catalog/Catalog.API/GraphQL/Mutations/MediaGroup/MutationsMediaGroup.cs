//using Catalog.API.Commands.MediaGroup;
//using MediatR;

//namespace Catalog.API.GraphQL.Mutations.MediaGroup
//{
//    [ExtendObjectType(typeof(Mutation))]
//    public class MutationsMediaGroup
//    {
//        [GraphQLDescription("Add a new media group")]
//        //[Authorize(Policy = "MediaGroup.Add")]
//        public async Task<AddMediaGroupPayload> AddMediaGroupAsync(AddMediaGroupInput input, [Service] IMediator mediator)
//        {
//            var newMediaGroup = await mediator.Send(new MtrMakeNewMediaGroupCmd(input));

//            var result = new AddMediaGroupPayload
//            {
//                MediaGroup = newMediaGroup
//            };

//            return result;
//        }

//        [GraphQLDescription("Update existing media group")]
//        //[Authorize(Policy = "MediaGroup.Change")]
//        public async Task<UpdateMediaGroupPayload> UpdateMediaGroupAsync(UpdateMediaGroupInput input, [Service] IMediator mediator)
//        {
//            var updatedMediaGroup = await mediator.Send(new MtrUpdateMediaGroupCmd(input));

//            var result = new UpdateMediaGroupPayload
//            {
//                MediaGroup = updatedMediaGroup
//            };

//            return result;
//        }

//        [GraphQLDescription("Delete existing media group")]
//        //[Authorize(Policy = "MediaGroup.Delete")]
//        public async Task<DeleteMediaGroupPayload> DeleteMediaGroupAsync(DeleteMediaGroupInput input, [Service] IMediator mediator)
//        {
//            var deletedMediaGroup = await mediator.Send(new MtrDeleteMediaGroupCmd(input));

//            var result = new DeleteMediaGroupPayload
//            {
//                MediaGroup = deletedMediaGroup
//            };

//            return result;
//        }
//    }
//}
