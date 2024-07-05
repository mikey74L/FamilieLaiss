using FLBackEnd.Domain.Entities;
using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLBackEnd.Infrastructure.DataBaseContext.Configurations;

public class CategoryEntityTypeConfiguration : EntityTypeConfigurationBase<Category>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Category> modelBuilder)
    {
        CreateModelForIDColumn(modelBuilder, "Sequence" + nameof(Category));

        modelBuilder.HasIndex(x => new { x.CategoryType, Name_German = x.NameGerman }).IsUnique();

        modelBuilder.HasIndex(x => new { x.CategoryType, Name_English = x.NameEnglish }).IsUnique();

        modelBuilder.HasMany(d => d.CategoryValues).WithOne(p => p.Category).HasForeignKey(d => d.CategoryId).Metadata
            .DependentToPrincipal.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}