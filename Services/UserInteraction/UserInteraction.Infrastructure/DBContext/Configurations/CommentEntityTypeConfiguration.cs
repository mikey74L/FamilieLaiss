using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserInteraction.Domain.Aggregates;

namespace UserInteraction.Infrastructure.DBContext.Configurations;

public class CommentEntityTypeConfiguration : EntityTypeConfigurationBase<Comment>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Comment> modelBuilder)
    {
        //Primary Key Column
        CreateModelForIDColumn(modelBuilder, "Sequence" + nameof(Comment));
    }
}
