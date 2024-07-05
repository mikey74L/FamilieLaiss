using FLBackEnd.Domain.Entities;
using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLBackEnd.Infrastructure.DataBaseContext.Configurations;

public class CategoryValueEntityTypeConfiguration : EntityTypeConfigurationBase<CategoryValue>
{
    protected override void ConfigureEntity(EntityTypeBuilder<CategoryValue> modelBuilder)
    {
        CreateModelForIDColumn(modelBuilder, "Sequence" + nameof(CategoryValue));

        modelBuilder.HasIndex(x => new { CategoryID = x.CategoryId, x.NameGerman }).IsUnique();

        modelBuilder.HasIndex(x => new { CategoryID = x.CategoryId, x.NameEnglish }).IsUnique();

        modelBuilder.HasMany(d => d.MediaItemCategoryValues).WithOne(p => p.CategoryValue)
            .HasForeignKey(d => d.CategoryValueId).Metadata.DependentToPrincipal
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}