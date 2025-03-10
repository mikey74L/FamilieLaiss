using Catalog.Domain.Aggregates;
using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.DBContext.Configurations;

public class MediaGroupEntityTypeConfiguration : EntityTypeConfigurationBase<MediaGroup>
{
    protected override void ConfigureEntity(EntityTypeBuilder<MediaGroup> modelBuilder)
    {
        //Primary Key Column
        CreateModelForIDColumn(modelBuilder, "Sequence" + nameof(MediaGroup));

        //Eindeutigen Index für Media-Type und Name-German erstellen
        modelBuilder.HasIndex(x => new { x.NameGerman }).IsUnique();

        //Eindeutigen Index für Media-Type und Name-English erstellen
        modelBuilder.HasIndex(x => new { x.NameEnglish }).IsUnique();

        //Category-Value-Property
        modelBuilder.HasMany(d => d.MediaItems).WithOne(p => p.MediaGroup).HasForeignKey(d => d.MediaGroupId).Metadata
            .DependentToPrincipal?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}