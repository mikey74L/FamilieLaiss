using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Reflection;
using Upload.Infrastructure.DBContext;

namespace PictureConvert.Infrastructure.Factory;

/// <summary>
/// A factory for creating <see cref="UploadServiceDBContext"/>. Would be used for Design-Time operations like migrations.
/// </summary>
public class TemporaryDBContextFactory : IDesignTimeDbContextFactory<UploadServiceDBContext>
{
    /// <summary>
    /// Creates a new instance of <see cref="UploadServiceDBContext"/>.
    /// </summary>
    /// <param name="args">Arguments provided by the design-time service.</param>
    /// <returns>An instance of <see cref="UploadServiceDBContext"/></returns>
    public UploadServiceDBContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<UploadServiceDBContext>();

        optionsBuilder.UseNpgsql("",
            opt => opt.MigrationsAssembly(typeof(UploadServiceDBContext).GetTypeInfo().Assembly.GetName().Name));

        return new UploadServiceDBContext(optionsBuilder.Options);
    }
}
