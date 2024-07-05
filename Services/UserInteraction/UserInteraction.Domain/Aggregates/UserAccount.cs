using DomainHelper.AbstractClasses;
using DomainHelper.Exceptions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserInteraction.Domain.Aggregates
{
    public class UserAccount : EntityModify<string>
    {
        #region Private Members
        private readonly ILazyLoader _LazyLoader;
        #endregion

        #region Properties
        /// <summary>
        /// Username for the user
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// Count of ratings (For faster access as property)
        /// </summary>
        public int RatingCount { get; private set; }

        /// <summary>
        /// Count of comments (For faster access as property)
        /// </summary>
        public int CommentCount { get; private set; }

        /// <summary>
        /// Count of favorites (For faster access as property)
        /// </summary>
        public int FavoriteCount { get; private set; }

        /// <summary>
        /// List of related raitings
        /// </summary>
        private HashSet<Rating> _Ratings;
        public IEnumerable<Rating> Ratings => _Ratings;

        /// <summary>
        /// List of related comments
        /// </summary>
        private HashSet<Comment> _Comments;
        public IEnumerable<Comment> Comments => _Comments;

        /// <summary>
        /// List of related comments
        /// </summary>
        private HashSet<Favorite> _Favorites;
        public IEnumerable<Favorite> Favorites => _Favorites;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor (Called by EF.Core)
        /// </summary>
        /// <param name="lazyLoader">The EF.Core lazy loader. Will be injected by DI-Container</param>
        private UserAccount(ILazyLoader lazyLoader)
        {
            _LazyLoader = lazyLoader;
        }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="id">Identifier for user account</param>
        /// <param name="userName">Username for the user</param>
        public UserAccount(string id, string userName)
        {
            //Überprüfen ob ein User-Name vorhanden ist
            if (string.IsNullOrEmpty(userName)) throw new DomainException("Username is required");

            //Übernehmen der Werte
            Id = id;
            UserName = userName;
        }
        #endregion

        #region Domain Methods
        /// <summary>
        /// Updates the user account from all assigned ratings
        /// </summary>
        public async Task UpdateRatingInfo()
        {
            //Laden der Werte wenn noch nicht geschehen
            await _LazyLoader.LoadAsync(this, navigationName: nameof(Ratings));

            //Setzen des Rating-Counts
            RatingCount = Ratings.Count();
        }

        /// <summary>
        /// Updates the user account info from all assigned comments
        /// </summary>
        public async Task UpdateCommentInfo()
        {
            //Laden der Werte wenn noch nicht geschehen
            await _LazyLoader.LoadAsync(this, navigationName: nameof(Comments));

            //Setzen des Rating-Counts
            CommentCount = Comments.Count();
        }

        /// <summary>
        /// Updates the user account info from all assigned favorites
        /// </summary>
        public async Task UpdateFavoriteInfo()
        {
            //Laden der Werte wenn noch nicht geschehen
            await _LazyLoader.LoadAsync(this, navigationName: nameof(Favorites));

            //Setzen des Rating-Counts
            FavoriteCount = Favorites.Count();
        }
        #endregion

        #region Called from Change-Tracker
        public override Task EntityAddedAsync()
        {
            return Task.CompletedTask;
        }

        public override async Task EntityDeletedAsync()
        {
            //Laden der Werte wenn noch nicht geschehen
            await _LazyLoader.LoadAsync(this, navigationName: nameof(Ratings));
            await _LazyLoader.LoadAsync(this, navigationName: nameof(Comments));
            await _LazyLoader.LoadAsync(this, navigationName: nameof(Favorites));

            //Aufrufen der Delete-Routine für alle zugehörigen Ratings
            foreach (var Item in Ratings)
            {
                await Item.EntityDeletedAsync();
            }

            //Aufrufen der Delete-Routine für alle zugehörigen Comments
            foreach (var Item in Comments)
            {
                await Item.EntityDeletedAsync();
            }

            //Aufrufen der Delete-Routine für alle zugehörigen Comments
            foreach (var Item in Favorites)
            {
                await Item.EntityDeletedAsync();
            }
        }

        public override Task EntityModifiedAsync()
        {
            //Ergebnis zurückliefern
            return Task.CompletedTask;
        }
        #endregion
    }
}
