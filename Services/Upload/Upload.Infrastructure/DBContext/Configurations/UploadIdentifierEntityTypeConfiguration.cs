using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Upload.Domain.Entities;

namespace Upload.Infrastructure.DBContext.Configurations;

internal class UploadIdentifierEntityTypeConfiguration : EntityTypeConfigurationBase<UploadIdentifier>
{
    protected override void ConfigureEntity(EntityTypeBuilder<UploadIdentifier> modelBuilder)
    {
        CreateModelForIDColumn(modelBuilder, "Sequence" + nameof(UploadIdentifier));

     }
}
