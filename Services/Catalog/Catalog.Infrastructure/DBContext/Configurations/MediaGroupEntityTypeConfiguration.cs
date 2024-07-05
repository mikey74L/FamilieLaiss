using Catalog.Domain.Aggregates;
using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Infrastructure.DBContext.Configurations
{
    public class MediaGroupEntityTypeConfiguration : EntityTypeConfigurationBase<MediaGroup>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<MediaGroup> modelBuilder)
        {
            //Primary Key Column
            CreateModelForIDColumn(modelBuilder, "Sequence" + nameof(MediaGroup));

            //Name_German-Property
            modelBuilder.Property(x => x.NameGerman).IsRequired().HasMaxLength(300);

            //Name_English-Property
            modelBuilder.Property(x => x.NameEnglish).IsRequired().HasMaxLength(300);

            //Description_German-Property
            modelBuilder.Property(x => x.DescriptionGerman).HasMaxLength(3000);

            //Description_English-Property
            modelBuilder.Property(x => x.DescriptionEnglish).HasMaxLength(3000);

            //Eindeutigen Index für Media-Type und Name-German erstellen
            modelBuilder.HasIndex(x => new { x.NameGerman }).IsUnique();

            //Eindeutigen Index für Media-Type und Name-English erstellen
            modelBuilder.HasIndex(x => new { x.NameEnglish }).IsUnique();

            //Category-Value-Property
            modelBuilder.HasMany(d => d.MediaItems).WithOne(p => p.MediaGroup).HasForeignKey(d => d.MediaGroupID).Metadata.DependentToPrincipal.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
