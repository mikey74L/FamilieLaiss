using DomainHelper.AbstractClasses;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainHelper.Exceptions;

namespace UserInteraction.Domain.Aggregates
{
    public class UserInteractionInfo : EntityModify<long>
    {
        #region Private Members
        private readonly ILazyLoader _LazyLoader;
        #endregion

        #region Properties
        /// <summary>
        /// The count of ratings (For faster Access as property)
        /// </summary>
        public int RatingCount { get; private set; }

        /// <summary>
        /// The average rating over all raitings
        /// </summary>
        public double AverageRating { get; private set; }

        /// <summary>
        /// The count of comments (For faster access as property)
        /// </summary>
        public int CommentCount { get; private set; }

        /// <summary>
        /// The count of favorites (For faster access as property)
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
        /// List of related favorites
        /// </summary>
        private HashSet<Favorite> _Favorites;
        public IEnumerable<Favorite> Favorites => _Favorites;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor (Called by EF.Core)
        /// </summary>
        /// <param name="lazyLoader">The EF.Core lazy loader. Will be injected by DI-Container</param>
        private UserInteractionInfo(ILazyLoader lazyLoader)
        {
            _LazyLoader = lazyLoader;
        }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="id">The identifier for this rating info</param>
        public UserInteractionInfo(long id)
        {
            //Übernehmen der Werte
            Id = id;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Calculates the avergae rating.
        /// </summary>
        private void MakeAverageRaiting()
        {
            //Deklarationen
            double CalculatedValue = 0;

            //Summe der Ratings bilden
            CalculatedValue = Ratings.Sum(x => x.Value);

            //Durch die Anzahl der Ratings teilen
            AverageRating = CalculatedValue / RatingCount;
        }
        #endregion

        #region Domain Methods
        /// <summary>
        /// Add a rating to user interaction info (media - element)
        /// </summary>
        /// <param name="accountID">Identifier for user account this rating belongs to</param>
        /// <param name="ratingValue">The rating value</param>
        /// <returns>The added rating</returns>
        public Rating AddRating(string accountID, int ratingValue)
        {
            //Eine neue Entity hinzufügen
            var EntityAdd = new Rating(this, accountID, ratingValue);

            //Wenn die Liste noch noch null sein sollte erstellen einer leeren Liste
            if (_Ratings == null)
            {
                _Ratings = new HashSet<Rating>();
            }

            //Hinzufügen der Entity zur Collection
            _Ratings.Add(EntityAdd);

            //Zurückliefern der hinzugefügten Entity
            return EntityAdd;
        }

        /// <summary>
        /// Add a comment to user interaction info (media - element)
        /// </summary>
        /// <param name="accountID">Identifier for user account this comment belongs to</param>
        /// <param name="content">The comment content</param>
        /// <returns>The added rating</returns>
        public Comment AddComment(string accountID, string content)
        {
            //Eine neue Entity hinzufügen
            var EntityAdd = new Comment(this, accountID, content);

            //Wenn die Liste noch noch null sein sollte erstellen einer leeren Liste
            if (_Comments == null)
            {
                _Comments = new HashSet<Comment>();
            }

            //Hinzufügen der Entity zur Collection
            _Comments.Add(EntityAdd);

            //Zurückliefern der hinzugefügten Entity
            return EntityAdd;
        }

        /// <summary>
        /// Add a favorite to user interaction info (media - element)
        /// </summary>
        /// <param name="accountID">Identifier for user account this favorite belongs to</param>
        /// <returns>The added favorite</returns>
        public Favorite AddFavorite(string accountID)
        {
            //Eine neue Entity hinzufügen
            var EntityAdd = new Favorite(this, accountID);

            //Wenn die Liste noch noch null sein sollte erstellen einer leeren Liste
            if (_Favorites == null)
            {
                _Favorites = new HashSet<Favorite>();
            }

            //Hinzufügen der Entity zur Collection
            _Favorites.Add(EntityAdd);

            //Zurückliefern der hinzugefügten Entity
            return EntityAdd;
        }

        /// <summary>
        /// Remove a favorite from this user interaction info (media -element)
        /// </summary>
        /// <param name="id">Identifier for favorite</param>
        public async Task RemoveFavorite(long id)
        {
            //Laden der Werte wenn noch nicht geschehen
            await _LazyLoader.LoadAsync(this, navigationName: nameof(Favorites));

            try
            {
                //Ermitteln des Items zum entfernen
                var ItemToRemove = Favorites.Single(x => x.Id == id);

                //Entfernen des Items
                _Favorites.Remove(ItemToRemove);
            }
            catch (InvalidOperationException)
            {
                throw new NoDataFoundException();
            }
        }

        /// <summary>
        /// Updates the user interaction info from all assigned ratings
        /// </summary>
        public async Task UpdateRatingInfo()
        {
            //Laden der Werte wenn noch nicht geschehen
            await _LazyLoader.LoadAsync(this, navigationName: nameof(Ratings));

            //Setzen des Rating-Counts
            RatingCount = Ratings.Count();

            //Kalkulieren der durschnittlichen Bewertung
            MakeAverageRaiting();
        }
     
        /// <summary>
        /// Updates the user interaction info from all assigned comments
        /// </summary>
        public async Task UpdateCommentInfo()
        {
            //Laden der Werte wenn noch nicht geschehen
            await _LazyLoader.LoadAsync(this, navigationName: nameof(Comments));

            //Setzen des Rating-Counts
            CommentCount = Comments.Count();
        }

        /// <summary>
        /// Updates the user interaction info from all assigned favorites
        /// </summary>
        public async Task UpdateFavoriteInfo()
        {
            //Laden der Werte wenn noch nicht geschehen
            await _LazyLoader.LoadAsync(this, navigationName: nameof(Favorites));

            //Setzen des Rating-Counts
            FavoriteCount = _Favorites.Count();
        }
        #endregion

        #region Called from Change-Tracker
        public override Task EntityModifiedAsync()
        {
            return Task.CompletedTask;
        }

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

            //Aufrufen der Delete-Methode für alle zugeordneten Ratings
            foreach (var Item in Ratings)
            {
                await Item.EntityDeletedAsync();
            }

            //Aufrufen der Delete-Methode für alle zugeordneten Comments
            foreach (var Item in Comments)
            {
                await Item.EntityDeletedAsync();
            }

            //Aufrufen der Delete-Methode für alle zugeordneten Comments
            foreach (var Item in Favorites)
            {
                await Item.EntityDeletedAsync();
            }
        }
        #endregion
    }
}
