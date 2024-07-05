using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfrastructureHelper.DatabaseContext
{
    public abstract class EntityTypeConfigurationBase<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class
    {
        public void Configure(EntityTypeBuilder<TEntity> modelBuilder)
        {
            //Ignorieren des DomainEvents-Fields
            IgnoreDomainEvents(modelBuilder);

            //Konfigurieren der restlichen Properties
            ConfigureEntity(modelBuilder);
        }

        protected abstract void ConfigureEntity(EntityTypeBuilder<TEntity> modelBuilder);

        protected void CreateModelForIDColumn(EntityTypeBuilder<TEntity> modelBuilder, string nameForSequence)
        {
            modelBuilder.HasKey("Id");
            if (!string.IsNullOrEmpty(nameForSequence))
            {
                modelBuilder.Property("Id").UseHiLo(nameForSequence);
            }
            else
            {
                modelBuilder.Property("Id").ValueGeneratedNever();
            }
            modelBuilder.Property("Id").IsRequired();
        }

        protected void IgnoreDomainEvents(EntityTypeBuilder<TEntity> modelBuilder)
        {
            modelBuilder.Ignore("DomainEvents");
        }
    }
}
