using DomainHelper.AbstractClasses;
using DomainHelper.Exceptions;
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

        /// <summary>
        /// The rating value
        /// </summary>
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
        /// <param name="accountID">Identifier for user account this rating belongs to</param>
        /// <param name="ratingValue">The rating value</param>
        public Rating(UserInteractionInfo userInteractionInfo, string accountID, int ratingValue)
        {
            //Überprüfen ob eine Rating-Info übergeben wurde
            if (userInteractionInfo == null) throw new DomainException("A user interaction info is needed");

            //Überprüfen ob ein UserAccount übergeben wurde
            if (string.IsNullOrEmpty(accountID)) throw new DomainException("A user account is needed");

            //Übernehmen der Werte
            UserInteractionInfo = userInteractionInfo;
            UserAccountID = accountID;
            Value = ratingValue;
        }
        #endregion

        #region Called from Change-Tracker
        public override Task EntityAddedAsync()
        {
            //Feuern des Domain-Events 
            AddDomainEvent(new MtrEventRatingAdded(Id, UserInteractionInfo.Id, UserAccountID));

            //Funktionsergebnis
            return Task.CompletedTask;
        }

        public override Task EntityDeletedAsync()
        {
            //Hinzufügen der Domain-Events
            AddDomainEvent(new MtrEventRatingDeleted(Id, UserInteractionInfoID, UserAccountID));

            //Funktionsergebnis
            return Task.CompletedTask;
        }
        #endregion

        #region Domain Methods
        #endregion
    }
}
