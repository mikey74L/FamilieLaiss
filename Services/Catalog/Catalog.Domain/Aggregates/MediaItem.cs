using Catalog.Domain.DomainEvents;
using Catalog.Domain.Entities;
using DomainHelper.AbstractClasses;
using DomainHelper.Exceptions;
using FamilieLaissSharedObjects.Enums;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Domain.Aggregates
{
    /// <summary>
    /// Entity for media item
    /// </summary>
    public class MediaItem : EntityModify<long>
    {
        #region Private Members
        private ILazyLoader _LazyLoader;
        #endregion

        #region Properties
        /// <summary>
        /// Identifier for the media group
        /// </summary>
        public long MediaGroupID { get; private set; }

        /// <summary>
        /// The media group this media item entry belongs to
        /// </summary>
        public MediaGroup MediaGroup { get; private set; }

        /// <summary>
        /// The type of Media-Item
        /// </summary>
        public EnumMediaType MediaType { get; private set; }

        /// <summary>
        /// German name for this Media-Item
        /// </summary>
        public string NameGerman { get; private set; }

        /// <summary>
        /// English name for this Media-Item
        /// </summary>
        public string NameEnglish { get; private set; }

        /// <summary>
        /// German description for this Media-Item
        /// </summary>
        public string? DescriptionGerman { get; private set; }

        /// <summary>
        /// English description for this Media-Item
        /// </summary>
        public string? DescriptionEnglish { get; private set; }

        /// <summary>
        /// Is this Media-Item only visible for family users
        /// </summary>
        public bool OnlyFamily { get; private set; }

        /// <summary>
        /// ID for the assigned upload picture item 
        /// </summary>
        public long? UploadPictureID { get; private set; }

        /// <summary>
        /// The upload picture for this media item
        /// </summary>
        public UploadPicture? UploadPicture { get; private set; }

        /// <summary>
        /// ID for the assigned upload video item 
        /// </summary>
        public long? UploadVideoID { get; private set; }

        /// <summary>
        /// The upload video for this media item
        /// </summary>
        public UploadVideo? UploadVideo { get; private set; }

        /// <summary>
        /// List of related media item category values
        /// </summary>
        public ICollection<MediaItemCategoryValue> MediaItemCategoryValues { get; private set; }
        #endregion

        #region C'tor
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private MediaItem(ILazyLoader lazyLoader)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            _LazyLoader = lazyLoader;
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected MediaItem()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {

        }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="mediaGroup">Media group this media item belongs to</param>
        /// <param name="mediaType">The type of Media-Item</param>
        /// <param name="nameGerman">German name for this Media-Item</param>
        /// <param name="nameEnglish">English name for this Media-Item</param>
        /// <param name="descriptionGerman">German description for this Media-Item</param>
        /// <param name="descriptionEnglish">English description for this Media-Item</param>
        /// <param name="onlyFamily">Is this Media-Item only visible for family users</param>
        /// <param name="uploadItemID">Id for the assigned upload item</param>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        internal MediaItem(MediaGroup mediaGroup, EnumMediaType mediaType, string nameGerman, string nameEnglish, string descriptionGerman,
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            string descriptionEnglish, bool onlyFamily, long uploadItemID)
        {
            //Überprüfen ob ein deutscher Name vorhanden ist
            if (string.IsNullOrEmpty(nameGerman)) throw new DomainException("A german name is needed");

            //Überprüfen ob ein englischer Name vorhanden ist
            if (string.IsNullOrEmpty(nameEnglish)) throw new DomainException("A english name is needed");

            //Übernehmen der Werte
            MediaGroup = mediaGroup;
            MediaType = mediaType;
            NameGerman = nameGerman;
            NameEnglish = nameEnglish;
            DescriptionGerman = descriptionGerman;
            DescriptionEnglish = descriptionEnglish;
            OnlyFamily = onlyFamily;
            if (MediaType == EnumMediaType.Picture)
            {
                UploadPictureID = uploadItemID;
            }
            else
            {
                UploadVideoID = uploadItemID;
            }

            //Wenn es sich um ein Bild handelt, dann wird der Name automatisch auf den Namen
            //des Upload-Items gesetzt
            if (MediaType == EnumMediaType.Picture)
            {
                NameGerman = $"PICTURE_{UploadPictureID}";
                NameEnglish = $"PICTURE_{UploadPictureID}";
            }
        }
        #endregion

        #region Domain Methods
        public void Update(string nameGerman, string nameEnglish, string descriptionGerman, string descriptionEnglish, bool onlyFamily)
        {
            //Überprüfen ob ein deutscher Name vorhanden ist
            if (string.IsNullOrEmpty(nameGerman) && MediaType == EnumMediaType.Video) throw new DomainException("A german name is needed");

            //Überprüfen ob ein englischer Name vorhanden ist
            if (string.IsNullOrEmpty(nameEnglish) && MediaType == EnumMediaType.Video) throw new DomainException("A english name is needed");

            //Übernehmen der Werte
            if (MediaType != EnumMediaType.Picture) //Nachfolgende nur übernehmen wenn es sich nicht um Photos handelt da diese keinen Namen und Beschreibung verwenden
            {
                NameGerman = nameGerman;
                NameEnglish = nameEnglish;
                DescriptionGerman = descriptionGerman;
                DescriptionEnglish = descriptionEnglish;
            }
            OnlyFamily = onlyFamily;
        }

        /// <summary>
        /// Add a category value to the list of assigned category values
        /// </summary>
        /// <param name="categoryValueID">Identifier for category value</param>
        /// <returns>The added media item category value</returns>
        public MediaItemCategoryValue AddCategoryValue(long categoryValueID)
        {
            //Eine neue User-Entity hinzufügen
            var ValueEntity = new MediaItemCategoryValue(this, categoryValueID);

            //Wenn die Liste noch null sein sollte erstellen einer leeren Liste
            if (MediaItemCategoryValues == null)
            {
                MediaItemCategoryValues = new HashSet<MediaItemCategoryValue>();
            }

            //Hinzufügen der Value-Entity zur Collection
            MediaItemCategoryValues.Add(ValueEntity);

            //Zurückliefern der hinzugefügten Entity
            return ValueEntity;
        }

        /// <summary>
        /// Updates the assigned category values
        /// </summary>
        /// <param name="values">List of assigned category value IDs</param>
        public async Task UpdateCategoryValues(IEnumerable<long> values)
        {
            //Laden der Kategorie-Werte wenn noch nicht geschehen
            await _LazyLoader.LoadAsync(this, navigationName: nameof(MediaItemCategoryValues));

            //Ermitteln der zu löschenden Items
            List<long> ItemsToDelete = new();
            bool Found = false;
            foreach (var AssignedValue in MediaItemCategoryValues)
            {
                foreach (var NewValue in values)
                {
                    if (NewValue == AssignedValue.CategoryValueID)
                    {
                        Found = true;
                    }
                }
                if (Found == false)
                {
                    ItemsToDelete.Add(AssignedValue.CategoryValueID);
                }
                else
                {
                    Found = false;
                }
            }

            //Ermitteln der Items die hinzugefügt werden müssen
            List<long> ItemsToAdd = new();
            foreach (var NewValue in values)
            {
                foreach (var AssignedValue in MediaItemCategoryValues)
                {
                    if (NewValue == AssignedValue.CategoryValueID)
                    {
                        Found = true;
                    }
                }
                if (Found == false)
                {
                    ItemsToAdd.Add(NewValue);
                }
                else
                {
                    Found = false;
                }
            }

            //Löschen der nicht mehr gebrauchten Items
            foreach (var Item in ItemsToDelete)
            {
                await RemoveCategoryValue(Item);
            }

            //Hinzufügen der neuen Items
            foreach (var Item in ItemsToAdd)
            {
                _ = AddCategoryValue(Item);
            }
        }

        /// <summary>
        /// Remove a category value to the list of assigned category values
        /// </summary>
        /// <param name="categoryValueID">The Identifier for category value</param>
        private async Task RemoveCategoryValue(long categoryValueID)
        {
            //Deklaration
            bool ItemFound = false;

            //Laden der Kategorie-Werte wenn noch nicht geschehen
            await _LazyLoader.LoadAsync(this, navigationName: nameof(MediaItemCategoryValues));

            //Entfernen des Items
            foreach (var Item in MediaItemCategoryValues)
            {
                if (Item.CategoryValueID == categoryValueID)
                {
                    ItemFound = true;
                    MediaItemCategoryValues.Remove(Item);
                    break;
                }
            }

            //Item wurde nicht gefunden
            if (!ItemFound) throw new DomainException(DomainExceptionType.NoDataFound);
        }

        /// <summary>
        /// Add multiple category values. Values will only be added if they do not already exist.
        /// </summary>
        /// <param name="values">All category value IDs to add</param>
        public async Task AssignCategoryValues(IEnumerable<long> values)
        {
            //Laden der Kategorie-Werte wenn noch nicht geschehen
            await _LazyLoader.LoadAsync(this, navigationName: nameof(MediaItemCategoryValues));

            //Über alle IDs iterieren
            int Count;
            MediaItemCategoryValue NewItem;
            foreach (var ID in values)
            {
                //Überprüfen ob der Kategorie-Wert schon existiert
                Count = MediaItemCategoryValues.Count(x => x.CategoryValueID == ID);

                //Nur wenn der Kategorie-Wert nich nicht existiert wird er hinzugefügt
                if (Count == 0)
                {
                    //Eine neue User-Entity hinzufügen
                    NewItem = new MediaItemCategoryValue(this, ID);

                    //Hinzufügen des neuen Items zur Liste
                    MediaItemCategoryValues.Add(NewItem);
                }
            }
        }
        #endregion

        #region Called from Change-Tracker
        public override Task EntityModifiedAsync()
        {
            return Task.CompletedTask;
        }

        public override Task EntityAddedAsync()
        {
            //Domain-Events generieren
            AddDomainEvent(new MtrEventMediaItemCreated(Id, MediaType));

            //Funktionsergebnis
            return Task.CompletedTask;
        }

        public override Task EntityDeletedAsync()
        {
            //Feuern des Domain-Events
            var uploadId = MediaType == EnumMediaType.Picture ? UploadPictureID : UploadVideoID;
            if (uploadId is not null)
                AddDomainEvent(new MtrEventMediaItemDeleted(Id, MediaType, uploadId.Value, false));

            //Funktionsergebnis
            return Task.CompletedTask;
        }
        #endregion
    }
}
