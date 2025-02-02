using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserInteraction.Domain.Aggregates;

namespace UserInteraction.Infrastructure.DBContext.Configurations
{
    public class UserInteractionInfoEntityTypeConfiguration : EntityTypeConfigurationBase<UserInteractionInfo>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<UserInteractionInfo> modelBuilder)
        {
            //Primary Key Column
            CreateModelForIDColumn(modelBuilder, null);

            //Ratings-Property
            modelBuilder.HasMany(d => d.Ratings).WithOne(p => p.UserInteractionInfo).HasForeignKey(d => d.UserInteractionInfoId).Metadata.DependentToPrincipal.SetPropertyAccessMode(PropertyAccessMode.Field);

            //Comments-Property
            modelBuilder.HasMany(d => d.Comments).WithOne(p => p.UserInteractionInfo).HasForeignKey(d => d.UserInteractionInfoId).Metadata.DependentToPrincipal.SetPropertyAccessMode(PropertyAccessMode.Field);

            //Favorites-Property
            modelBuilder.HasMany(d => d.Favorites).WithOne(p => p.UserInteractionInfo).HasForeignKey(d => d.UserInteractionInfoId).Metadata.DependentToPrincipal.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
