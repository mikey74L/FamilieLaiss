using DomainHelper.AbstractClasses;
using DomainHelper.Exceptions;
using HotChocolate;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using UserInteraction.Domain.DomainEvents;

namespace UserInteraction.Domain.Aggregates
{
    public class Rating : EntityCreation<long>
    {
        #region Properties
        /// <summary>
        /// Identifier for user interaction info
        /// </summary>
        [GraphQLIgnore]
        [Required]
        public long UserInteractionInfoId { get; private set; }

        /// <summary>
        /// The user interaction info this rating belongs to
        /// </summary>
        [GraphQLDescription("The user interaction info this rating belongs to")]
        public UserInteractionInfo UserInteractionInfo { get; private set; }

        /// <summary>
        /// Identifier for media item
        /// </summary>
        [GraphQLIgnore]
        [Required]
        public long MediaItemId { get; private set; }

        /// <summary>
        /// The media item this rating belongs to
        /// </summary>
        [GraphQLDescription("The media item this rating belongs to")]
        public MediaItem MediaItem { get; private set; }

        /// <summary>
        /// The rating value
        /// </summary>
        [Required]
        [GraphQLDescription("The user interaction info this rating belongs to")]
        public int Value { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor (Called by EF.Core)
        /// </summary>
        private Rating()
        {

        }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="userInteractionInfo">The rating info this rating belongs to</param>
        /// <param name="ratingValue">The rating value</param>
        public Rating(UserInteractionInfo userInteractionInfo, int ratingValue)
        {
            //Überprüfen ob eine Rating-Info übergeben wurde
            if (userInteractionInfo == null) throw new DomainException("A user interaction info is needed");

            //Übernehmen der Werte
            UserInteractionInfo = userInteractionInfo;
            Value = ratingValue;
        }
        #endregion

        #region Called from Change-Tracker
        public override Task EntityAddedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
        {
            //Feuern des Domain-Events 
            AddDomainEvent(new MtrEventRatingAdded(Id, UserInteractionInfo.Id));

            //Funktionsergebnis
            return Task.CompletedTask;
        }

        public override Task EntityDeletedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
        {
            //Hinzufügen der Domain-Events
            AddDomainEvent(new MtrEventRatingDeleted(Id, UserInteractionInfoId));

            //Funktionsergebnis
            return Task.CompletedTask;
        }
        #endregion

        #region Domain Methods
        #endregion
    }
}
