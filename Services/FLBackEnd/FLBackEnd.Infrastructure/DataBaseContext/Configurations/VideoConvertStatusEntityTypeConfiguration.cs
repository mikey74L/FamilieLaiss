using FLBackEnd.Domain.Entities;
using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLBackEnd.Infrastructure.DataBaseContext.Configurations;

internal class VideoConvertStatusEntityTypeConfiguration : EntityTypeConfigurationBase<VideoConvertStatus>
{
    protected override void ConfigureEntity(EntityTypeBuilder<VideoConvertStatus> modelBuilder)
    {
        //Primary Key Column
        CreateModelForIDColumn(modelBuilder, "Sequence" + nameof(VideoConvertStatus));
    }
}