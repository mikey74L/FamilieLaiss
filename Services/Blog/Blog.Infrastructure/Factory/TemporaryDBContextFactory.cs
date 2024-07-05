using Blog.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Reflection;

namespace Blog.Infrastructure.Factory
{
    /// <summary>
    /// A factory for creating <see cref="BlogServiceDBContext"/>. Would be used for Design-Time operations like migrations.
    /// </summary>
    public class TemporaryDBContextFactory : IDesignTimeDbContextFactory<BlogServiceDBContext>
    {
        /// <summary>
        /// Creates a new instance of <see cref="CatalogServiceDBContext"/>.
        /// </summary>
        /// <param name="args">Arguments provided by the design-time service.</param>
        /// <returns>An instance of <see cref="CatalogServiceDBContext"/></returns>
        public BlogServiceDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BlogServiceDBContext>();

            optionsBuilder.UseNpgsql("",
                opt => opt.MigrationsAssembly(typeof(BlogServiceDBContext).GetTypeInfo().Assembly.GetName().Name));

            return new BlogServiceDBContext(optionsBuilder.Options);
        }
    }
}
