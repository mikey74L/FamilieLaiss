using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scheduler.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler.Infrastructure.DBContext.Configurations
{
    internal class SchedulerResourceEntityTypeConfiguration: EntityTypeConfigurationBase<SchedulerResource>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<SchedulerResource> modelBuilder)
        {
            //Primary Key Column
            CreateModelForIDColumn(modelBuilder, "Sequence" + nameof(SchedulerResource));

            //Property Name
            modelBuilder.Property(x => x.Name).IsRequired().HasMaxLength(70);

            //Property Color
            modelBuilder.Property(x => x.Color).IsRequired().HasMaxLength(15);

            //Property Starting-Time
            modelBuilder.Property(x => x.StartingTime).IsRequired().HasMaxLength(5);

            //Property Ending-Time
            modelBuilder.Property(x => x.EndingTime).IsRequired().HasMaxLength(5);
        }
    }
}
