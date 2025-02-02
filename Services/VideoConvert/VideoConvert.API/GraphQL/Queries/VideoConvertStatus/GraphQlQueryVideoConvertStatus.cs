using HotChocolate.Fusion.SourceSchema.Types;
using PictureConvert.API.GraphQL.DataLoader.VideoConvertStatus;
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

    [GraphQLDescription("Returns a video convert status item")]
    [Lookup]
    public async Task<Domain.Entities.VideoConvertStatus> GetVideoConvertStatus(long id, VideoConvertStatusDataLoader dataLoader)
        => await dataLoader.LoadAsync(id);
}