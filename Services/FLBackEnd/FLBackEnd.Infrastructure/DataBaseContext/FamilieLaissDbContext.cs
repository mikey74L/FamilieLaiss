using FLBackEnd.Domain.Entities;
using FLBackEnd.Infrastructure.Converter;
using FLBackEnd.Infrastructure.DataBaseContext.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FLBackEnd.Infrastructure.DatabaseContext;

public class FamilieLaissDbContext : DbContext
{
    #region C'tor

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="options">The options for this context.</param>
    public FamilieLaissDbContext(DbContextOptions<FamilieLaissDbContext> options) : base(options)
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
        //Calling model builders for all entities
        modelBuilder.ApplyConfiguration(new BlogEntryEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryValueEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MediaGroupEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MediaItemEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MediaItemCategoryValueEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new UploadIdentifierEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new UploadPictureEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new UploadVideoEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new VideoConvertStatusEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PictureConvertStatusEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new UserSettingEntityTypeConfiguration());
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<DateTimeOffset>()
            .HaveConversion<DateTimeOffsetConverter>();
    }
    #endregion

    #region DBSets
    public DbSet<BlogEntry> BlogEntries { get; set; }
    public DbSet<Category> Category { get; set; }
    public DbSet<CategoryValue> CategoryValues { get; set; }
    public DbSet<MediaGroup> MediaGroups { get; set; }
    public DbSet<MediaItem> MediaItems { get; set; }
    public DbSet<MediaItemCategoryValue> MediaItemCategoryValues { get; set; }
    public DbSet<UploadIdentifier> UploadIdentifiers { get; set; }
    public DbSet<UploadPicture> UploadPictures { get; set; }
    public DbSet<UploadVideo> UploadVideos { get; set; }
    public DbSet<VideoConvertStatus> VideoConvertStatusEntries { get; set; }
    public DbSet<PictureConvertStatus> PictureConvertStatusEntries { get; set; }
    public DbSet<UserSetting> UserSettings { get; set; }
    #endregion
}