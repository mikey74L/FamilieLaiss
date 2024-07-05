using DomainHelper.AbstractClasses;
using DomainHelper.Exceptions;
using HotChocolate;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace User.Domain.Aggregates
{
    [GraphQLDescription("Country")]
    public class Country : EntityCreation<string>
    {
        #region Private Properties
        private ILazyLoader _LazyLoader;
        #endregion

        #region Properties
        /// <summary>
        /// The german name
        /// </summary>
        [GraphQLDescription("The german country name")]
        public string NameGerman { get; private set; } = string.Empty;

        /// <summary>
        /// The englisch name
        /// </summary>
        [GraphQLDescription("The english country name")]
        public string NameEnglish { get; private set; } = string.Empty;

        /// <summary>
        /// List of related media item category values
        /// </summary>
        [GraphQLDescription("The list of users that live in this country")]
        [UseFiltering]
        [UseSorting]
        public ICollection<User> Users { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor would be used by EF-Core
        /// </summary>
        private Country(ILazyLoader lazyLoader)
        {
            _LazyLoader = lazyLoader;
        }

        public Country(string id, string nameGerman, string nameEnglish)
        {
            //Überprüfen ob die ID festgelegt wurde
            if (string.IsNullOrEmpty(id)) throw new DomainException("An ID is needed for this country");

            //Überprüfen ob ein German-Name festgelegt wurde
            if (string.IsNullOrEmpty(nameGerman)) throw new DomainException("A german name is needed for this country");

            //Überprüfen ob ein English-Name festgelegt wurde
            if (string.IsNullOrEmpty(nameEnglish)) throw new DomainException("A english name is needed for this country");

            //Übernehmen der Werte
            Id = id;
            NameGerman = nameGerman;
            NameEnglish = nameEnglish;
        }
        #endregion

        #region Domain Methods
        #endregion

        #region Called from Change-Tracker
        public override Task EntityAddedAsync()
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
