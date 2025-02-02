using Catalog.API.GraphQL.DataLoaders.UploadVideo;
using HotChocolate.Fusion.SourceSchema.Types;

namespace Catalog.API.GraphQL.Queries.UploadVideo;

[ExtendObjectType(typeof(Query))]
public class GraphQlQueryUploadVideo
{
    [GraphQLDescription("Returns a upload video")]
    [Lookup]
    public async Task<Domain.Entities.UploadVideo> GetUploadVideo(long id, UploadVideoDataLoader dataLoader)
        => await dataLoader.LoadAsync(id);
}