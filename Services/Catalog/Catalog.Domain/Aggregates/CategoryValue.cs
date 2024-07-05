using DomainHelper.AbstractClasses;
using DomainHelper.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Domain.Aggregates
{
    /// <summary>
    /// Entity for category value
    /// </summary>
    public class CategoryValue : EntityModify<long>
    {
        #region Properties
        /// <summary>
        /// Identifier for the category
        /// </summary>
        public long CategoryID { get; private set; }

        /// <summary>
        /// The Category this category value entry belongs to
        /// </summary>
        public Category Category { get; private set; } = default!;

        /// <summary>
        /// German name for this category value
        /// </summary>
        public string NameGerman { get; private set; } = string.Empty;

        /// <summary>
        /// English name for this category value
        /// </summary>
        public string NameEnglish { get; private set; } = string.Empty;

        /// <summary>
        /// List of related media item category values
        /// </summary>
        public ICollection<MediaItemCategoryValue> MediaItemCategoryValues { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor without parameters would be used by EF-Core
        /// </summary>
        protected CategoryValue()
        {

        }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="category">The category for this value</param>
        /// <param name="nameGerman">The german name for this value</param>
        /// <param name="nameEnglish">The english name for this value</param>
        internal CategoryValue(Category category, string? nameGerman, string? nameEnglish)
        {
            //Überprüfen ob ein German-Name festgelegt wurde
            if (string.IsNullOrEmpty(nameGerman)) throw new DomainException("A german name is needed for this category value");

            //Überprüfen ob ein English-Name festgelgt wurde
            if (string.IsNullOrEmpty(nameEnglish)) throw new DomainException("A english name is needed for this category value");

            //Übernehmen der Werte
            Category = category;
            NameGerman = nameGerman;
            NameEnglish = nameEnglish;
        }
        #endregion

        #region Domain-Methods
        /// <summary>
        /// Updates the category value data
        /// </summary>
        /// <param name="nameGerman">The german name for category value</param>
        /// <param name="nameEnglish">The english name for category value</param>
        public void Update(string? nameGerman, string? nameEnglish)
        {
            //Überprüfen ob ein German-Name festgelegt wurde
            if (string.IsNullOrEmpty(nameGerman)) throw new DomainException("A german name is needed for this category value");

            //Überprüfen ob ein English-Name festgelgt wurde
            if (string.IsNullOrEmpty(nameEnglish)) throw new DomainException("A english name is needed for this category value");

            //Übernehmen der Werte
            NameGerman = nameGerman;
            NameEnglish = nameEnglish;
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

        public override Task EntityDeletedAsync()
        {
            return Task.CompletedTask;
        }
        #endregion
    }
}
