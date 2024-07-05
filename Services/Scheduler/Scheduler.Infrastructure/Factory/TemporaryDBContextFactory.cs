using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Scheduler.Infrastructure.DBContext;
using System.Reflection;

namespace Scheduler.Infrastructure.Factory
{
    /// <summary>
    /// A factory for creating <see cref="SchedulerServiceDBContext"/>. Would be used for Design-Time operations like migrations.
    /// </summary>
    public class TemporaryDBContextFactory : IDesignTimeDbContextFactory<SchedulerServiceDBContext>
    {
        /// <summary>
        /// Creates a new instance of <see cref="SettingsServiceDBContext"/>.
        /// </summary>
        /// <param name="args">Arguments provided by the design-time service.</param>
        /// <returns>An instance of <see cref="SettingsServiceDBContext"/></returns>
        public SchedulerServiceDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SchedulerServiceDBContext>();

            optionsBuilder.UseNpgsql("",
                opt => opt.MigrationsAssembly(typeof(SchedulerServiceDBContext).GetTypeInfo().Assembly.GetName().Name));

            return new SchedulerServiceDBContext(optionsBuilder.Options);
        }
    }
}
