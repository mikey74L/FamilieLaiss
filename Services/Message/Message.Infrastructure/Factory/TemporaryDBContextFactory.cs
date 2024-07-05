using Message.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Reflection;

namespace Message.Infrastructure.Factory
{
    /// <summary>
    /// A factory for creating <see cref="MessageServiceDBContext"/>. Would be used for Design-Time operations like migrations.
    /// </summary>
    public class TemporaryDBContextFactory : IDesignTimeDbContextFactory<MessageServiceDBContext>
    {
        /// <summary>
        /// Creates a new instance of <see cref="MessageServiceDBContext"/>.
        /// </summary>
        /// <param name="args">Arguments provided by the design-time service.</param>
        /// <returns>An instance of <see cref="MessageServiceDBContext"/></returns>
        public MessageServiceDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MessageServiceDBContext>();

            optionsBuilder.UseNpgsql("",
                opt => opt.MigrationsAssembly(typeof(MessageServiceDBContext).GetTypeInfo().Assembly.GetName().Name));

            return new MessageServiceDBContext(optionsBuilder.Options);
        }
    }
}
