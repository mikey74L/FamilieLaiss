using Catalog.Domain.Entities;
using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.DBContext.Configurations
{
    public class UploadVideoEntityTypeConfiguration : EntityTypeConfigurationBase<UploadVideo>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<UploadVideo> modelBuilder)
        {
            //Primary Key Column
            CreateModelForIDColumn(modelBuilder, null);
        }
    }
}
