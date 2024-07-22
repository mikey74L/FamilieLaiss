using MediatR;
using Upload.API.Mediator.Commands.UploadPicture;

namespace Upload.API.GraphQL.Mutations.UploadPicture;

[ExtendObjectType(typeof(Mutation))]
public class GraphQlMutationUploadPicture
{
    [GraphQLDescription("Delete picture vom upload area")]
    public async Task<DeleteUploadPicturePayload> DeleteUploadPictureAsync(DeleteUploadPictureInput input,
        [Service] IMediator mediator)
    {
        var deletedUploadPicture = await mediator.Send(new MtrDeleteUploadPictureCmd() { InputData = input });

        var result = new DeleteUploadPicturePayload
        {
            UploadPicture = deletedUploadPicture
        };

        return result;
    }

    [GraphQLDescription("Delete all pictures vom upload area that are not assigned")]
    public async Task<DeleteAllUploadPicturePayload> DeleteAllUploadPicturesAsync(DeleteAllUploadPictureInput input,
        [Service] IMediator mediator)
    {
        var deletedUploadPictures = await mediator.Send(new MtrDeleteAllUploadPicturesCmd() { InputData = input });

        var result = new DeleteAllUploadPicturePayload
        {
            UploadPictures = deletedUploadPictures.ToList()
        };

        return result;
    }
}