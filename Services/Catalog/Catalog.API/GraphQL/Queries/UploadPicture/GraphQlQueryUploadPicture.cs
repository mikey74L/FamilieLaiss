using Catalog.API.GraphQL.DataLoaders.UploadPicture;
using HotChocolate.Fusion.SourceSchema.Types;

namespace Catalog.API.GraphQL.Queries.UploadPicture;

[ExtendObjectType(typeof(Query))]
public class GraphQlQueryUploadPicture
{
    [GraphQLDescription("Returns a upload picture")]
    [Lookup]
    public async Task<Domain.Entities.UploadPicture> GetUploadPicture(long id, UploadPictureDataLoader dataLoader)
        => await dataLoader.LoadAsync(id);
}