using Message.Domain.Aggregates;
using Message.Infrastructure.DBContext.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Message.Infrastructure.DBContext
{
    /// <summary>
    /// Entity-Framework-Core database context for picture convert service
    /// </summary>
    public class MessageServiceDBContext : DbContext
    {
        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="options">The options for this context.</param>
        public MessageServiceDBContext(DbContextOptions<MessageServiceDBContext> options) : base(options)
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
            modelBuilder.ApplyConfiguration(new MessageEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MessageUserEntityTypeConfiguration());
        }
        #endregion

        #region DBSets
        public DbSet<Message.Domain.Aggregates.Message> Messages { get; set; }
        private DbSet<MessageUser> MessageUsers { get; set; }
        #endregion
    }
}
