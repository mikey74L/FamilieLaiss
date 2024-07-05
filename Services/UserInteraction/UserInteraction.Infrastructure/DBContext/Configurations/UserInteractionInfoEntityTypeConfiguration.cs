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

            //Property für "Rating Count"
            modelBuilder.Property(p => p.RatingCount).IsRequired().HasDefaultValue(0);

            //Property für "Average-Rating"
            modelBuilder.Property(p => p.AverageRating).IsRequired().HasDefaultValue(0);

            //Property für "Comment Count"
            modelBuilder.Property(p => p.CommentCount).IsRequired().HasDefaultValue(0);

            //Property für "Favorite Count"
            modelBuilder.Property(p => p.FavoriteCount).IsRequired().HasDefaultValue(0);

            //Ratings-Property
            modelBuilder.HasMany(d => d.Ratings).WithOne(p => p.UserInteractionInfo).HasForeignKey(d => d.UserInteractionInfoID).Metadata.DependentToPrincipal.SetPropertyAccessMode(PropertyAccessMode.Field);

            //Comments-Property
            modelBuilder.HasMany(d => d.Comments).WithOne(p => p.UserInteractionInfo).HasForeignKey(d => d.UserInteractionInfoID).Metadata.DependentToPrincipal.SetPropertyAccessMode(PropertyAccessMode.Field);

            //Favorites-Property
            modelBuilder.HasMany(d => d.Favorites).WithOne(p => p.UserInteractionInfo).HasForeignKey(d => d.UserInteractionInfoID).Metadata.DependentToPrincipal.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
