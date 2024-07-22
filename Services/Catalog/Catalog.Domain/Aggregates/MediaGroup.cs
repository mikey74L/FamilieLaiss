using Catalog.Domain.DomainEvents.MediaGroup;
using DomainHelper.AbstractClasses;
using DomainHelper.Exceptions;
using FamilieLaissSharedObjects.Enums;
using HotChocolate;
using HotChocolate.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Catalog.Domain.Aggregates;

/// <summary>
/// Entity for media group
/// </summary>
[GraphQLDescription("Media group")]
public class MediaGroup : EntityModify<long>
{
    #region Private Members

    private ILazyLoader lazyLoader;

    #endregion

    #region Properties

    /// <summary>
    /// German name for this Media-Group
    /// </summary>
    [GraphQLDescription("German name for this media group")]
    [GraphQLNonNullType]
    [MaxLength(300)]
    [Required]
    public string NameGerman { get; private set; } = string.Empty;

    /// <summary>
    /// English name for this Media-Group
    /// </summary>
    [GraphQLDescription("English name for this media group")]
    [GraphQLNonNullType]
    [MaxLength(300)]
    [Required]
    public string NameEnglish { get; private set; } = string.Empty;

    /// <summary>
    /// German description for this Media-Group
    /// </summary>
    [GraphQLDescription("German description for this media group")]
    [GraphQLNonNullType]
    [MaxLength(3000)]
    [Required]
    public string DescriptionGerman { get; private set; } = string.Empty;

    /// <summary>
    /// English description for this Media-Group
    /// </summary>
    [GraphQLDescription("English description for this media group")]
    [GraphQLNonNullType]
    [MaxLength(3000)]
    [Required]
    public string DescriptionEnglish { get; private set; } = string.Empty;

    /// <summary>
    /// The date on which the event took place
    /// </summary>
    [GraphQLDescription("The date on which the event took place")]
    public DateTimeOffset EventDate { get; private set; }

    /// <summary>
    /// List of related media items
    /// </summary>
    [GraphQLDescription("List of related media items")]
    [UseFiltering]
    [UseSorting]
    public ICollection<MediaItem> MediaItems { get; private set; } = [];

    #endregion

    #region C'tor

    /// <summary>
    /// Constructor would be used by EF-Core
    /// </summary>
    private MediaGroup(ILazyLoader lazyLoader)
    {
        this.lazyLoader = lazyLoader;
    }

