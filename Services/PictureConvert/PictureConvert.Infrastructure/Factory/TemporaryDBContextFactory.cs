using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PictureConvert.Infrastructure.DBContext;
using System.Reflection;

namespace PictureConvert.Infrastructure.Factory;

/// <summary>
/// A factory for creating <see cref="PictureConvertServiceDbContext"/>. Would be used for Design-Time operations like migrations.
/// </summary>
public class TemporaryDbContextFactory : IDesignTimeDbContextFactory<PictureConvertServiceDbContext>
{
    /// <summary>
    /// Creates a new instance of <see cref="PictureConvertServiceDbContext"/>.
    /// </summary>
    /// <param name="args">Arguments provided by the design-time service.</param>
    /// <returns>An instance of <see cref="PictureConvertServiceDbContext"/></returns>
    public PictureConvertServiceDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PictureConvertServiceDbContext>();

        optionsBuilder.UseNpgsql("",
            opt => opt.MigrationsAssembly(typeof(PictureConvertServiceDbContext).GetTypeInfo().Assembly.GetName()
                .Name));

        return new PictureConvertServiceDbContext(optionsBuilder.Options);
    }
}