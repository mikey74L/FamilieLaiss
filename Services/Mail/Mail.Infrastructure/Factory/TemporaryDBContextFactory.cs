using Mail.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Reflection;

namespace Mail.Infrastructure.Factory
{
    /// <summary>
    /// A factory for creating <see cref="MailServiceDBContext"/>. Would be used for Design-Time operations like migrations.
    /// </summary>
    public class TemporaryDBContextFactory : IDesignTimeDbContextFactory<MailServiceDBContext>
    {
        /// <summary>
        /// Creates a new instance of <see cref="MailServiceDBContext"/>.
        /// </summary>
        /// <param name="args">Arguments provided by the design-time service.</param>
        /// <returns>An instance of <see cref="MailServiceDBContext"/></returns>
        public MailServiceDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MailServiceDBContext>();

            optionsBuilder.UseNpgsql("",
                opt => opt.MigrationsAssembly(typeof(MailServiceDBContext).GetTypeInfo().Assembly.GetName().Name));

            return new MailServiceDBContext(optionsBuilder.Options);
        }
    }
}
