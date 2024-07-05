using FLBackEnd.Domain.Entities;
using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLBackEnd.Infrastructure.DataBaseContext.Configurations;

public class UploadPictureEntityTypeConfiguration : EntityTypeConfigurationBase<UploadPicture>
{
    protected override void ConfigureEntity(EntityTypeBuilder<UploadPicture> modelBuilder)
    {
        //Primary Key Column
        CreateModelForIDColumn(modelBuilder, null);

        modelBuilder.OwnsOne(x => x.UploadPictureExifInfo).ToTable("UploadPictureExifInfos");
        modelBuilder.Navigation(x => x.UploadPictureExifInfo).IsRequired(false);

        modelBuilder.OwnsOne(x => x.GoogleGeoCodingAddress).ToTable("GoogleGeoCodingAddressesPicture");
        modelBuilder.Navigation(x => x.GoogleGeoCodingAddress).IsRequired(false);

        modelBuilder.HasOne(a => a.ConvertStatus).WithOne(b => b.UploadPicture)
            .HasForeignKey<PictureConvertStatus>(b => b.UploadPictureId);
    }
}