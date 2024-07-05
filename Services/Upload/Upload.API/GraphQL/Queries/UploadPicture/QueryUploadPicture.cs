using Microsoft.EntityFrameworkCore;
using Upload.Infrastructure.DBContext;

namespace Upload.API.GraphQL.Queries.UploadPicture
{
    public class QueryUploadPicture
    {
        public IQueryable<Upload.Domain.Entities.UploadPicture> GetUploadPictures(UploadServiceDBContext context)
        {
            return context.UploadPictures;
        }

        public async Task<int> GetUploadPictureCount(UploadServiceDBContext context)
        {
            return await context.UploadPictures.CountAsync(x => x.Status == FamilieLaissSharedObjects.Enums.EnumUploadStatus.Converted);
        }
    }
}
