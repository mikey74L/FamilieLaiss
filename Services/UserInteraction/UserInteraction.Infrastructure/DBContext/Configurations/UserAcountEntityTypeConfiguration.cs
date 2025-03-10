using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserInteraction.Domain.Aggregates;

namespace UserInteraction.Infrastructure.DBContext.Configurations;

public class UserAccountEntityTypeConfiguration : EntityTypeConfigurationBase<UserAccount>
{
    protected override void ConfigureEntity(EntityTypeBuilder<UserAccount> modelBuilder)
    {
        //Primary Key Column
        CreateModelForIDColumn(modelBuilder, null);

        modelBuilder.HasOne(x => x.UserInteractionInfo).WithOne(x => x.UserAccount).HasForeignKey<UserInteractionInfo>(x => x.UserAccountId);
    }
}
