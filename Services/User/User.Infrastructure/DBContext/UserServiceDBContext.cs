using InfrastructureHelper.Context;
using Microsoft.EntityFrameworkCore;
using User.Domain.Aggregates;
using User.Infrastructure.DBContext.Configurations;

namespace User.Infrastructure.DBContext;

/// <summary>
/// Entity-Framework-Core database context for picture convert service
/// </summary>
/// <remarks>
/// C'tor
/// </remarks>
/// <param name="options">The options for this context.</param>
public class UserServiceDbContext(DbContextOptions<UserServiceDbContext> options) :
    BaseContextFamilieLaiss<UserServiceDbContext>(options)
{
    #region Protected override
    /// <summary>
    /// Would be called when the model is creating to define special behaviour
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context</param>
    protected override void OnModelCreatingInternal(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserAccountEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CountryEntityTypeConfiguration());
    }
    #endregion

    #region DBSets
    public DbSet<UserAccount> UserAccounts { get; set; }
    public DbSet<Country> Countries { get; set; }
    #endregion
}
