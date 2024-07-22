using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoConvert.Domain.Entities;

namespace VideoConvert.Infrastructure.DBContext.Configurations;

internal class UploadVideoEntityTypeConfiguration : EntityTypeConfigurationBase<UploadVideo>
{
    protected override void ConfigureEntity(EntityTypeBuilder<UploadVideo> modelBuilder)
    {
        //Primary Key Property
        CreateModelForIDColumn(modelBuilder, null);

        //Status 
        modelBuilder.HasOne(a => a.ConvertStatus).WithOne(b => b.UploadVideo)
            .HasForeignKey<VideoConvertStatus>(b => b.UploadVideoId);
    }
}