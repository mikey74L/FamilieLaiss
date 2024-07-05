using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Infrastructure.DBContext.Configurations
{
    internal class UserEntityTypeConfiguration: EntityTypeConfigurationBase<User.Domain.Aggregates.User>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<User.Domain.Aggregates.User> modelBuilder)
        {
            //Primary Key Column
            CreateModelForIDColumn(modelBuilder, null);

            //UserName-Property
            modelBuilder.Property(x => x.UserName).IsRequired().HasMaxLength(15);

            //EMail-Property
            modelBuilder.Property(x => x.EMail).IsRequired().HasMaxLength(100);

            //GenderID-Property
            modelBuilder.Property(x => x.GenderID).HasMaxLength(20);

            //GivenName-Property
            modelBuilder.Property(x => x.GivenName).HasMaxLength(100);

            //FamilyName-Property
            modelBuilder.Property(x => x.FamilyName).HasMaxLength(100);

            //Street-Property
            modelBuilder.Property(x => x.Street).HasMaxLength(100);

            //City-Property
            modelBuilder.Property(x => x.City).HasMaxLength(100);

            //HNR-Property
            modelBuilder.Property(x => x.HNR).HasMaxLength(20);

            //ZIP-Property
            modelBuilder.Property(x => x.ZIP).HasMaxLength(20);
        }
    }
}
