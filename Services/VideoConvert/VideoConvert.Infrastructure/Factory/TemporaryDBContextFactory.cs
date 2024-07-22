using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Reflection;
using VideoConvert.Infrastructure.DBContext;

namespace VideoConvert.Infrastructure.Factory;

/// <summary>
/// A factory for creating <see cref="VideoConvertServiceDbContext"/>. Would be used for Design-Time operations like migrations.
/// </summary>
public class TemporaryDbContextFactory : IDesignTimeDbContextFactory<VideoConvertServiceDbContext>
{
    /// <summary>
    /// Creates a new instance of <see cref="VideoConvertServiceDbContext"/>.
    /// </summary>
    /// <param name="args">Arguments provided by the design-time service.</param>
    /// <returns>An instance of <see cref="VideoConvertServiceDbContext"/></returns>
    public VideoConvertServiceDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<VideoConvertServiceDbContext>();

        optionsBuilder.UseNpgsql("",
            opt => opt.MigrationsAssembly(typeof(VideoConvertServiceDbContext).GetTypeInfo().Assembly.GetName()
                .Name));

        return new VideoConvertServiceDbContext(optionsBuilder.Options);
    }
}