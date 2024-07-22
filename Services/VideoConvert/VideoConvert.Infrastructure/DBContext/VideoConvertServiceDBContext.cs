using Microsoft.EntityFrameworkCore;
using VideoConvert.Domain.Entities;
using VideoConvert.Infrastructure.DBContext.Configurations;

namespace VideoConvert.Infrastructure.DBContext;

/// <summary>
/// Entity-Framework-Core database context for video convert service
/// </summary>
public class VideoConvertServiceDbContext : DbContext
{
    #region C'tor

    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="options">The options for this context.</param>
    public VideoConvertServiceDbContext(DbContextOptions<VideoConvertServiceDbContext> options) : base(options)
    {
    }

    #endregion

    #region Protected override

    /// <summary>
    /// Would be called when the model is creating to define special behaviour
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Aufrufen des Model-Builders für ConvertStatus
        modelBuilder.ApplyConfiguration(new ConvertStatusEntityTypeConfiguration());

        //Aufrufen des Model-Builders für Upload-Video
        modelBuilder.ApplyConfiguration(new UploadVideoEntityTypeConfiguration());
    }

    #endregion

    #region DBSets

    public DbSet<VideoConvertStatus> ConvertStatusEntries { get; set; }

    public DbSet<UploadVideo> UploadVideos { get; set; }

    #endregion
}