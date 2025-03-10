using Catalog.API.Mediator.Commands.MediaGroup;
using FLBackEnd.API.GraphQL.Mutations.MediaGroup;
using MediatR;

namespace Catalog.API.GraphQL.Mutations.MediaGroup;

[ExtendObjectType(typeof(Mutation))]
public class GraphQlMutationMediaGroup
{
    //[Authorize(Policy = "MediaGroup.Add")]
    [GraphQLDescription("Add a new media group")]
    public async Task<AddMediaGroupPayload> AddMediaGroupAsync(AddMediaGroupInput input, [Service] IMediator mediator)
    {
        var newMediaGroup = await mediator.Send(new MtrAddMediaGroupCommand() { InputData = input });

        var result = new AddMediaGroupPayload
        {
            MediaGroup = newMediaGroup
        };

        return result;
    }

    //[Authorize("MediaGroup.Change")]
    [GraphQLDescription("Update existing media group")]
    public async Task<UpdateMediaGroupPayload> UpdateMediaGroupAsync(UpdateMediaGroupInput input,
        [Service] IMediator mediator)
    {
        var updatedMediaGroup = await mediator.Send(new MtrUpdateMediaGroupCommand() { InputData = input });

        var result = new UpdateMediaGroupPayload
        {
            MediaGroup = updatedMediaGroup
        };

        return result;
    }

    //[Authorize(Policy = "MediaGroup.Delete")]
    [GraphQLDescription("Delete existing media group")]
    public async Task<DeleteMediaGroupPayload> DeleteMediaGroupAsync(DeleteMediaGroupInput input,
        [Service] IMediator mediator)
    {
        var deletedMediaGroup = await mediator.Send(new MtrDeleteMediaGroupCommand() { InputData = input });

        var result = new DeleteMediaGroupPayload
        {
            MediaGroup = deletedMediaGroup
        };

        return result;
    }
}