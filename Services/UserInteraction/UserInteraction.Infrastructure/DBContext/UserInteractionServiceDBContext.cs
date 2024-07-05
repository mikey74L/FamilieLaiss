using Microsoft.EntityFrameworkCore;
using UserInteraction.Domain.Aggregates;
using UserInteraction.Infrastructure.DBContext.Configurations;

namespace UserInteraction.Infrastructure.DBContext
{
    /// <summary>
    /// Entity-Framework-Core database context for mail service
    /// </summary>
    public class UserInteractionServiceDBContext: DbContext
    {
        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="options">The options for this context.</param>
        public UserInteractionServiceDBContext(DbContextOptions<UserInteractionServiceDBContext> options) : base(options)
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
            modelBuilder.ApplyConfiguration(new UserInteractionInfoEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RatingEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CommentEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FavoriteEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserAccountEntityTypeConfiguration());
        }
        #endregion

        #region DBSets
        public DbSet<UserInteractionInfo> UserInteractionInfos { get; set; }
        
        public DbSet<UserAccount> UserAccounts { get; set; }

        private DbSet<Rating> Ratings { get; set; }

        private DbSet<Comment> Comments { get; set; }

        private DbSet<Favorite> Favorites { get; set; }
        #endregion
    }
}
