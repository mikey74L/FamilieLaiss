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
    public class Favorite : EntityCreation<long>
    {
        #region Properties
        /// <summary>
        /// Identifier for user interaction info
        /// </summary>
        [Required]
        [GraphQLIgnore]
        public long UserInteractionInfoId { get; private set; }

        /// <summary>
        /// The user interaction info this favorite belongs to
        /// </summary>
        [GraphQLDescription("The user interaction info this favorite belongs to")]
        public UserInteractionInfo UserInteractionInfo { get; private set; }

        /// <summary>
        /// Identifier for media item
        /// </summary>
        [Required]
        [GraphQLIgnore]
        public long MediaItemId { get; private set; }

        /// <summary>
        /// The media item this favorite belongs to
        /// </summary>
        [GraphQLDescription("The media item this favorite belongs to")]
        public MediaItem MediaItem { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor (called by EF.Core)
        /// </summary>
        private Favorite()
        {

        }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="userInteractionInfo">The user interaction info this comment belongs to</param>
        public Favorite(UserInteractionInfo userInteractionInfo)
        {
            //Überprüfen ob eine Rating-Info übergeben wurde
            if (userInteractionInfo == null) throw new DomainException("A user interaction info is needed");

            //Übernehmen der Werte
            UserInteractionInfo = userInteractionInfo;
        }
        #endregion

        #region Domain Mehtods
        #endregion

        #region Called from Change Tracker
        public override Task EntityAddedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
        {
            //Feuern des Domain-Events 
            AddDomainEvent(new MtrEventFavoriteAdded(Id, UserInteractionInfo.Id));

            //Funktionsergebnis
            return Task.CompletedTask;
        }

        public override Task EntityDeletedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
        {
            //Hinzufügen der Domain-Events
            AddDomainEvent(new MtrEventFavoriteDeleted(Id, UserInteractionInfoId));

            //Funktionsergebnis
            return Task.CompletedTask;
        }
        #endregion
    }
}
