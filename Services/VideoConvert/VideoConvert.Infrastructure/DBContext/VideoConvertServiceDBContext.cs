using InfrastructureHelper.Context;
using Microsoft.EntityFrameworkCore;
using VideoConvert.Domain.Entities;
using VideoConvert.Infrastructure.DBContext.Configurations;

namespace VideoConvert.Infrastructure.DBContext;

/// <summary>
/// Entity-Framework-Core database context for video convert service
/// </summary>
/// <remarks>
/// C'tor
/// </remarks>
/// <param name="options">The options for this context.</param>
public class VideoConvertServiceDbContext(DbContextOptions<VideoConvertServiceDbContext> options) :
    BaseContextFamilieLaiss<VideoConvertServiceDbContext>(options)
{
    #region Protected override

    /// <summary>
    /// Would be called when the model is creating to define special behaviour
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context</param>
    protected override void OnModelCreatingInternal(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ConvertStatusEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new UploadVideoEntityTypeConfiguration());
    }

    #endregion

    #region DBSets

    public DbSet<VideoConvertStatus> ConvertStatusEntries { get; set; }

    public DbSet<UploadVideo> UploadVideos { get; set; }

    #endregion
}