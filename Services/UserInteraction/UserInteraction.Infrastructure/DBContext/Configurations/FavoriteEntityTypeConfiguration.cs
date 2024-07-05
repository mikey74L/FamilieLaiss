using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserInteraction.Domain.Aggregates;

namespace UserInteraction.Infrastructure.DBContext.Configurations
{
    public class FavoriteEntityTypeConfiguration : EntityTypeConfigurationBase<Favorite>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Favorite> modelBuilder)
        {
            //Primary Key Column
            CreateModelForIDColumn(modelBuilder, "Sequence" + nameof(Favorite));
        }
    }
}
