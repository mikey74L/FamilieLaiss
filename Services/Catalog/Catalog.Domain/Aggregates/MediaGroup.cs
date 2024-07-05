using Catalog.Domain.DomainEvents;
using DomainHelper.AbstractClasses;
using DomainHelper.Exceptions;
using FamilieLaissSharedObjects.Enums;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Domain.Aggregates
{
    /// <summary>
    /// Entity for media group
    /// </summary>
    public class MediaGroup : EntityModify<long>
    {
        #region Private Members
        private ILazyLoader _LazyLoader;
        #endregion

        #region Properties
        /// <summary>
        /// German name for this Media-Group
        /// </summary>
        public string NameGerman { get; private set; }

        /// <summary>
        /// English name for this Media-Group
        /// </summary>
        public string NameEnglish { get; private set; }

        /// <summary>
        /// German description for this Media-Group
        /// </summary>
        public string DescriptionGerman { get; private set; }

        /// <summary>
        /// English description for this Media-Group
        /// </summary>
        public string DescriptionEnglish { get; private set; }

        /// <summary>
        /// The date on which the event took place
        /// </summary>
        public DateTimeOffset EventDate { get; private set; }

        /// <summary>
        /// List of related media items
        /// </summary>
        public ICollection<MediaItem> MediaItems { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor would be used by EF-Core
        /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private MediaGroup(ILazyLoader lazyLoader)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            _LazyLoader = lazyLoader;
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected MediaGroup()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {

        }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="descriptionEnglish">English description for this Media-Group</param>
        /// <param name="descriptionGerman">German description for this Media-Group</param>
        /// <param name="nameEnglish">English name for this Media-Group</param>
        /// <param name="nameGerman">German name for this Media-Group</param>
        /// <param name="eventDate">The date on which the event took place</param>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public MediaGroup(string nameGerman, string nameEnglish, string descriptionGerman, string descriptionEnglish,
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            DateTimeOffset eventDate) : base()
        {
            //Überprüfen ob ein deutscher Name vorhanden ist
            if (string.IsNullOrEmpty(nameGerman)) throw new DomainException("A german name is needed for this Media-Group");

            //Überprüfen ob ein englischer Name vorhanden ist
            if (string.IsNullOrEmpty(nameEnglish)) throw new DomainException("A english name is needed for this Media-Group");

            //Übernehmen der Werte
            NameGerman = nameGerman;
            NameEnglish = nameEnglish;
            DescriptionGerman = descriptionGerman;
            DescriptionEnglish = descriptionEnglish;
            EventDate = eventDate;
        }
        #endregion

        #region Domain-Methods
        /// <summary>
        /// Update the media group
        /// </summary>
        /// <param name="descriptionEnglish">English description for this Media-Group</param>
        /// <param name="descriptionGerman">German description for this Media-Group</param>
        /// <param name="nameEnglish">English name for this Media-Group</param>
        /// <param name="nameGerman">German name for this Media-Group</param>
        /// <param name="eventDate">The date on which the event took place</param>
        public void Update(string nameGerman, string nameEnglish, string descriptionGerman, string descriptionEnglish,
            DateTimeOffset eventDate)
        {
            //Überprüfen ob ein deutscher Name vorhanden ist
            if (string.IsNullOrEmpty(nameGerman)) throw new DomainException("A german name is needed for this Media-Group");

            //Überprüfen ob ein englischer Name vorhanden ist
            if (string.IsNullOrEmpty(nameEnglish)) throw new DomainException("A english name is needed for this Media-Group");

            //Übernehmen der Werte
            NameGerman = nameGerman;
            NameEnglish = nameEnglish;
            DescriptionGerman = descriptionGerman;
            DescriptionEnglish = descriptionEnglish;
            EventDate = eventDate;
        }

        /// <summary>
        /// Add a media item to a media group
        /// </summary>
        /// <param name="id">The Identifier for the media item</param>
        /// <param name="mediaType">The type of Media-Item</param>
        /// <param name="nameGerman">German name for this Media-Item</param>
        /// <param name="nameEnglish">English name for this Media-Item</param>
        /// <param name="descriptionGerman">German description for this Media-Item</param>
        /// <param name="descriptionEnglish">English description for this Media-Item</param>
        /// <param name="onlyFamily">Is this Media-Item only visible for family users</param>
        /// <param name="uploadItemID">Id for the assigned upload item (picture or video)</param>
        /// <returns>The added media item</returns>
        public MediaItem AddMediaItem(EnumMediaType mediaType, string nameGerman, string nameEnglish, string descriptionGerman,
            string descriptionEnglish, bool onlyFamily, long uploadItemID)
        {
            //Eine neue Media-Item-Entity hinzufügen
            var ValueEntity = new MediaItem(this, mediaType, nameGerman, nameEnglish, descriptionGerman, descriptionEnglish, onlyFamily,
                uploadItemID);

            //Wenn die Liste noch null sein sollte erstellen einer leeren Liste
            if (MediaItems == null)
            {
                MediaItems = new HashSet<MediaItem>();
            }

            //Hinzufügen der Value-Entity zur Collection
            MediaItems.Add(ValueEntity);

            //Hinzufügen des Domain-Events
            if (mediaType == EnumMediaType.Picture && ValueEntity.UploadPictureID is not null)
            {
                AddDomainEvent(new MtrEventUploadPictureAssigned(ValueEntity.UploadPictureID.Value));
            }
            else if (mediaType == EnumMediaType.Video && ValueEntity.UploadVideoID is not null)
            {
                AddDomainEvent(new MtrEventUploadVideoAssigned(ValueEntity.UploadVideoID.Value));
            }

            //Zurückliefern der hinzugefügten Entity
            return ValueEntity;
        }

        /// <summary>
        /// Remove a media item from this media group
        /// </summary>
        /// <param name="mediaItemID">The Identifier for media item</param>
        /// <param name="deleteUploadItem">Delete upload item?</param>
        public async Task<MediaItem> RemoveMediaItem(long mediaItemID, bool deleteUploadItem)
        {
            //Deklaration
            MediaItem? result = null;

            //Laden der Kategorie - Werte wenn noch nicht geschehen
            await _LazyLoader.LoadAsync(this, navigationName: nameof(MediaItems));

            //Entfernen des Items
            foreach (var Item in MediaItems)
            {
                if (Item.Id == mediaItemID)
                {
                    result = Item;
                    MediaItems.Remove(Item);

                    //Hier muss auch das Domain-Event gefeuert werden
                    var uploadId = Item.MediaType == EnumMediaType.Picture ? Item.UploadPictureID : Item.UploadVideoID;
                    if (uploadId is not null)
                        AddDomainEvent(new MtrEventMediaItemDeleted(Item.Id, Item.MediaType, uploadId.Value, deleteUploadItem));

                    break;
                }
            }

            //Item wurde nicht gefunden
            if (result is null)
            {
                throw new DomainException(DomainExceptionType.NoDataFound);
            }
            else
            {
                return result;
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
            //Domain-Event hinzufügen
            AddDomainEvent(new MtrEventMediaGroupCreated(Id, NameGerman, NameEnglish));

            //Ergebnis zurückliefern
            return Task.CompletedTask;
        }

        public override async Task EntityDeletedAsync()
        {
            //Laden der zugewiesenen Media-Items
            await _LazyLoader.LoadAsync(this, navigationName: nameof(MediaItems));

            //Für jedes zugeordnete Media-Element die Delete-Methode aufrufen
            foreach (var Element in MediaItems)
            {
                await Element.EntityDeletedAsync();
            }
        }
        #endregion
    }
}
