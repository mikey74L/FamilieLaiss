using Catalog.API.GraphQL.DataLoader.UploadPicture;

namespace Catalog.API.GraphQL.Queries.UploadPicture;

[ExtendObjectType(typeof(Query))]
public class GraphQlQueryUploadPicture
{
    [GraphQLDescription("Returns a upload picture")]
    public async Task<Domain.Entities.UploadPicture> GetUploadPicture(long id, UploadPictureDataLoader dataLoader)
        => await dataLoader.LoadAsync(id);
}