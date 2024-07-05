using Blog.Domain.Entities;
using Blog.Infrastructure.DBContext.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.DBContext
{
    /// <summary>
    /// Entity-Framework-Core database context for blog service
    /// </summary>
    public class BlogServiceDBContext: DbContext
    {
        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="options">The options for this context.</param>
        public BlogServiceDBContext(DbContextOptions<BlogServiceDBContext> options) : base(options)
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
            modelBuilder.ApplyConfiguration(new BlogEntryEntityTypeConfiguration());
        }
        #endregion
      
        #region DBSets
        public DbSet<BlogEntry> BlogEntries { get; set; }
        #endregion
    }
}
