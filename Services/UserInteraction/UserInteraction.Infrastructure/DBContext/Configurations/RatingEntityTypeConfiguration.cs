using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserInteraction.Domain.Aggregates;

namespace UserInteraction.Infrastructure.DBContext.Configurations
{
    public class RatingEntityTypeConfiguration : EntityTypeConfigurationBase<Rating>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Rating> modelBuilder)
        {
            //Primary Key Column
            CreateModelForIDColumn(modelBuilder, "Sequence" + nameof(Rating));

            //Property für "Value"
            modelBuilder.Property(p => p.Value).IsRequired().HasDefaultValue(0);
        }
    }
}
