using Catalog.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Reflection;

namespace Catalog.Infrastructure.Factory
{
    /// <summary>
    /// A factory for creating <see cref="CatalogServiceDbContext"/>. Would be used for Design-Time operations like migrations.
    /// </summary>
    public class TemporaryDbContextFactory : IDesignTimeDbContextFactory<CatalogServiceDbContext>
    {
        /// <summary>
        /// Creates a new instance of <see cref="CatalogServiceDbContext"/>.
        /// </summary>
        /// <param name="args">Arguments provided by the design-time service.</param>
        /// <returns>An instance of <see cref="CatalogServiceDbContext"/></returns>
        public CatalogServiceDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CatalogServiceDbContext>();

            optionsBuilder.UseNpgsql("",
                opt => opt.MigrationsAssembly(typeof(CatalogServiceDbContext).GetTypeInfo().Assembly.GetName().Name));

            return new CatalogServiceDbContext(optionsBuilder.Options);
        }
    }
}
