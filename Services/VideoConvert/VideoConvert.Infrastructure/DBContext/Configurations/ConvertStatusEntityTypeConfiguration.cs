using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoConvert.Domain.Entities;

namespace VideoConvert.Infrastructure.DBContext.Configurations;

internal class ConvertStatusEntityTypeConfiguration : EntityTypeConfigurationBase<VideoConvertStatus>
{
    protected override void ConfigureEntity(EntityTypeBuilder<VideoConvertStatus> modelBuilder)
    {
        //Primary Key Column
        CreateModelForIDColumn(modelBuilder, "Sequence" + nameof(VideoConvertStatus));
    }
}