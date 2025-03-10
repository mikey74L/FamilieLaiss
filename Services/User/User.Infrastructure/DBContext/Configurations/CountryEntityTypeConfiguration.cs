using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.Domain.Aggregates;

namespace User.Infrastructure.DBContext.Configurations;

internal class CountryEntityTypeConfiguration : EntityTypeConfigurationBase<Country>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Country> modelBuilder)
    {
        //Primary Key Column
        CreateModelForIDColumn(modelBuilder, null);

        //Media-Item-Category-Values
        modelBuilder.HasMany(d => d.Users).WithOne(x => x.Country).HasForeignKey(d => d.CountryId)?.Metadata?.DependentToPrincipal?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
