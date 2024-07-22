using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Reflection;
using Upload.Infrastructure.DBContext;

namespace Upload.Infrastructure.Factory;

/// <summary>
/// A factory for creating <see cref="UploadServiceDbContext"/>. Would be used for Design-Time operations like migrations.
/// </summary>
public class TemporaryDbContextFactory : IDesignTimeDbContextFactory<UploadServiceDbContext>
{
    /// <summary>
    /// Creates a new instance of <see cref="UploadServiceDbContext"/>.
    /// </summary>
    /// <param name="args">Arguments provided by the design-time service.</param>
    /// <returns>An instance of <see cref="UploadServiceDbContext"/></returns>
    public UploadServiceDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<UploadServiceDbContext>();

        optionsBuilder.UseNpgsql("",
            opt => opt.MigrationsAssembly(typeof(UploadServiceDbContext).GetTypeInfo().Assembly.GetName().Name));

        return new UploadServiceDbContext(optionsBuilder.Options);
    }
}