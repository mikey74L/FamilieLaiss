using FLBackEnd.Domain.Entities;
using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLBackEnd.Infrastructure.DataBaseContext.Configurations;

internal class UploadIdentifierEntityTypeConfiguration : EntityTypeConfigurationBase<UploadIdentifier>
{
    protected override void ConfigureEntity(EntityTypeBuilder<UploadIdentifier> modelBuilder)
    {
        CreateModelForIDColumn(modelBuilder, "Sequence" + nameof(UploadIdentifier));
    }
}