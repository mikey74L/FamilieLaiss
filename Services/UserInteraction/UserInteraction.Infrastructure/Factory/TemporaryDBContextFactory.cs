using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Reflection;
using UserInteraction.Infrastructure.DBContext;

namespace UserInteraction.Infrastructure.Factory
{
    /// <summary>
    /// A factory for creating <see cref="RatingServiceDBContext"/>. Would be used for Design-Time operations like migrations.
    /// </summary>
    public class TemporaryDBContextFactory : IDesignTimeDbContextFactory<UserInteractionServiceDBContext>
    {
        /// <summary>
        /// Creates a new instance of <see cref="UserInteractionServiceDBContext"/>.
        /// </summary>
        /// <param name="args">Arguments provided by the design-time service.</param>
        /// <returns>An instance of <see cref="UserInteractionServiceDBContext"/></returns>
        public UserInteractionServiceDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UserInteractionServiceDBContext>();

            optionsBuilder.UseNpgsql("",
                opt => opt.MigrationsAssembly(typeof(UserInteractionServiceDBContext).GetTypeInfo().Assembly.GetName().Name));

            return new UserInteractionServiceDBContext(optionsBuilder.Options);
        }
    }
}
