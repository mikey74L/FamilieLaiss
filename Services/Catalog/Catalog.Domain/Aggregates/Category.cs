using DomainHelper.AbstractClasses;
using DomainHelper.Exceptions;
using FamilieLaissSharedObjects.Enums;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Domain.Aggregates
{
    /// <summary>
    /// Entity for category
    /// </summary>
    public class Category : EntityModify<long>
    {
        #region Private Properties
        private ILazyLoader _LazyLoader;
        #endregion

        #region Properties
        /// <summary>
        /// The type of category
        /// </summary>
        public EnumCategoryType CategoryType { get; private set; }

        /// <summary>
        /// German name for this category
        /// </summary>
        public string NameGerman { get; private set; } = string.Empty;

        /// <summary>
        /// English name for this category
        /// </summary>
        public string NameEnglish { get; private set; } = string.Empty;

        /// <summary>
        /// List of related category values
        /// </summary>
        public ICollection<CategoryValue> CategoryValues { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor would be used by EF-Core
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private Category(ILazyLoader lazyLoader)
        {
            _LazyLoader = lazyLoader;
        }

        /// <summary>
        /// C'tor only for GraphQL-Select
        /// </summary>
        protected Category()
        {

        }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="categoryType">The type of category</param>
        /// <param name="nameGerman">German name for this category</param>
        /// <param name="nameEnglish">English name for this category</param>
        public Category(EnumCategoryType? categoryType, string? nameGerman, string? nameEnglish)
        {
            //Überprüfen ob ein German-Name festgelegt wurde
            if (categoryType is null) throw new DomainException(DomainExceptionType.WrongParameter, "A german name is needed for this category");

            //Überprüfen ob ein German-Name festgelegt wurde
            if (string.IsNullOrEmpty(nameGerman)) throw new DomainException(DomainExceptionType.WrongParameter, "A german name is needed for this category");

            //Überprüfen ob ein English-Name festgelegt wurde
            if (string.IsNullOrEmpty(nameEnglish)) throw new DomainException(DomainExceptionType.WrongParameter, "A english name is needed for this category");

            //Übernehmen der Werte
            CategoryType = categoryType.Value;
            NameGerman = nameGerman;
            NameEnglish = nameEnglish;

            //Erstellen der leeren Liste für die Values
            CategoryValues = new HashSet<CategoryValue>();
        }
        #endregion

        #region Domain Methods
        /// <summary>
        /// Update the category
        /// </summary>
        /// <param name="nameGerman">German name for this category</param>
        /// <param name="nameEnglish">English name for this category</param>
        public void Update(string? nameGerman, string? nameEnglish)
        {
            //Überprüfen ob ein German-Name festgelegt wurde
            if (string.IsNullOrEmpty(nameGerman)) throw new DomainException(DomainExceptionType.WrongParameter, "A german name is needed for this category");

            //Überprüfen ob ein English-Name festgelegt wurde
            if (string.IsNullOrEmpty(nameEnglish)) throw new DomainException(DomainExceptionType.WrongParameter, "A english name is needed for this category");

            //Übernehmen der Properties
            NameGerman = nameGerman;
            NameEnglish = nameEnglish;
        }

        /// <summary>
        /// Add a value to a category
        /// </summary>
        /// <param name="germanName">The german name for the category value</param>
        /// <param name="englishName">The english name for this category value</param>
        /// <returns>The added category value</returns>
        public CategoryValue AddCategoryValue(string? germanName, string? englishName)
        {
            //Eine neue User-Entity hinzufügen
            var valueEntity = new CategoryValue(this, germanName, englishName);

            //Wenn die Liste noch noch null sein sollte erstellen einer leeren Liste
            if (CategoryValues == null)
            {
                CategoryValues = new HashSet<CategoryValue>();
            }

            //Hinzufügen der Value-Entity zur Collection
            CategoryValues.Add(valueEntity);

            //Zurückliefern der hinzugefügten Entity
            return valueEntity;
        }

        /// <summary>
        /// Remove a value from this category
        /// </summary>
        /// <param name="categoryValueId">The Identifier for category value</param>
        public async Task RemoveCategoryValue(long categoryValueId)
        {
            //Deklaration
            bool itemFound = false;

            //Laden der Kategorie-Werte wenn noch nicht geschehen
            await _LazyLoader.LoadAsync(this, navigationName: nameof(CategoryValues));

            //Entfernen des Items
            foreach (var item in CategoryValues)
            {
                if (item.Id == categoryValueId)
                {
                    itemFound = true;
                    CategoryValues.Remove(item);
                    break;
                }
            }

            //Item wurde nicht gefunden
            if (!itemFound) throw new DomainException(DomainExceptionType.NoDataFound);
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
            //Laden der Kategorie-Werte wenn noch nicht geschehen
            await _LazyLoader.LoadAsync(this, navigationName: nameof(CategoryValues));

            //Für alle zugeordneten Category-Values die Delete-Routine aufrufen
            foreach (var item in CategoryValues)
            {
                await item.EntityDeletedAsync();
            }
        }
        #endregion
    }
}
