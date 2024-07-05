using FLBackEnd.Domain.Entities;
using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLBackEnd.Infrastructure.DataBaseContext.Configurations;

public class MediaGroupEntityTypeConfiguration : EntityTypeConfigurationBase<MediaGroup>
{
    protected override void ConfigureEntity(EntityTypeBuilder<MediaGroup> modelBuilder)
    {
        CreateModelForIDColumn(modelBuilder, "Sequence" + nameof(MediaGroup));

        modelBuilder.HasIndex(x => new { x.NameGerman }).IsUnique();

        modelBuilder.HasIndex(x => new { x.NameEnglish }).IsUnique();

        modelBuilder.HasMany(d => d.MediaItems).WithOne(p => p.MediaGroup).HasForeignKey(d => d.MediaGroupId).Metadata
            .DependentToPrincipal.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}