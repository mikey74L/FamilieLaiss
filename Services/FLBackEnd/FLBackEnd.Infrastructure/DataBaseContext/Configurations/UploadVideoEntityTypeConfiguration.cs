using FLBackEnd.Domain.Entities;
using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLBackEnd.Infrastructure.DataBaseContext.Configurations;

public class UploadVideoEntityTypeConfiguration : EntityTypeConfigurationBase<UploadVideo>
{
    protected override void ConfigureEntity(EntityTypeBuilder<UploadVideo> modelBuilder)
    {
        //Primary Key Column
        CreateModelForIDColumn(modelBuilder, null);

        modelBuilder.OwnsOne(x => x.GoogleGeoCodingAddress).ToTable("GoogleGeoCodingAddressesVideo");
        modelBuilder.Navigation(x => x.GoogleGeoCodingAddress).IsRequired(false);

        //Status 
        modelBuilder.HasOne(a => a.ConvertStatus).WithOne(b => b.UploadVideo)
            .HasForeignKey<VideoConvertStatus>(b => b.UploadVideoId);
    }
}