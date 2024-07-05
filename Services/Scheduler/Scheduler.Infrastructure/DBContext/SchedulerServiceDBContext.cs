using Microsoft.EntityFrameworkCore;
using Scheduler.Domain.Aggregates;
using Scheduler.Domain.Entities;
using Scheduler.Infrastructure.DBContext.Configurations;

namespace Scheduler.Infrastructure.DBContext
{
    /// <summary>
    /// Entity-Framework-Core database context for scheduler service
    /// </summary>
    public class SchedulerServiceDBContext: DbContext
    {
        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="options">The options for this context.</param>
        public SchedulerServiceDBContext(DbContextOptions<SchedulerServiceDBContext> options) : base(options)
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
            //Aufrufen des Model-Builders für Scheduler Resource
            modelBuilder.ApplyConfiguration(new SchedulerResourceEntityTypeConfiguration());

            //Aufrufen des Model-Builders für Scheduler Event
            modelBuilder.ApplyConfiguration(new SchedulerEventEntityTypeConfiguration());
        }
        #endregion

        #region DBSets
        public DbSet<SchedulerResource> SchedulerResources { get; set; }
        public DbSet<SchedulerEvent> SchedulerEvents { get; set; }
        #endregion
    }
}
