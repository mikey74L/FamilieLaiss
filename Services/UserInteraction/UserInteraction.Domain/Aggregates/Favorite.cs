using DomainHelper.AbstractClasses;
using DomainHelper.Exceptions;
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
        public long UserInteractionInfoID { get; private set; }

        /// <summary>
        /// The user interaction info this rating belongs to
        /// </summary>
        public UserInteractionInfo UserInteractionInfo { get; private set; }

        /// <summary>
        /// Identifier for user account
        /// </summary>
        public string UserAccountID { get; private set; }

        /// <summary>
        /// The user account this rating belongs to
        /// </summary>
        public UserAccount UserAccount { get; private set; }
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
        /// <param name="userAccountID">Identifier for user account</param>
        public Favorite(UserInteractionInfo userInteractionInfo, string userAccountID)
        {
            //Überprüfen ob eine Rating-Info übergeben wurde
            if (userInteractionInfo == null) throw new DomainException("A user interaction info is needed");

            //Überprüfen ob ein UserAccount übergeben wurde
            if (string.IsNullOrEmpty(userAccountID)) throw new DomainException("A user account is needed");

            //Übernehmen der Werte
            UserInteractionInfo = userInteractionInfo;
            UserAccountID = userAccountID;
        }
        #endregion

        #region Domain Mehtods
        #endregion

        #region Called from Change Tracker
        public override Task EntityAddedAsync()
        {
            //Feuern des Domain-Events 
            AddDomainEvent(new MtrEventFavoriteAdded(Id, UserInteractionInfo.Id, UserAccountID));

            //Funktionsergebnis
            return Task.CompletedTask;
        }

        public override Task EntityDeletedAsync()
        {
            //Hinzufügen der Domain-Events
            AddDomainEvent(new MtrEventFavoriteDeleted(Id, UserInteractionInfoID, UserAccountID));

            //Funktionsergebnis
            return Task.CompletedTask;
        }
        #endregion
    }
}
