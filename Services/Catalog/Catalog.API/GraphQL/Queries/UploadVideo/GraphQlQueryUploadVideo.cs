using Catalog.API.GraphQL.DataLoader.UploadVideo;

namespace Catalog.API.GraphQL.Queries.UploadVideo;

[ExtendObjectType(typeof(Query))]
public class GraphQlQueryUploadVideo
{
    [GraphQLDescription("Returns a upload video")]
    public async Task<Domain.Entities.UploadVideo> GetUploadVideo(long id, UploadVideoDataLoader dataLoader)
        => await dataLoader.LoadAsync(id);
}