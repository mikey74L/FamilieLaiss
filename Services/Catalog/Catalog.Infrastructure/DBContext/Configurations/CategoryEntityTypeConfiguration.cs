using Catalog.Domain.Aggregates;
using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.DBContext.Configurations
{
    public class CategoryEntityTypeConfiguration : EntityTypeConfigurationBase<Category>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Category> modelBuilder)
        {
            //Primary Key Column
            CreateModelForIDColumn(modelBuilder, "Sequence" + nameof(Category));

            //Prio-Property
            modelBuilder.Property(x => x.CategoryType).IsRequired();

            //Name_German-Property
            modelBuilder.Property(x => x.NameGerman).IsRequired().HasMaxLength(300);

            //Name_English-Property
            modelBuilder.Property(x => x.NameEnglish).IsRequired().HasMaxLength(300);

            //Eindeutigen Index für Category Type und Name-German erstellen
            modelBuilder.HasIndex(x => new { x.CategoryType , Name_German = x.NameGerman}).IsUnique();

            //Eindeutigen Index für Category Type und Name-English erstellen
            modelBuilder.HasIndex(x => new { x.CategoryType, Name_English = x.NameEnglish }).IsUnique();

            //Category-Value-Property
            modelBuilder.HasMany(d => d.CategoryValues).WithOne(p => p.Category).HasForeignKey(d => d.CategoryID).Metadata.DependentToPrincipal.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
