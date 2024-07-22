using Catalog.Domain.Aggregates;
using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.DBContext.Configurations;

public class CategoryValueEntityTypeConfiguration : EntityTypeConfigurationBase<CategoryValue>
{
    protected override void ConfigureEntity(EntityTypeBuilder<CategoryValue> modelBuilder)
    {
        //Primary Key Column
        CreateModelForIDColumn(modelBuilder, "Sequence" + nameof(CategoryValue));

        //Eindeutigen Index für den German-Name 
        modelBuilder.HasIndex(x => new { x.CategoryId, x.NameGerman }).IsUnique();

        //Eindeutigen Index für den English-Name
        modelBuilder.HasIndex(x => new { x.CategoryId, x.NameEnglish }).IsUnique();

        //Media-Item-Category-Values
        modelBuilder.HasMany(d => d.MediaItemCategoryValues).WithOne(p => p.CategoryValue)
            .HasForeignKey(d => d.CategoryValueId).Metadata.DependentToPrincipal
            ?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}