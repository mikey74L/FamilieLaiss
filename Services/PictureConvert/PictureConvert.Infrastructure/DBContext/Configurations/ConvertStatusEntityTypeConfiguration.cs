using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PictureConvert.Domain.Entities;

namespace PictureConvert.Infrastructure.DBContext.Configurations;

internal class ConvertStatusEntityTypeConfiguration : EntityTypeConfigurationBase<PictureConvertStatus>
{
    protected override void ConfigureEntity(EntityTypeBuilder<PictureConvertStatus> modelBuilder)
    {
        //Primary Key Column
        CreateModelForIDColumn(modelBuilder, "Sequence" + nameof(PictureConvertStatus));
    }
}