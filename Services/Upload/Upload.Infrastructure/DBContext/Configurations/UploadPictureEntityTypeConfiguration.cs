using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Upload.Domain.Entities;

namespace Upload.Infrastructure.DBContext.Configurations;

internal class UploadPictureEntityTypeConfiguration : EntityTypeConfigurationBase<UploadPicture>
{
    protected override void ConfigureEntity(EntityTypeBuilder<UploadPicture> modelBuilder)
    {
        CreateModelForIDColumn(modelBuilder, null);

        modelBuilder.OwnsOne(x => x.UploadPictureExifInfo).ToTable("UploadPictureExifInfos");
        modelBuilder.Navigation(x => x.UploadPictureExifInfo).IsRequired(false);

        modelBuilder.OwnsOne(x => x.GoogleGeoCodingAddress).ToTable("GoogleGeoCodingAdressesPicture");
        modelBuilder.Navigation(x => x.GoogleGeoCodingAddress).IsRequired(false);
    }
}
