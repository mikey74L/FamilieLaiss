using PictureConvert.Infrastructure.DBContext;

namespace PictureConvert.API.GraphQL.Queries.PictureConvertStatus;

[ExtendObjectType(typeof(Query))]
public class GraphQlQueryPictureConvertStatus
{
    [GraphQLDescription("Returns a list of picture convert status items")]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Domain.Entities.PictureConvertStatus> GetPictureConvertStatusItems(
        PictureConvertServiceDbContext context)
    {
        return context.ConvertStatusEntries;
    }
}