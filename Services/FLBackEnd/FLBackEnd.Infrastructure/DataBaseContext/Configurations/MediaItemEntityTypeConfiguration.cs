using FLBackEnd.Domain.Entities;
using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLBackEnd.Infrastructure.DataBaseContext.Configurations;

public class MediaItemEntityTypeConfiguration : EntityTypeConfigurationBase<MediaItem>
{
    protected override void ConfigureEntity(EntityTypeBuilder<MediaItem> modelBuilder)
    {
        CreateModelForIDColumn(modelBuilder, "Sequence" + nameof(MediaItem));

        modelBuilder.HasOne(d => d.UploadPicture).WithOne(p => p.MediaItem)
            .HasForeignKey<UploadPicture>(x => x.MediaItemId).Metadata.DependentToPrincipal
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        modelBuilder.HasOne(d => d.UploadVideo).WithOne(p => p.MediaItem).HasForeignKey<UploadVideo>(x => x.MediaItemId)
            .Metadata.DependentToPrincipal.SetPropertyAccessMode(PropertyAccessMode.Field);

        modelBuilder.HasIndex(x => new { x.MediaGroupId, x.NameGerman }).IsUnique();

        modelBuilder.HasIndex(x => new { x.MediaGroupId, x.NameEnglish }).IsUnique();

        modelBuilder.HasMany(d => d.MediaItemCategoryValues).WithOne(p => p.MediaItem).HasForeignKey(d => d.MediaItemId)
            .Metadata.DependentToPrincipal.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}