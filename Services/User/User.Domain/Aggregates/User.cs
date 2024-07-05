using DomainHelper.AbstractClasses;
using HotChocolate;
using Microsoft.EntityFrameworkCore.Infrastructure;
using User.Domain.DomainEvents;

namespace User.Domain.Aggregates
{
    [GraphQLDescription("User")]
    public class User : EntityModify<string>
    {
        #region Private Members
        private ILazyLoader _LazyLoader;
        #endregion

        #region Properties
        /// <summary>
        /// eMail-Adress for user
        /// </summary>
        [GraphQLDescription("The eMail-Adress for the user")]
        [GraphQLNonNullType]
        public string EMail { get; private set; } = string.Empty;

        /// <summary>
        /// The user name (Display Name)
        /// </summary>
        [GraphQLDescription("The user name for the user")]
        [GraphQLNonNullType]
        public string UserName { get; private set; } = string.Empty;

        /// <summary>
        /// The gender ID
        /// </summary>
        [GraphQLDescription("The gender ID")]
        public string? GenderID { get; private set; }

        /// <summary>
        /// The given name
        /// </summary>
        [GraphQLDescription("The given name for the user")]
        public string? GivenName { get; private set; }

        /// <summary>
        /// The family name
        /// </summary>
        [GraphQLDescription("The family name for the user")]
        public string? FamilyName { get; private set; }

        /// <summary>
        /// The name of the street
        /// </summary>
        [GraphQLDescription("The street name for the adress of the user")]
        public string? Street { get; private set; }

        /// <summary>
        /// The house number
        /// </summary>
        [GraphQLDescription("The house number for the adress of the user")]
        public string? HNR { get; private set; }

        /// <summary>
        /// The postal code
        /// </summary>
        [GraphQLDescription("The ZIP-Code for the adress of the user")]
        public string? ZIP { get; private set; }

        /// <summary>
        /// The name of the city
        /// </summary>
        [GraphQLDescription("The name of the city the user lives in")]
        public string? City { get; private set; }

        /// <summary>
        /// The ID of the assigned country
        /// </summary>
        [GraphQLIgnore]
        public string? CountryID { get; private set; }

        /// <summary>
        /// The country where the user lives
        /// </summary>
        [GraphQLDescription("The assigned country the user lives in")]
        [UseFiltering]
        public Country? Country { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor without parameters would be used by EF-Core
        /// </summary>
        private User(ILazyLoader lazyLoader)
        {
            _LazyLoader = lazyLoader;
        }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="userID">The ID for the user from Auth0</param>
        /// <param name="eMail">The eMail-Adress for the user from Auth0</param>
        /// <param name="userName">The user name for the user from Auth0</param>
        public User(string userID, string eMail, string userName)
        {
            //Übernehmen der Werte
            Id = userID;
            EMail = eMail;
            UserName = userName;
        }
        #endregion

        #region Domain Methods
        /// <summary>
        /// Update the user
        /// </summary>
        /// <param name="genderID">The Gender ID</param>
        /// <param name="givenName">The given name</param>
        /// <param name="familyName">The family name</param>
        /// <param name="street">The name of the street</param>
        /// <param name="hnr">The house number</param>
        /// <param name="zip">The postal code</param>
        /// <param name="city">The name of the city</param>
        /// <param name="countryID">The Country-ID for the country</param>
        public void Update(string genderID, string givenName, string familyName, string street, string hnr, string zip, string city,
            string countryID)
        {
            //Übernehmen der Properties
            GenderID = genderID;
            GivenName = givenName;
            FamilyName = familyName;
            Street = street;
            HNR = hnr;
            ZIP = zip;
            City = city;
            CountryID = countryID;
        }
        #endregion

        #region Called from Change-Tracker
        public override Task EntityAddedAsync()
        {
            //Domain-Event hinzufügen
            AddDomainEvent(new MtrEventUserCreated(Id, EMail, UserName));

            //Ergebnis zurückliefern
            return Task.CompletedTask;
        }

        public override Task EntityModifiedAsync()
        {
            return Task.CompletedTask;
        }

        public override Task EntityDeletedAsync()
        {
            return Task.CompletedTask;
        }
        #endregion
    }
}
