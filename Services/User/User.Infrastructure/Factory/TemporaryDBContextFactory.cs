using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Reflection;
using User.Infrastructure.DBContext;

namespace User.Infrastructure.Factory
{
    /// <summary>
    /// A factory for creating <see cref="MessageServiceDBContext"/>. Would be used for Design-Time operations like migrations.
    /// </summary>
    public class TemporaryDBContextFactory : IDesignTimeDbContextFactory<UserServiceDBContext>
    {
        /// <summary>
        /// Creates a new instance of <see cref="MessageServiceDBContext"/>.
        /// </summary>
        /// <param name="args">Arguments provided by the design-time service.</param>
        /// <returns>An instance of <see cref="MessageServiceDBContext"/></returns>
        public UserServiceDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UserServiceDBContext>();

            optionsBuilder.UseNpgsql("",
                opt => opt.MigrationsAssembly(typeof(UserServiceDBContext).GetTypeInfo().Assembly.GetName().Name));

            return new UserServiceDBContext(optionsBuilder.Options);
        }
    }
}
