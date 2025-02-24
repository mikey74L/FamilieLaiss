using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Upload.Domain.Entities;

namespace Upload.Infrastructure.DBContext.Configurations;

internal class ConvertStatusEntityTypeConfiguration : EntityTypeConfigurationBase<PictureConvertStatus>
{
    protected override void ConfigureEntity(EntityTypeBuilder<PictureConvertStatus> modelBuilder)
    {
        //Primary Key Column
        CreateModelForIDColumn(modelBuilder, "Sequence" + nameof(PictureConvertStatus));
    }
}