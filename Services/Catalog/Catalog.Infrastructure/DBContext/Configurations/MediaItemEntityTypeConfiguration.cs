using Catalog.Domain.Aggregates;
using Catalog.Domain.Entities;
using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.DBContext.Configurations
{
    public class MediaItemEntityTypeConfiguration : EntityTypeConfigurationBase<MediaItem>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<MediaItem> modelBuilder)
        {
            //Primary Key Column
            CreateModelForIDColumn(modelBuilder, "Sequence" + nameof(MediaItem));

            //Media-Type Property
            modelBuilder.Property(x => x.MediaType).IsRequired();

            //OnlyFamily Property
            modelBuilder.Property(x => x.OnlyFamily).IsRequired();

            //Name_German-Property
            modelBuilder.Property(x => x.NameGerman).IsRequired().HasMaxLength(200);

            //Name_English-Property
            modelBuilder.Property(x => x.NameEnglish).IsRequired().HasMaxLength(200);

            //Description_German-Property
            modelBuilder.Property(x => x.DescriptionGerman).HasMaxLength(2000);

            //Description_English-Property
            modelBuilder.Property(x => x.DescriptionEnglish).HasMaxLength(2000);

            //UploadPictureId
            modelBuilder.HasOne(d => d.UploadPicture).WithOne(p => p.MediaItem).HasForeignKey<UploadPicture>(x => x.MediaItemID).Metadata.DependentToPrincipal.SetPropertyAccessMode(PropertyAccessMode.Field);

            //UploadVideoId
            modelBuilder.HasOne(d => d.UploadVideo).WithOne(p => p.MediaItem).HasForeignKey<UploadVideo>(x => x.MediaItemID).Metadata.DependentToPrincipal.SetPropertyAccessMode(PropertyAccessMode.Field);

            //Eindeutigen Index für Media-Type und Name-German erstellen
            modelBuilder.HasIndex(x => new { x.MediaType, x.NameGerman }).IsUnique();

            //Eindeutigen Index für Media-Type und Name-English erstellen
            modelBuilder.HasIndex(x => new { x.MediaType, x.NameEnglish }).IsUnique();

            //Liste für die Media-Item-Category-Values
            modelBuilder.HasMany(d => d.MediaItemCategoryValues).WithOne(p => p.MediaItem).HasForeignKey(d => d.MediaItemID).Metadata.DependentToPrincipal.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
