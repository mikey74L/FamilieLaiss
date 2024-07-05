using FLBackEnd.Domain.Entities;
using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLBackEnd.Infrastructure.DataBaseContext.Configurations;

public class MediaItemCategoryValueEntityTypeConfiguration : EntityTypeConfigurationBase<MediaItemCategoryValue>
{
    protected override void ConfigureEntity(EntityTypeBuilder<MediaItemCategoryValue> modelBuilder)
    {
        //Primary Key Column
        CreateModelForIDColumn(modelBuilder, "Sequence" + nameof(MediaItemCategoryValue));
    }
}