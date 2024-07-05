using FLBackEnd.API.Mediator.Commands.FileUpload;
using MediatR;

namespace FLBackEnd.API.GraphQL.Queries.FileUpload;

[ExtendObjectType(typeof(Query))]
public class GraphQlQueryFileUpload
{
    [GraphQLDescription("Get next free upload id")]
    public async Task<long> GetNextUploadId([Service] IMediator mediator)
    {
        var getUploadIdResult = await mediator.Send(new MtrCreateIdForUploadCmd());

        return getUploadIdResult;
    }
}