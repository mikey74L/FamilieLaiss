using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserInteraction.Domain.Aggregates;

namespace UserInteraction.Infrastructure.DBContext.Configurations
{
    public class UserAccountEntityTypeConfiguration : EntityTypeConfigurationBase<UserAccount>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<UserAccount> modelBuilder)
        {
            //Primary Key Column
            CreateModelForIDColumn(modelBuilder, null);

            //Konfigurieren der Properties
            modelBuilder.Property(p => p.UserName).IsRequired();
            modelBuilder.Property(p => p.CommentCount).IsRequired().HasDefaultValue(0);
            modelBuilder.Property(p => p.RatingCount).IsRequired().HasDefaultValue(0);
            modelBuilder.Property(p => p.FavoriteCount).IsRequired().HasDefaultValue(0);
            modelBuilder.HasMany(d => d.Ratings).WithOne(p => p.UserAccount).HasForeignKey(d => d.UserAccountID).Metadata.DependentToPrincipal.SetPropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.HasMany(d => d.Comments).WithOne(p => p.UserAccount).HasForeignKey(d => d.UserAccountID).Metadata.DependentToPrincipal.SetPropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.HasMany(d => d.Favorites).WithOne(p => p.UserAccount).HasForeignKey(d => d.UserAccountID).Metadata.DependentToPrincipal.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
