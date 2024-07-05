using FLBackEnd.Domain.Entities;
using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLBackEnd.Infrastructure.DataBaseContext.Configurations;

public class BlogEntryEntityTypeConfiguration : EntityTypeConfigurationBase<BlogEntry>
{
    protected override void ConfigureEntity(EntityTypeBuilder<BlogEntry> modelBuilder)
    {
        //Primary Key Column
        CreateModelForIDColumn(modelBuilder, "Sequence" + nameof(BlogEntry));
    }
}