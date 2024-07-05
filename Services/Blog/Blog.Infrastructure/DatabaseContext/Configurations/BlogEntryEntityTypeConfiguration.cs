using Blog.Domain.Entities;
using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Infrastructure.DBContext.Configurations
{
    public class BlogEntryEntityTypeConfiguration : EntityTypeConfigurationBase<BlogEntry>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<BlogEntry> modelBuilder)
        {
            //Primary Key Column
            CreateModelForIDColumn(modelBuilder, "Sequence" + nameof(BlogEntry));

            //HeaderGerman-Property
            modelBuilder.Property(x => x.HeaderGerman).IsRequired().HasMaxLength(200);

            //HeaderEnglish-Property
            modelBuilder.Property(x => x.HeaderEnglish).IsRequired().HasMaxLength(200);

            //TextGerman-Property
            modelBuilder.Property(x => x.TextGerman).IsRequired();

            //TextEnglish-Property
            modelBuilder.Property(x => x.TextEnglish).IsRequired();
        }
    }
}
