using DomainHelper.AbstractClasses;
using DomainHelper.Exceptions;
using FamilieLaissSharedObjects.Enums;
using HotChocolate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace FLBackEnd.Domain.Entities;

/// <summary>
/// Entity for media item
/// </summary>
[GraphQLDescription("Media item")]
public class MediaItem : EntityModify<long>
{
    #region Private Members

    private ILazyLoader lazyLoader;

    #endregion

    #region Properties

    /// <summary>
    /// Identifier for the media group
    /// </summary>
    [GraphQLIgnore]
    public long MediaGroupId { get; private set; }

    /// <summary>
    /// The media group this media item entry belongs to
    /// </summary>
    [GraphQLDescription("The media group this media item entry belongs to")]
    public MediaGroup MediaGroup { get; private set; } = default!;

    /// <summary>
    /// The type of Media-Item
    /// </summary>
    [GraphQLDescription("The type of media item")]
    [Required]
    public EnumMediaType MediaType { get; private set; }

    /// <summary>
    /// German name for this Media-Item
    /// </summary>
    [GraphQLDescription("German name for this media item")]
    [GraphQLNonNullType]
    [MaxLength(200)]
    [Required]
    public string NameGerman { get; private set; } = string.Empty;

    /// <summary>
    /// English name for this Media-Item
    /// </summary>
    [GraphQLDescription("English name for this media item")]
    [GraphQLNonNullType]
    [MaxLength(200)]
    [Required]
    public string NameEnglish { get; private set; } = string.Empty;

    /// <summary>
    /// German description for this Media-Item
    /// </summary>
    [GraphQLDescription("German description for this media item")]
    [MaxLength(2000)]
    public string? DescriptionGerman { get; private set; }

    /// <summary>
    /// English description for this Media-Item
    /// </summary>
    [GraphQLDescription("English description for this media item")]
    [MaxLength(2000)]
    public string? DescriptionEnglish { get; private set; }

    /// <summary>
    /// Is this Media-Item only visible for family users
    /// </summary>
    [GraphQLDescription("Is this media item is only visible for family users")]
    [Required]
    public bool OnlyFamily { get; private set; }

    /// <summary>
    /// ID for the assigned upload picture item 
    /// </summary>
    [GraphQLIgnore]
    public long? UploadPictureId { get; private set; }

    /// <summary>
    /// The upload picture for this media item
    /// </summary>
    [GraphQLDescription("The assigned picture if the item is of type picture")]
    public UploadPicture? UploadPicture { get; private set; }

    /// <summary>
    /// ID for the assigned upload video item 
    /// </summary>
    [GraphQLIgnore]
    public long? UploadVideoId { get; private set; }

    /// <summary>
    /// The upload video for this media item
    /// </summary>
    [GraphQLDescription("The assigned video if the item is of type video")]
    public UploadVideo? UploadVideo { get; private set; }

    /// <summary>
    /// List of related media item category values
    /// </summary>
    [GraphQLDescription("List of related media item category values")]
    public ICollection<MediaItemCategoryValue> MediaItemCategoryValues { get; private set; } = default!;

    #endregion

    #region C'tor

    /// <summary>
    /// Constructor is called by ef core
    /// </summary>
    /// <param name="lazyLoader"></param>
    private MediaItem(ILazyLoader lazyLoader)
    {
        this.lazyLoader = lazyLoader;
    }

