using InfrastructureHelper.Context;
using Microsoft.EntityFrameworkCore;
using PictureConvert.Domain.Entities;
using PictureConvert.Infrastructure.DBContext.Configurations;

namespace PictureConvert.Infrastructure.DBContext;

/// <summary>
/// Entity-Framework-Core database context for picture convert service
/// </summary>
/// <remarks>
/// C'tor
/// </remarks>
/// <param name="options">The options for this context.</param>
public class PictureConvertServiceDbContext(DbContextOptions<PictureConvertServiceDbContext> options)
    : BaseContextFamilieLaiss<PictureConvertServiceDbContext>(options)
{
    #region Protected override

    /// <summary>
    /// Would be called when the model is creating to define special behaviour
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context</param>
    protected override void OnModelCreatingInternal(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ConvertStatusEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new UploadPictureEntityTypeConfiguration());
    }

    #endregion

    #region DBSets

    public DbSet<PictureConvertStatus> ConvertStatusEntries { get; set; }

    public DbSet<UploadPicture> UploadPictures { get; set; }

    #endregion
}