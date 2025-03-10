using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PictureConvert.Domain.Entities;

namespace PictureConvert.Infrastructure.DBContext.Configurations;

internal class UploadPictureEntityTypeConfiguration : EntityTypeConfigurationBase<UploadPicture>
{
    protected override void ConfigureEntity(EntityTypeBuilder<UploadPicture> modelBuilder)
    {
        //Primary Key Property
        CreateModelForIDColumn(modelBuilder, null);

        //Status 
        modelBuilder.HasOne(a => a.Status).WithOne(b => b.UploadPicture)
            .HasForeignKey<PictureConvertStatus>(b => b.UploadPictureId);
    }
}