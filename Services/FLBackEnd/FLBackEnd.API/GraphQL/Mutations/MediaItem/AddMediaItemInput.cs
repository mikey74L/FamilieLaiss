using FamilieLaissSharedObjects.Enums;

namespace FLBackEnd.API.GraphQL.Mutations.MediaItem;

[GraphQLDescription("InputData type for adding media item")]
public class AddMediaItemInput
{
    [GraphQLDescription("The unique identifier for the media group this media item belongs to")]
    public long MediaGroupId { get; private set; }

    [GraphQLDescription("The type of media item")]
    public EnumMediaType MediaType { get; private set; }

    [GraphQLDescription("German name for this media item")]
    public string NameGerman { get; private set; } = string.Empty;

    [GraphQLDescription("English name for this media item")]
    public string NameEnglish { get; private set; } = string.Empty;

    [GraphQLDescription("German description for this media item")]
    public string? DescriptionGerman { get; private set; }

    [GraphQLDescription("English description for this media item")]
    public string? DescriptionEnglish { get; private set; }

    [GraphQLDescription("Is this media item only visible for family users")]
    public bool OnlyFamily { get; private set; }

    [GraphQLDescription("The unique identifier for the assigned upload item (picture or video)")]
    public long UploadItemId { get; private set; }

    [GraphQLDescription("The list of unique identifiers for the category values added to this media item")]
    public List<long> CategoryValueIds { get; private set; } = [];
}
