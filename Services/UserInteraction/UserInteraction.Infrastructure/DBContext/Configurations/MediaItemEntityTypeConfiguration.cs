using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserInteraction.Domain.Aggregates;

namespace UserInteraction.Infrastructure.DBContext.Configurations;

public class MediaItemEntityTypeConfiguration : EntityTypeConfigurationBase<MediaItem>
{
    protected override void ConfigureEntity(EntityTypeBuilder<MediaItem> modelBuilder)
    {
        //Primary Key Column
        CreateModelForIDColumn(modelBuilder, "Sequence" + nameof(MediaItem));

        //Ratings-Property
        modelBuilder.HasMany(d => d.Ratings).WithOne(p => p.MediaItem).HasForeignKey(d => d.MediaItemId).Metadata.DependentToPrincipal.SetPropertyAccessMode(PropertyAccessMode.Field);

        //Comments-Property
        modelBuilder.HasMany(d => d.Comments).WithOne(p => p.MediaItem).HasForeignKey(d => d.MediaItemId).Metadata.DependentToPrincipal.SetPropertyAccessMode(PropertyAccessMode.Field);

        //Favorites-Property
        modelBuilder.HasMany(d => d.Favorites).WithOne(p => p.MediaItem).HasForeignKey(d => d.MediaItemId).Metadata.DependentToPrincipal.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
