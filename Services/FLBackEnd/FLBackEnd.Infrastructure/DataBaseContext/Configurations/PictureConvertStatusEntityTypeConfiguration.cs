using FLBackEnd.Domain.Entities;
using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLBackEnd.Infrastructure.DataBaseContext.Configurations;

internal class PictureConvertStatusEntityTypeConfiguration : EntityTypeConfigurationBase<PictureConvertStatus>
{
    protected override void ConfigureEntity(EntityTypeBuilder<PictureConvertStatus> modelBuilder)
    {
        CreateModelForIDColumn(modelBuilder, "Sequence" + nameof(PictureConvertStatus));
    }
}