    /// <summary>
    /// Constructor is called by GraphQL
    /// </summary>
    private MediaItem()
    {
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mediaGroup">Media group this media item belongs to</param>
    /// <param name="mediaType">The type of Media-Item</param>
    /// <param name="nameGerman">German name for this Media-Item</param>
    /// <param name="nameEnglish">English name for this Media-Item</param>
    /// <param name="descriptionGerman">German description for this Media-Item</param>
    /// <param name="descriptionEnglish">English description for this Media-Item</param>
    /// <param name="onlyFamily">Is this Media-Item only visible for family users</param>
    /// <param name="uploadItemId">Id for the assigned upload item</param>
    /// <param name="nameUploadPicture">The filename from the original upload picture</param>
    internal MediaItem(MediaGroup mediaGroup, EnumMediaType mediaType, string nameGerman, string nameEnglish,
        string? descriptionGerman,
        string? descriptionEnglish, bool onlyFamily, long uploadItemId, string? nameUploadPicture)
    {
        if (MediaType == EnumMediaType.Video && string.IsNullOrEmpty(nameGerman))
        {
            throw new DomainException("A german name is needed");
        }

        if (MediaType == EnumMediaType.Video && string.IsNullOrEmpty(nameEnglish))
        {
            throw new DomainException("A english name is needed");
        }

        MediaGroup = mediaGroup;
        MediaType = mediaType;
        NameGerman = nameGerman;
        NameEnglish = nameEnglish;
        DescriptionGerman = descriptionGerman;
        DescriptionEnglish = descriptionEnglish;
        OnlyFamily = onlyFamily;
        if (MediaType == EnumMediaType.Picture)
        {
            UploadPictureId = uploadItemId;
        }
        else
        {
            UploadVideoId = uploadItemId;
        }

        if (MediaType != EnumMediaType.Picture) return;
        NameGerman = nameUploadPicture ?? "";
        NameEnglish = nameUploadPicture ?? "";
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Remove a category value to the list of assigned category values
    /// </summary>
    /// <param name="categoryValueId">The Identifier for category value</param>
    private async Task RemoveCategoryValue(long categoryValueId)
    {
        //Deklaration
        bool ItemFound = false;

        //Laden der Kategorie-Werte wenn noch nicht geschehen
        await lazyLoader.LoadAsync(this, navigationName: nameof(MediaItemCategoryValues));

        //Entfernen des Items
        foreach (var Item in MediaItemCategoryValues)
        {
            if (Item.CategoryValueId == categoryValueId)
            {
                ItemFound = true;
                MediaItemCategoryValues.Remove(Item);
                break;
            }
        }

        //Item wurde nicht gefunden
        if (!ItemFound)
        {
            throw new DomainException(DomainExceptionType.NoDataFound,
                $"Category value with ID = {categoryValueId} not found.");
        }
    }

    #endregion

    #region Domain Methods

    [GraphQLIgnore]
    public void Update(string nameGerman, string nameEnglish, string? descriptionGerman, string? descriptionEnglish,
        bool onlyFamily)
    {
        if (string.IsNullOrEmpty(nameGerman) && MediaType == EnumMediaType.Video)
        {
            throw new DomainException("A german name is needed");
        }

        if (string.IsNullOrEmpty(nameEnglish) && MediaType == EnumMediaType.Video)
        {
            throw new DomainException("A english name is needed");
        }

        if (MediaType !=
            EnumMediaType
                .Picture) //Nachfolgende nur übernehmen wenn es sich nicht um Fotos handelt da diese keinen Namen und Beschreibung verwenden
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
    /// <param name="categoryValueId">Identifier for category value</param>
    /// <returns>The added media item category value</returns>
    [GraphQLIgnore]
    public MediaItemCategoryValue AddCategoryValue(long categoryValueId)
    {
        var ValueEntity = new MediaItemCategoryValue(this, categoryValueId);

        if (MediaItemCategoryValues == null)
        {
            MediaItemCategoryValues = [];
        }

        MediaItemCategoryValues.Add(ValueEntity);

        return ValueEntity;
    }

    /// <summary>
    /// Updates the assigned category values
    /// </summary>
    /// <param name="values">List of assigned category value IDs</param>
    [GraphQLIgnore]
    public async Task UpdateCategoryValues(IEnumerable<long> values)
    {
        //Laden der Kategorie-Werte wenn noch nicht geschehen
        await lazyLoader.LoadAsync(this, navigationName: nameof(MediaItemCategoryValues));

        //Ermitteln der zu löschenden Items
        List<long> ItemsToDelete = [];
        bool Found = false;
        foreach (var AssignedValue in MediaItemCategoryValues)
        {
            foreach (var NewValue in values)
            {
                if (NewValue == AssignedValue.CategoryValueId)
                {
                    Found = true;
                }
            }

            if (Found == false)
            {
                ItemsToDelete.Add(AssignedValue.CategoryValueId);
            }
            else
            {
                Found = false;
            }
        }

        //Ermitteln der Items die hinzugefügt werden müssen
        List<long> ItemsToAdd = [];
        foreach (var NewValue in values)
        {
            foreach (var AssignedValue in MediaItemCategoryValues)
            {
                if (NewValue == AssignedValue.CategoryValueId)
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
            AddCategoryValue(Item);
        }
    }

    /// <summary>
    /// Add multiple category values. Values will only be added if they do not already exist.
    /// </summary>
    /// <param name="values">All category value IDs to add</param>
    public async Task AssignCategoryValues(IEnumerable<long> values)
    {
        //Laden der Kategorie-Werte wenn noch nicht geschehen
        await lazyLoader.LoadAsync(this, navigationName: nameof(MediaItemCategoryValues));

        //Über alle IDs iterieren
        int Count;
        MediaItemCategoryValue NewItem;
        foreach (var ID in values)
        {
            //Überprüfen ob der Kategorie-Wert schon existiert
            Count = MediaItemCategoryValues.Count(x => x.CategoryValueId == ID);

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

    [GraphQLIgnore]
    public override Task EntityModifiedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
    {
        return Task.CompletedTask;
    }

    [GraphQLIgnore]
    public override async Task EntityAddedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
    {
        if (MediaType == EnumMediaType.Picture)
        {
            var uploadPicture = await dbContext.FindAsync<UploadPicture>(UploadPictureId);

            if (uploadPicture is not null)
            {
                uploadPicture.SetPictureStateToAssigned(Id);
                dbContext.Update(uploadPicture);
            }
        }
        else
        {
            var uploadVideo = await dbContext.FindAsync<UploadVideo>(UploadVideoId);

            if (uploadVideo is not null)
            {
                uploadVideo.SetVideoStateToAssigned();
                dbContext.Update(uploadVideo);
            }
        }
    }

    [GraphQLIgnore]
    public override async Task EntityDeletedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
    {
        if (MediaType == EnumMediaType.Picture)
        {
            await lazyLoader.LoadAsync(this, navigationName: nameof(UploadPicture));

            if (UploadPicture is not null)
            {
                if (dictContextParams["KeepUploadItem"] is bool keepUploadItem && keepUploadItem)
                {
                    UploadPicture.SetPictureStateToUnAssigned();
                    dbContext.Update(UploadPicture);
                }
                else
                {
                    dbContext.Remove(UploadPicture);

                    await UploadPicture.EntityDeletedAsync(dbContext, dictContextParams);
                }
            }
        }
        else
        {
            await lazyLoader.LoadAsync(this, navigationName: nameof(UploadVideo));

            if (UploadVideo is not null)
            {
                if (dictContextParams["KeepUploadItem"] is bool keepUploadItem && keepUploadItem)
                {
                    UploadVideo.SetVideoStateToUnAssigned();
                    dbContext.Update(UploadVideo);
                }
                else
                {
                    dbContext.Remove(UploadVideo);

                    await UploadVideo.EntityDeletedAsync(dbContext, dictContextParams);
                }
            }
        }
    }

    #endregion
}