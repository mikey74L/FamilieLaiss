using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Upload.Domain.Entities;

namespace Upload.Infrastructure.DBContext.Configurations;

internal class UploadPortraitEntityTypeConfiguration : EntityTypeConfigurationBase<UploadPortrait>
{
    protected override void ConfigureEntity(EntityTypeBuilder<UploadPortrait> modelBuilder)
    {
        CreateModelForIDColumn(modelBuilder, null);

        modelBuilder.Property(x => x.UserName).IsRequired();
    }
}
