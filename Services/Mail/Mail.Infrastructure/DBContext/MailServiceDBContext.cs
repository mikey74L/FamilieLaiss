using Mail.Infrastructure.DBContext.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mail.Infrastructure.DBContext
{
    /// <summary>
    /// Entity-Framework-Core database context for mail service
    /// </summary>
    public class MailServiceDBContext: DbContext
    {
        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="options">The options for this context.</param>
        public MailServiceDBContext(DbContextOptions<MailServiceDBContext> options) : base(options)
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
            modelBuilder.ApplyConfiguration(new MailEntityTypeConfiguration());
        }
        #endregion

        #region DBSets
        public DbSet<Mail.Domain.Entities.Mail> Mails { get; set; }
        #endregion
    }
}
