using Catalog.Domain.Aggregates;
using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.DBContext.Configurations;

public class CategoryEntityTypeConfiguration : EntityTypeConfigurationBase<Category>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Category> modelBuilder)
    {
        //Primary Key Column
        CreateModelForIDColumn(modelBuilder, "Sequence" + nameof(Category));

        //Eindeutigen Index für Category Type und Name-German erstellen
        modelBuilder.HasIndex(x => new { x.CategoryType, Name_German = x.NameGerman }).IsUnique();

        //Eindeutigen Index für Category Type und Name-English erstellen
        modelBuilder.HasIndex(x => new { x.CategoryType, Name_English = x.NameEnglish }).IsUnique();

        //Category-Value-Property
        modelBuilder.HasMany(d => d.CategoryValues).WithOne(p => p.Category).HasForeignKey(d => d.CategoryId).Metadata
            .DependentToPrincipal?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}