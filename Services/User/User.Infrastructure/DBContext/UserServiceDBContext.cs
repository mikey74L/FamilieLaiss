using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Infrastructure.DBContext.Configurations;

namespace User.Infrastructure.DBContext
{
    /// <summary>
    /// Entity-Framework-Core database context for picture convert service
    /// </summary>
    public class UserServiceDBContext : DbContext
    {
        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="options">The options for this context.</param>
        public UserServiceDBContext(DbContextOptions<UserServiceDBContext> options) : base(options)
        {
        }
        #endregion

        #region Protected override
        /// <summary>
        /// Would be called when the model is creating to define special behaviour
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Aufrufen des Model-Builders für ConvertStatus
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CountryEntityTypeConfiguration());
        }
        #endregion

        #region DBSets
        public DbSet<User.Domain.Aggregates.User> Users { get; set; }
        public DbSet<User.Domain.Aggregates.Country> Countries { get; set; }
        #endregion
    }
}
