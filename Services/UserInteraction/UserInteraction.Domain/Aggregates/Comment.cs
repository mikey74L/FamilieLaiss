

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
    public class Comment : EntityCreation<long>
    {
        #region Properties
        /// <summary>
        /// Identifier for user interaction info
        /// </summary>
        [Required]
        [GraphQLIgnore]
        public long UserInteractionInfoId { get; private set; }

        /// <summary>
        /// The user interaction info this comment belongs to
        /// </summary>
        [GraphQLDescription("The user interaction info this comment belongs to")]
        public UserInteractionInfo UserInteractionInfo { get; private set; }

        /// <summary>
        /// Identifier for media item
        /// </summary>
        [Required]
        [GraphQLIgnore]
        public long MediaItemId { get; private set; }

        /// <summary>
        /// The media item this comment belongs to
        /// </summary>
        [GraphQLDescription("The media item this comment belongs to")]
        public MediaItem MediaItem { get; private set; }

        /// <summary>
        /// Content for the comment
        /// </summary>
        [GraphQLDescription("Content for the comment")]
        [MaxLength(2000)]
        public string Content { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor (called by EF.Core)
        /// </summary>
        private Comment()
        {

        }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="userInteractionInfo">The user interaction info this comment belongs to</param>
        /// <param name="content">The content text for this comment</param>
        public Comment(UserInteractionInfo userInteractionInfo, string content)
        {
            //Überprüfen ob eine Rating-Info übergeben wurde
            if (userInteractionInfo == null) throw new DomainException("A user interaction info is needed");

            //Übernehmen der Werte
            UserInteractionInfo = userInteractionInfo;
            Content = content;
        }
        #endregion

        #region Domain Mehtods
        #endregion

        #region Called from Change Tracker
        public override Task EntityAddedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
        {
            //Feuern des Domain-Events 
            AddDomainEvent(new MtrEventCommentAdded(Id, UserInteractionInfo.Id));

            //Funktionsergebnis
            return Task.CompletedTask;
        }

        public override Task EntityDeletedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
        {
            //Hinzufügen der Domain-Events
            AddDomainEvent(new MtrEventCommentDeleted(Id, UserInteractionInfoId));

            //Funktionsergebnis
            return Task.CompletedTask;
        }
        #endregion
    }
}
