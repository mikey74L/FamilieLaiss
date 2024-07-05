using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Infrastructure.DBContext.Configurations
{
    internal class CountryEntityTypeConfiguration : EntityTypeConfigurationBase<User.Domain.Aggregates.Country>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<User.Domain.Aggregates.Country> modelBuilder)
        {
            //Primary Key Column
            CreateModelForIDColumn(modelBuilder, null);

            //NameGerman-Property
            modelBuilder.Property(x => x.NameGerman).IsRequired().HasMaxLength(100);

            //NameEnglish-Property
            modelBuilder.Property(x => x.NameEnglish).IsRequired().HasMaxLength(100);

            //Media-Item-Category-Values
            modelBuilder.HasMany(d => d.Users).WithOne(x => x.Country).HasForeignKey(d => d.CountryID)?.Metadata?.DependentToPrincipal?.SetPropertyAccessMode(PropertyAccessMode.Field);

        }
    }
}
