using Microsoft.EntityFrameworkCore;
using Upload.Domain.Entities;
using Upload.Infrastructure.DBContext.Configurations;

namespace Upload.Infrastructure.DBContext;

/// <summary>
/// Entity-Framework-Core database context for upload service
/// </summary>
public class UploadServiceDbContext : DbContext
{
    #region C'tor

    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="options">The options for this context.</param>
    public UploadServiceDbContext(DbContextOptions<UploadServiceDbContext> options) : base(options)
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
        modelBuilder.ApplyConfiguration(new UploadPictureEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new UploadVideoEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new UploadIdentifierEntityTypeConfiguration());
    }

    #endregion

    #region DBSets

    public DbSet<UploadIdentifier> UploadIdentifiers { get; set; }

    public DbSet<UploadPicture> UploadPictures { get; set; }

    public DbSet<UploadVideo> UploadVideos { get; set; }

    #endregion
}