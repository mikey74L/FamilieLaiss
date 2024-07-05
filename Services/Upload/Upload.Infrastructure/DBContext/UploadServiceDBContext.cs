using Microsoft.EntityFrameworkCore;
using Upload.Domain.Entities;
using Upload.Infrastructure.DBContext.Configurations;

namespace Upload.Infrastructure.DBContext;

/// <summary>
/// Entity-Framework-Core database context for upload service
/// </summary>
public class UploadServiceDBContext : DbContext
{
    #region C'tor
    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="options">The options for this context.</param>
    public UploadServiceDBContext(DbContextOptions<UploadServiceDBContext> options) : base(options)
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
        modelBuilder.ApplyConfiguration(new UploadPictureEntityTypeConfiguration());

        //Aufrufen des Model-Builders für Upload-Picture
        modelBuilder.ApplyConfiguration(new UploadVideoEntityTypeConfiguration());

        //Aufrufen des Model-Builders für Upload-Portrait
        modelBuilder.ApplyConfiguration(new UploadPortraitEntityTypeConfiguration());

        //Aufrufen des Model-Builders für Upload-Identifier
        modelBuilder.ApplyConfiguration(new UploadIdentifierEntityTypeConfiguration());
    }
    #endregion

    #region DBSets
    public DbSet<UploadIdentifier> UploadIdentifiers { get; set; }

    public DbSet<UploadPicture> UploadPictures { get; set; }

    public DbSet<UploadVideo> UploadVideos { get; set; }

    public DbSet<UploadPortrait> UploadPortraits { get; set; }
    #endregion
}
