using FamilieLaissSharedObjects.Enums;
using FLBackEnd.Infrastructure.DatabaseContext;

namespace FLBackEnd.API.GraphQL.Queries.UploadVideo;

[ExtendObjectType(typeof(Query))]
public class GraphQlQueryUploadVideo
{
    [GraphQLDescription("Returns a list of upload videos")]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Domain.Entities.UploadVideo> GetUploadVideos(FamilieLaissDbContext context)
    {
        return context.UploadVideos;
    }

    [GraphQLDescription("Returns the current count for converted and unassigned upload videos")]
    public int GetUploadVideoCount(FamilieLaissDbContext context)
    {
        return context.UploadVideos.Count(x => x.Status == EnumUploadStatus.Converted);
    }
}
