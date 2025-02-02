using DomainHelper.AbstractClasses;
using DomainHelper.Exceptions;
using HotChocolate;
using HotChocolate.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserInteraction.Domain.Aggregates
{
    public class UserInteractionInfo : EntityModify<long>
    {
        #region Private Members
        private readonly ILazyLoader lazyLoader;
        #endregion

        #region Properties
        /// <summary>
        /// Count of ratings (For faster access as property)
        /// </summary>
        [Required]
        [GraphQLDescription("Cout of ratings")]
        public int RatingCount { get; private set; }

        /// <summary>
        /// Count of comments (For faster access as property)
        /// </summary>
        [Required]
        [GraphQLDescription("Count of comments")]
        public int CommentCount { get; private set; }

        /// <summary>
        /// Count of favorites (For faster access as property)
        /// </summary>
        [Required]
        [GraphQLDescription("Count of favorites")]
        public int FavoriteCount { get; private set; }

        /// <summary>
        /// List of related raitings
        /// </summary>
        [GraphQLDescription("List of related ratings")]
        [UseFiltering]
        [UseSorting]
        public ICollection<Rating> Ratings { get; private set; } = [];

        /// <summary>
        /// List of related comments
        /// </summary>
        [GraphQLDescription("List of related comments")]
        [UseFiltering]
        [UseSorting]
        public ICollection<Comment> Comments { get; private set; } = [];

        /// <summary>
        /// List of related favorites
        /// </summary>
        [GraphQLDescription("List of related favorites")]
        [UseFiltering]
        [UseSorting]
        public ICollection<Favorite> Favorites { get; private set; } = [];

        /// <summary>
        /// Identifier for the user account
        /// </summary>
        [GraphQLIgnore]
        [Required]
        public string UserAccountId { get; private set; }

        /// <summary>
        /// The user account this interaction info is linked to
        /// </summary>
        [GraphQLDescription("The user account this interaction info is linked to")]
        public UserAccount UserAccount { get; private set; } = default!;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor (Called by Graphql)
        /// </summary>
        private UserInteractionInfo()
        {

        }

        /// <summary>
        /// C'tor (Called by EF.Core)
        /// </summary>
        /// <param name="lazyLoader">The EF.Core lazy loader. Will be injected by DI-Container</param>
        private UserInteractionInfo(ILazyLoader lazyLoader)
        {
            this.lazyLoader = lazyLoader;
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

        #region Domain Methods
        /// <summary>
        /// Add a rating to user interaction info (media - element)
        /// </summary>
        /// <param name="ratingValue">The rating value</param>
        /// <returns>The added rating</returns>
        [GraphQLIgnore]
        public Rating AddRating(int ratingValue)
        {
            //Eine neue Entity hinzufügen
            var EntityAdd = new Rating(this, ratingValue);

            //Hinzufügen der Entity zur Collection
            Ratings.Add(EntityAdd);

            //Zurückliefern der hinzugefügten Entity
            return EntityAdd;
        }

        /// <summary>
        /// Add a comment to user interaction info (media - element)
        /// </summary>
        /// <param name="content">The comment content</param>
        /// <returns>The added rating</returns>
        [GraphQLIgnore]
        public Comment AddComment(string content)
        {
            //Eine neue Entity hinzufügen
            var EntityAdd = new Comment(this, content);

            //Hinzufügen der Entity zur Collection
            Comments.Add(EntityAdd);

            //Zurückliefern der hinzugefügten Entity
            return EntityAdd;
        }

        /// <summary>
        /// Add a favorite to user interaction info (media - element)
        /// </summary>
        /// <returns>The added favorite</returns>
        [GraphQLIgnore]
        public Favorite AddFavorite()
        {
            //Eine neue Entity hinzufügen
            var EntityAdd = new Favorite(this);

            //Hinzufügen der Entity zur Collection
            Favorites.Add(EntityAdd);

            //Zurückliefern der hinzugefügten Entity
            return EntityAdd;
        }

        /// <summary>
        /// Remove a favorite from this user interaction info (media -element)
        /// </summary>
        /// <param name="id">Identifier for favorite</param>
        [GraphQLIgnore]
        public async Task RemoveFavorite(long id)
        {
            //Laden der Werte wenn noch nicht geschehen
            await lazyLoader.LoadAsync(this, navigationName: nameof(Favorites));

            try
            {
                //Ermitteln des Items zum entfernen
                var ItemToRemove = Favorites.Single(x => x.Id == id);

                //Entfernen des Items
                Favorites.Remove(ItemToRemove);
            }
            catch (InvalidOperationException)
            {
                throw new DomainException(DomainExceptionType.NoDataFound);
            }
        }

        /// <summary>
        /// Updates the user interaction info from all assigned comments
        /// </summary>
        [GraphQLIgnore]
        public async Task UpdateCommentInfo()
        {
            //Laden der Werte wenn noch nicht geschehen
            await lazyLoader.LoadAsync(this, navigationName: nameof(Comments));

            //Setzen des Rating-Counts
            CommentCount = Comments.Count();
        }

        /// <summary>
        /// Updates the user interaction info from all assigned favorites
        /// </summary>
        [GraphQLIgnore]
        public async Task UpdateFavoriteInfo()
        {
            //Laden der Werte wenn noch nicht geschehen
            await lazyLoader.LoadAsync(this, navigationName: nameof(Favorites));

            //Setzen des Rating-Counts
            FavoriteCount = Favorites.Count();
        }
        #endregion

        #region Called from Change-Tracker
        public override Task EntityModifiedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
        {
            return Task.CompletedTask;
        }

        public override Task EntityAddedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
        {
            return Task.CompletedTask;
        }

        public override async Task EntityDeletedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
        {
            //Laden der Werte wenn noch nicht geschehen
            await lazyLoader.LoadAsync(this, navigationName: nameof(Ratings));
            await lazyLoader.LoadAsync(this, navigationName: nameof(Comments));
            await lazyLoader.LoadAsync(this, navigationName: nameof(Favorites));

            //Aufrufen der Delete-Methode für alle zugeordneten Ratings
            foreach (var Item in Ratings)
            {
                await Item.EntityDeletedAsync(dbContext, dictContextParams);
            }

            //Aufrufen der Delete-Methode für alle zugeordneten Comments
            foreach (var Item in Comments)
            {
                await Item.EntityDeletedAsync(dbContext, dictContextParams);
            }

            //Aufrufen der Delete-Methode für alle zugeordneten Comments
            foreach (var Item in Favorites)
            {
                await Item.EntityDeletedAsync(dbContext, dictContextParams);
            }
        }
        #endregion
    }
}
