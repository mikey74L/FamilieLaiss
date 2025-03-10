using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Upload.Domain.Entities;

namespace Upload.Infrastructure.DBContext.Configurations;

internal class UploadVideoEntityTypeConfiguration : EntityTypeConfigurationBase<UploadVideo>
{
    protected override void ConfigureEntity(EntityTypeBuilder<UploadVideo> modelBuilder)
    {
        //Primary Key Column
        CreateModelForIDColumn(modelBuilder, null);

        modelBuilder.OwnsOne(x => x.GoogleGeoCodingAddress).ToTable("GoogleGeoCodingAddressesVideo");
        modelBuilder.Navigation(x => x.GoogleGeoCodingAddress).IsRequired(false);
    }
}