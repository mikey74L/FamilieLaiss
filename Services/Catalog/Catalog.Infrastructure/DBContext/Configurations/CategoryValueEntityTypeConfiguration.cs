using Catalog.Domain.Aggregates;
using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Infrastructure.DBContext.Configurations
{
    public class CategoryValueEntityTypeConfiguration : EntityTypeConfigurationBase<CategoryValue>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<CategoryValue> modelBuilder)
        {
            //Primary Key Column
            CreateModelForIDColumn(modelBuilder, "Sequence" + nameof(CategoryValue));

            //Name_German-Property
            modelBuilder.Property(x => x.NameGerman).IsRequired().HasMaxLength(300);

            //Name_English-Property
            modelBuilder.Property(x => x.NameEnglish).IsRequired().HasMaxLength(300);

            //Eindeutigen Index für den German-Name 
            modelBuilder.HasIndex(x => new { x.CategoryID, x.NameGerman }).IsUnique();

            //Eindeutigen Index für den English-Name
            modelBuilder.HasIndex(x => new { x.CategoryID, x.NameEnglish }).IsUnique();

            //Media-Item-Category-Values
            modelBuilder.HasMany(d => d.MediaItemCategoryValues).WithOne(p => p.CategoryValue).HasForeignKey(d => d.CategoryValueID).Metadata.DependentToPrincipal.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
