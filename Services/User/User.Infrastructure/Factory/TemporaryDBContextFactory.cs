using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Reflection;
using User.Infrastructure.DBContext;

namespace User.Infrastructure.Factory;

/// <summary>
/// A factory for creating <see cref="UserServiceDBContext"/>. Would be used for Design-Time operations like migrations.
/// </summary>
public class TemporaryDbContextFactory : IDesignTimeDbContextFactory<UserServiceDbContext>
{
    /// <summary>
    /// Creates a new instance of <see cref="UserServiceDbContext"/>.
    /// </summary>
    /// <param name="args">Arguments provided by the design-time service.</param>
    /// <returns>An instance of <see cref="UserServiceDBContext"/></returns>
    public UserServiceDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<UserServiceDbContext>();

        optionsBuilder.UseNpgsql("",
            opt => opt.MigrationsAssembly(typeof(UserServiceDbContext).GetTypeInfo().Assembly.GetName().Name));

        return new UserServiceDbContext(optionsBuilder.Options);
    }
}
