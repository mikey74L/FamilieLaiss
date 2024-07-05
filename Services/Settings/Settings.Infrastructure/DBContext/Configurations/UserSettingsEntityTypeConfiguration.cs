using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Settings.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Settings.Infrastructure.DBContext.Configurations
{
    internal class UserSettingsEntityTypeConfiguration: EntityTypeConfigurationBase<UserSettings>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<UserSettings> modelBuilder)
        {
            //Primary Key Column
            CreateModelForIDColumn(modelBuilder, null);

            //Setting IsRequired for all columns
            modelBuilder.Property(q => q.GalleryCloseDimmer).IsRequired().HasDefaultValue(false);
            modelBuilder.Property(q => q.GalleryCloseEsc).IsRequired().HasDefaultValue(false);
            modelBuilder.Property(q => q.GalleryMouseWheelChangeSlide).IsRequired().HasDefaultValue(false);
            modelBuilder.Property(q => q.GalleryShowFullScreen).IsRequired().HasDefaultValue(true);
            modelBuilder.Property(q => q.GalleryShowThumbnails).IsRequired().HasDefaultValue(true);
            modelBuilder.Property(q => q.GalleryTransitionDuration).IsRequired().HasDefaultValue(600);
            modelBuilder.Property(q => q.GalleryTransitionType).IsRequired().HasDefaultValue("lg-fade");
            modelBuilder.Property(q => q.SimpleFilter).IsRequired().HasDefaultValue(true);
            modelBuilder.Property(q => q.VideoAutoPlay).IsRequired().HasDefaultValue(true);
            modelBuilder.Property(q => q.VideoAutoPlayOtherVideos).IsRequired().HasDefaultValue(true);
            modelBuilder.Property(q => q.VideoLoop).IsRequired().HasDefaultValue(false);
            modelBuilder.Property(q => q.VideoTimeToPlayNextVideo).IsRequired().HasDefaultValue(5);
            modelBuilder.Property(q => q.VideoVolume).IsRequired().HasDefaultValue(100);
            modelBuilder.Property(q => q.ShowButtonForward).IsRequired().HasDefaultValue(true);
            modelBuilder.Property(q => q.ShowButtonRewind).IsRequired().HasDefaultValue(true);
            modelBuilder.Property(q => q.ShowZoomMenu).IsRequired().HasDefaultValue(true);
            modelBuilder.Property(q => q.ShowPlayRateMenu).IsRequired().HasDefaultValue(true);
            modelBuilder.Property(q => q.ShowMirrorButton).IsRequired().HasDefaultValue(true);
            modelBuilder.Property(q => q.ShowContextMenu).IsRequired().HasDefaultValue(true);
            modelBuilder.Property(q => q.ShowQualityMenu).IsRequired().HasDefaultValue(true);
            modelBuilder.Property(q => q.ShowTooltipForPlaytimeOnMouseCursor).IsRequired().HasDefaultValue(true);
            modelBuilder.Property(q => q.ShowTooltipForCurrentPlaytime).IsRequired().HasDefaultValue(true);
            modelBuilder.Property(q => q.ShowZoomInfo).IsRequired().HasDefaultValue(true);
            modelBuilder.Property(q => q.AllowZoomingWithMouseWheel).IsRequired().HasDefaultValue(true);
            modelBuilder.Property(q => q.QuestionKeepUploadWhenDelete).IsRequired().HasDefaultValue(true);
            modelBuilder.Property(q => q.DefaultKeepUploadWhenDelete).IsRequired().HasDefaultValue(true);
        }
    }
}