    /// <summary>
    /// Constructor would be used by GraphQL
    /// </summary>
    private MediaGroup()
    {
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="descriptionEnglish">English description for this Media-Group</param>
    /// <param name="descriptionGerman">German description for this Media-Group</param>
    /// <param name="nameEnglish">English name for this Media-Group</param>
    /// <param name="nameGerman">German name for this Media-Group</param>
    /// <param name="eventDate">The date on which the event took place</param>
    public MediaGroup(string nameGerman, string nameEnglish, string descriptionGerman, string descriptionEnglish,
        DateTimeOffset eventDate) : base()
    {
        if (string.IsNullOrEmpty(nameGerman))
        {
            throw new DomainException("A german name is needed for this Media-Group");
        }

        if (string.IsNullOrEmpty(nameEnglish))
        {
            throw new DomainException("A english name is needed for this Media-Group");
        }

        NameGerman = nameGerman;
        NameEnglish = nameEnglish;
        DescriptionGerman = descriptionGerman;
        DescriptionEnglish = descriptionEnglish;
        EventDate = eventDate;
    }

    #endregion

    #region Domain Methods

    /// <summary>
    /// Update the media group
    /// </summary>
    /// <param name="nameGerman">German name for this media group</param>
    /// <param name="nameEnglish">English name for this media group</param>
    /// <param name="descriptionEnglish">English description for this Media-Group</param>
    /// <param name="descriptionGerman">German description for this Media-Group</param>
    /// <param name="eventDate">The date on which the event took place</param>
    [GraphQLIgnore]
    public void Update(string? nameGerman, string? nameEnglish, string descriptionGerman, string descriptionEnglish,
        DateTimeOffset eventDate)
    {
        if (string.IsNullOrEmpty(nameGerman))
        {
            throw new DomainException("A german name is needed for this Media-Group");
        }

        if (string.IsNullOrEmpty(nameEnglish))
        {
            throw new DomainException("A english name is needed for this Media-Group");
        }

        NameGerman = nameGerman;
        NameEnglish = nameEnglish;
        DescriptionGerman = descriptionGerman;
        DescriptionEnglish = descriptionEnglish;
        EventDate = eventDate;
    }

    /// <summary>
    /// Add a media item to a media group
    /// </summary>
    /// <param name="mediaType">The type of Media-Item</param>
    /// <param name="nameGerman">German name for this Media-Item</param>
    /// <param name="nameEnglish">English name for this Media-Item</param>
    /// <param name="descriptionGerman">German description for this Media-Item</param>
    /// <param name="descriptionEnglish">English description for this Media-Item</param>
    /// <param name="onlyFamily">Is this Media-Item only visible for family users</param>
    /// <param name="uploadItemId">Id for the assigned upload item (picture or video)</param>
    /// <param name="nameUploadPicture">The filename of the original upload picture</param>
    /// <returns>The added media item</returns>
    [GraphQLIgnore]
    public MediaItem AddMediaItem(EnumMediaType mediaType, string nameGerman, string nameEnglish,
        string? descriptionGerman,
        string? descriptionEnglish, bool onlyFamily, long uploadItemId, string? nameUploadPicture)
    {
        //Eine neue Media-Item-Entity hinzufügen
        var valueEntity = new MediaItem(this, mediaType, nameGerman, nameEnglish, descriptionGerman, descriptionEnglish,
            onlyFamily,
            uploadItemId, nameUploadPicture);

        //Wenn die Liste noch null sein sollte erstellen einer leeren Liste
        MediaItems ??= [];

        //Hinzufügen der Value-Entity zur Collection
        MediaItems.Add(valueEntity);

        //Zurückliefern der hinzugefügten Entity
        return valueEntity;
    }

    /// <summary>
    /// Remove a media item from this media group
    /// </summary>
    /// <param name="mediaItemId">The Identifier for media item</param>
    [GraphQLIgnore]
    public async Task<MediaItem> RemoveMediaItem(long mediaItemId)
    {
        //Deklaration
        MediaItem? result = null;

        //Laden der Kategorie - Werte wenn noch nicht geschehen
        await lazyLoader.LoadAsync(this, navigationName: nameof(MediaItems));

        //Entfernen des Items
        foreach (var item in MediaItems)
        {
            if (item.Id != mediaItemId) continue;
            result = item;
            MediaItems.Remove(item);

            break;
        }

        //Item wurde nicht gefunden
        if (result is null)
        {
            throw new DomainException(DomainExceptionType.NoDataFound,
                $"Media item with ID = {mediaItemId} not found.");
        }
        else
        {
            return result;
        }
    }

    #endregion

    #region Called from Change-Tracker

    [GraphQLIgnore]
    public override Task EntityModifiedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
    {
        AddDomainEvent(new DomainEventMediaGroupChanged(Id));

        return Task.CompletedTask;
    }

    [GraphQLIgnore]
    public override Task EntityAddedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
    {
        AddDomainEvent(new DomainEventMediaGroupCreated(Id));

        return Task.CompletedTask;
    }

    [GraphQLIgnore]
    public override async Task EntityDeletedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
    {
        AddDomainEvent(new DomainEventMediaGroupDeleted(Id));

        await lazyLoader.LoadAsync(this, navigationName: nameof(MediaItems));

        foreach (var item in MediaItems)
        {
            await item.EntityDeletedAsync(dbContext, dictContextParams);
        }
    }

    #endregion
}