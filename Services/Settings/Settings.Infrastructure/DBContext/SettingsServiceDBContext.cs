using InfrastructureHelper.Context;
using Microsoft.EntityFrameworkCore;
using Settings.Domain.Entities;
using Settings.Infrastructure.DBContext.Configurations;

namespace Settings.Infrastructure.DBContext;

/// <summary>
/// Entity-Framework-Core database context for user settings service
/// </summary>
/// <remarks>
/// C'tor
/// </remarks>
/// <param name="options">The options for this context.</param>
public class SettingsServiceDbContext(DbContextOptions<SettingsServiceDbContext> options) :
    BaseContextFamilieLaiss<SettingsServiceDbContext>(options)
{
    #region Protected override
    /// <summary>
    /// Would be called when the model is creating to define special behaviour
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context</param>
    protected override void OnModelCreatingInternal(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserSettingsEntityTypeConfiguration());
    }
    #endregion

    #region DBSets
    public DbSet<UserSetting> UserSettings { get; set; }
    #endregion
}
