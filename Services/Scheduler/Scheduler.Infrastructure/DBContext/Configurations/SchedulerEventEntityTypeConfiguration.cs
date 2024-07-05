using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scheduler.Domain.Aggregates;
using Scheduler.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler.Infrastructure.DBContext.Configurations
{
    internal class SchedulerEventEntityTypeConfiguration: EntityTypeConfigurationBase<SchedulerEvent>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<SchedulerEvent> modelBuilder)
        {
            //Primary Key Column
            CreateModelForIDColumn(modelBuilder, "Sequence" + nameof(SchedulerEvent));

            //Property Title
            modelBuilder.Property(x => x.Title).IsRequired().HasMaxLength(200);

            //Property Description
            modelBuilder.Property(x => x.Description).HasMaxLength(700);

            //Property Location
            modelBuilder.Property(x => x.Location).HasMaxLength(250);

            //Property Start-Time
            modelBuilder.Property(x => x.StartTime).IsRequired();

            //Property End-Time
            modelBuilder.Property(x => x.EndTime).IsRequired();

            //Property IsAllDay
            modelBuilder.Property(x => x.IsAllDay).IsRequired();
        }
    }
}
