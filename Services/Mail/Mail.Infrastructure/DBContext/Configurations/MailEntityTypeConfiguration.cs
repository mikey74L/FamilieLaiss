using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mail.Infrastructure.DBContext.Configurations
{
    internal class MailEntityTypeConfiguration: EntityTypeConfigurationBase<Mail.Domain.Entities.Mail>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Mail.Domain.Entities.Mail> modelBuilder)
        {
            //Primary Key Column
            CreateModelForIDColumn(modelBuilder, "Sequence" + nameof(Mail.Domain.Entities.Mail));

            //SenderAdress-Property
            modelBuilder.Property(x => x.SenderAdress).IsRequired().HasMaxLength(300);

            //SenderName-Property
            modelBuilder.Property(x => x.SenderName).IsRequired().HasMaxLength(300);

            //ReceiverAdress-Property
            modelBuilder.Property(x => x.ReceiverAdress).IsRequired().HasMaxLength(300);

            //ReceiverName-Property
            modelBuilder.Property(x => x.ReceiverName).IsRequired().HasMaxLength(300);

            //Subject-Property
            modelBuilder.Property(x => x.Subject).IsRequired().HasMaxLength(200);

            //IsBodyHtml-Property
            modelBuilder.Property(x => x.IsBodyHTML).IsRequired().HasDefaultValue(false);

            //Body-Property
            modelBuilder.Property(x => x.Body).IsRequired();
        }
    }
}
