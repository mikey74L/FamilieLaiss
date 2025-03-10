using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.Domain.Aggregates;

namespace User.Infrastructure.DBContext.Configurations;

internal class UserAccountEntityTypeConfiguration : EntityTypeConfigurationBase<UserAccount>
{
    protected override void ConfigureEntity(EntityTypeBuilder<UserAccount> modelBuilder)
    {
        //Primary Key Column
        CreateModelForIDColumn(modelBuilder, null);
    }
}
