using Catalog.Domain.Aggregates;
using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Infrastructure.DBContext.Configurations
{
    public class MediaItemCategoryValueEntityTypeConfiguration : EntityTypeConfigurationBase<MediaItemCategoryValue>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<MediaItemCategoryValue> modelBuilder)
        {
            //Primary Key Column
            CreateModelForIDColumn(modelBuilder, "Sequence" + nameof(MediaItemCategoryValue));
        }
    }
}
