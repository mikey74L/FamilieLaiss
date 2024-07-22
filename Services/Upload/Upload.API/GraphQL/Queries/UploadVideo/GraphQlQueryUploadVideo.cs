using FamilieLaissSharedObjects.Enums;
using Upload.API.GraphQL.DataLoader.UploadVideo;
using Upload.Infrastructure.DBContext;

namespace Upload.API.GraphQL.Queries.UploadVideo;

[ExtendObjectType(typeof(Query))]
public class GraphQlQueryUploadVideo
{
    [GraphQLDescription("Returns a list of upload videos")]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Domain.Entities.UploadVideo> GetUploadVideos(UploadServiceDbContext context)
    {
        return context.UploadVideos;
    }

    [GraphQLDescription("Returns a upload video")]
    [UseProjection]
    public async Task<Domain.Entities.UploadVideo> GetUploadVideo(long id, UploadVideoDataLoader dataLoader)
        => await dataLoader.LoadAsync(id);

    [GraphQLDescription("Returns the current count for converted and unassigned upload videos")]
    public int GetUploadVideoCount(UploadServiceDbContext context)
    {
        return context.UploadVideos.Count(x => x.Status == EnumUploadStatus.Converted);
    }
}