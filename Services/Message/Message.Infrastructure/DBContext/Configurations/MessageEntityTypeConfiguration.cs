using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Message.Infrastructure.DBContext.Configurations
{
    internal class MessageEntityTypeConfiguration: EntityTypeConfigurationBase<Message.Domain.Aggregates.Message>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Message.Domain.Aggregates.Message> modelBuilder)
        {
            //Primary Key Column
            CreateModelForIDColumn(modelBuilder, "Sequence" + nameof(Message.Domain.Aggregates.Message));

            //Prio-Property
            modelBuilder.Property(x => x.Prio).IsRequired();

            //Text_German-Property
            modelBuilder.Property(x => x.Text_German).IsRequired().HasMaxLength(2000);

            //Text_English-Property
            modelBuilder.Property(x => x.Text_English).IsRequired().HasMaxLength(2000);

            //Additional-Property
            modelBuilder.Property(x => x.AdditionalData).HasMaxLength(2000);

            //Google-Geo-Coding-Adress-Property
            modelBuilder.HasMany(d => d.MessageUsers).WithOne(p => p.Message).HasForeignKey(d => d.Id).Metadata.DependentToPrincipal.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
