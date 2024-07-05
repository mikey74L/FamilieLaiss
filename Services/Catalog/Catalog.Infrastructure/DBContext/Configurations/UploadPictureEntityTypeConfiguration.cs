using Catalog.Domain.Entities;
using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.DBContext.Configurations
{
    public class UploadPictureEntityTypeConfiguration : EntityTypeConfigurationBase<UploadPicture>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<UploadPicture> modelBuilder)
        {
            //Primary Key Column
            CreateModelForIDColumn(modelBuilder, null);
        }
    }
}
