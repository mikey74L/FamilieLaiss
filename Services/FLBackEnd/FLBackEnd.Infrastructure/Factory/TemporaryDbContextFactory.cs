using System.Reflection;
using FLBackEnd.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FLBackEnd.Infrastructure.Factory;

/// <summary>
/// A factory for creating <see cref="FamilieLaissDbContext"/>. Would be used for Design-Time operations like migrations.
/// </summary>
public class TemporaryDbContextFactory : IDesignTimeDbContextFactory<FamilieLaissDbContext>
{
    /// <summary>
    /// Creates a new instance of <see cref="FamilieLaissDbContext"/>.
    /// </summary>
    /// <param name="args">Arguments provided by the design-time service.</param>
    /// <returns>An instance of <see cref="FamilieLaissDbContext"/></returns>
    public FamilieLaissDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<FamilieLaissDbContext>();

        optionsBuilder.UseNpgsql("",
            opt => opt.MigrationsAssembly(typeof(FamilieLaissDbContext).GetTypeInfo().Assembly.GetName().Name));

        return new FamilieLaissDbContext(optionsBuilder.Options);
    }
}