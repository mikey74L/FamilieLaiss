using InfrastructureHelper.Context;
using Microsoft.EntityFrameworkCore;
using Upload.Domain.Entities;
using Upload.Infrastructure.DBContext.Configurations;

namespace Upload.Infrastructure.DBContext;

/// <summary>
/// Entity-Framework-Core database context for upload service
/// </summary>
/// <remarks>
/// C'tor
/// </remarks>
/// <param name="options">The options for this context.</param>
public class UploadServiceDbContext(DbContextOptions<UploadServiceDbContext> options) :
    BaseContextFamilieLaiss<UploadServiceDbContext>(options)
{
    #region Protected override

    /// <summary>
    /// Would be called when the model is creating to define special behaviour
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context</param>
    protected override void OnModelCreatingInternal(ModelBuilder modelBuilder)
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