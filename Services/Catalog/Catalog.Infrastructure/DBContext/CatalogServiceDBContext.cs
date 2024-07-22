using Catalog.Domain.Aggregates;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.DBContext.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.DBContext;

/// <summary>
/// Entity-Framework-Core database context for catalog service
/// </summary>
/// <remarks>
/// C'tor
/// </remarks>
/// <param name="options">The options for this context.</param>
public class CatalogServiceDbContext(DbContextOptions<CatalogServiceDbContext> options) : DbContext(options)
{
    #region Protected override

    /// <summary>
    /// Would be called when the model is creating to define special behaviour
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Aufrufen des Model-Builders für ConvertStatus
        modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryValueEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MediaGroupEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MediaItemEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MediaItemCategoryValueEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new UploadPictureEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new UploadVideoEntityTypeConfiguration());
    }

    #endregion

    #region DBSets

    public DbSet<Category> Category { get; set; }
    public DbSet<CategoryValue> CategoryValues { get; set; }
    public DbSet<MediaGroup> MediaGroups { get; set; }
    public DbSet<MediaItem> MediaItems { get; set; }
    public DbSet<MediaItemCategoryValue> MediaItemCategoryValues { get; set; }
    public DbSet<UploadPicture> UploadPictures { get; set; }
    public DbSet<UploadVideo> UploadVideos { get; set; }

    #endregion
}