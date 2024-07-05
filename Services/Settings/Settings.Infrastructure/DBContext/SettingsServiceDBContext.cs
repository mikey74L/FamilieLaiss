using Microsoft.EntityFrameworkCore;
using Settings.Domain.Entities;
using Settings.Infrastructure.DBContext.Configurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Settings.Infrastructure.DBContext
{
    /// <summary>
    /// Entity-Framework-Core database context for picture convert service
    /// </summary>
    public class SettingsServiceDBContext: DbContext
    {
        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="options">The options for this context.</param>
        public SettingsServiceDBContext(DbContextOptions<SettingsServiceDBContext> options) : base(options)
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
            modelBuilder.ApplyConfiguration(new UserSettingsEntityTypeConfiguration());
        }
        #endregion

        #region DBSets
        public DbSet<UserSettings> UserSettings { get; set; }
        #endregion
    }
}
