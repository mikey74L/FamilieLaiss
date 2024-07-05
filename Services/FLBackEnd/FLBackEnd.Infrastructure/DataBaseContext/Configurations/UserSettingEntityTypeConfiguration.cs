using FLBackEnd.Domain.Entities;
using InfrastructureHelper.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FLBackEnd.Infrastructure.DataBaseContext.Configurations;

public class UserSettingEntityTypeConfiguration : EntityTypeConfigurationBase<UserSetting>
{
    protected override void ConfigureEntity(EntityTypeBuilder<UserSetting> modelBuilder)
    {
        CreateModelForIDColumn(modelBuilder, null);

        //Setting default values for properties
        modelBuilder.Property(q => q.GalleryCloseDimmer).HasDefaultValue(false);
        modelBuilder.Property(q => q.GalleryCloseEsc).HasDefaultValue(false);
        modelBuilder.Property(q => q.GalleryMouseWheelChangeSlide).HasDefaultValue(false);
        modelBuilder.Property(q => q.GalleryShowFullScreen).HasDefaultValue(true);
        modelBuilder.Property(q => q.GalleryShowThumbnails).HasDefaultValue(true);
        modelBuilder.Property(q => q.GalleryTransitionDuration).HasDefaultValue(600);
        modelBuilder.Property(q => q.GalleryTransitionType).HasDefaultValue("lg-fade");
        modelBuilder.Property(q => q.VideoAutoPlay).HasDefaultValue(true);
        modelBuilder.Property(q => q.VideoAutoPlayOtherVideos).HasDefaultValue(true);
        modelBuilder.Property(q => q.VideoLoop).HasDefaultValue(false);
        modelBuilder.Property(q => q.VideoTimeToPlayNextVideo).HasDefaultValue(5);
        modelBuilder.Property(q => q.VideoVolume).HasDefaultValue(100);
        modelBuilder.Property(q => q.VideoTimeSeekForwardRewind).HasDefaultValue(10);
        modelBuilder.Property(q => q.QuestionKeepUploadWhenDelete).HasDefaultValue(true);
        modelBuilder.Property(q => q.DefaultKeepUploadWhenDelete).HasDefaultValue(true);
        modelBuilder.Property(q => q.ShowButtonForward).HasDefaultValue(true);
        modelBuilder.Property(q => q.ShowButtonRewind).HasDefaultValue(true);
        modelBuilder.Property(q => q.ShowZoomMenu).HasDefaultValue(true);
        modelBuilder.Property(q => q.ShowPlayRateMenu).HasDefaultValue(true);
        modelBuilder.Property(q => q.ShowMirrorButton).HasDefaultValue(true);
        modelBuilder.Property(q => q.ShowQualityMenu).HasDefaultValue(true);
        modelBuilder.Property(q => q.ShowZoomInfo).HasDefaultValue(true);
        modelBuilder.Property(q => q.AllowZoomingWithMouseWheel).HasDefaultValue(false);
        modelBuilder.Property(q => q.ShowTooltipForCurrentPlaytime).HasDefaultValue(true);
        modelBuilder.Property(q => q.ShowTooltipForPlaytimeOnMouseCursor).HasDefaultValue(true);
    }
}
