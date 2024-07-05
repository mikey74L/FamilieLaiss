using InfrastructureHelper.DatabaseContext;
using Message.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Message.Infrastructure.DBContext.Configurations
{
    internal class MessageUserEntityTypeConfiguration: EntityTypeConfigurationBase<MessageUser>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<MessageUser> modelBuilder)
        {
            //Primary Key Column
            CreateModelForIDColumn(modelBuilder, "Sequence" + nameof(MessageUser));

            //Prio-Property
            modelBuilder.Property(x => x.UserName).IsRequired();

            //Readed-Property
            modelBuilder.Property(x => x.Readed).IsRequired().HasDefaultValue(false);
        }
    }
}
