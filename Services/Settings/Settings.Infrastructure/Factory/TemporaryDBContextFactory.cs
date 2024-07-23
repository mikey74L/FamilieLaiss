using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Settings.Infrastructure.DBContext;
using System.Reflection;

namespace Settings.Infrastructure.Factory;

/// <summary>
/// A factory for creating <see cref="SettingServiceDBContext"/>. Would be used for Design-Time operations like migrations.
/// </summary>
public class TemporaryDbContextFactory : IDesignTimeDbContextFactory<SettingsServiceDbContext>
{
    /// <summary>
    /// Creates a new instance of <see cref="SettingsServiceDbContext"/>.
    /// </summary>
    /// <param name="args">Arguments provided by the design-time service.</param>
    /// <returns>An instance of <see cref="SettingsServiceDbContext"/></returns>
    public SettingsServiceDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SettingsServiceDbContext>();

        optionsBuilder.UseNpgsql("",
            opt => opt.MigrationsAssembly(typeof(SettingsServiceDbContext).GetTypeInfo().Assembly.GetName().Name));

        return new SettingsServiceDbContext(optionsBuilder.Options);
    }
}
