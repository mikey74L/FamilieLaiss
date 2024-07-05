using FamilieLaissSharedObjects.Attributes;
using HotChocolate;

namespace FamilieLaissSharedObjects.Enums;

/// <summary>
/// Enumeration for setting the category type for categories
/// </summary>
[GraphQLDescription("Enumeration for setting the category type for categories")]
public enum EnumCategoryType : byte
{
    /// <summary>
    /// Is usable for picture and photo
    /// </summary>
    [DescriptionTranslation("Foto / Video", "Photo / Video")]
    [GraphQLDescription("Is usable for picture and photo")]
    Both = 0,

    /// <summary>
    /// Is usable for Picture
    /// </summary>
    [DescriptionTranslation("Foto", "Photo")]
    [GraphQLDescription("Is usable for Picture")]
    Picture = 1,

    /// <summary>
    /// Is usable for video
    /// </summary>
    [DescriptionTranslation("Video", "Video")]
    [GraphQLDescription("Is usable for video")]
    Video = 2,
}