using VideoConvert.Infrastructure.DBContext;

namespace VideoConvert.API.GraphQL.Queries.VideoConvertStatus;

[ExtendObjectType(typeof(Query))]
public class GraphQlQueryVideoConvertStatus
{
    [GraphQLDescription("Returns a list of video convert status items")]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Domain.Entities.VideoConvertStatus> GetVideoConvertStatusItems(
        VideoConvertServiceDbContext context)
    {
        return context.ConvertStatusEntries;
    }
}