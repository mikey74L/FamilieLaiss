using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Settings.Infrastructure.DBContext;
using System.Reflection;

namespace Settings.Infrastructure.Factory
{
    /// <summary>
    /// A factory for creating <see cref="SettingServiceDBContext"/>. Would be used for Design-Time operations like migrations.
    /// </summary>
    public class TemporaryDBContextFactory : IDesignTimeDbContextFactory<SettingsServiceDBContext>
    {
        /// <summary>
        /// Creates a new instance of <see cref="SettingsServiceDBContext"/>.
        /// </summary>
        /// <param name="args">Arguments provided by the design-time service.</param>
        /// <returns>An instance of <see cref="SettingsServiceDBContext"/></returns>
        public SettingsServiceDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SettingsServiceDBContext>();

            optionsBuilder.UseNpgsql("",
                opt => opt.MigrationsAssembly(typeof(SettingsServiceDBContext).GetTypeInfo().Assembly.GetName().Name));

            return new SettingsServiceDBContext(optionsBuilder.Options);
        }
    }
}
