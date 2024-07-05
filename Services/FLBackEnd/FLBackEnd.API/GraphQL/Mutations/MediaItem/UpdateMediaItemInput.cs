namespace FLBackEnd.API.GraphQL.Mutations.MediaItem;

[GraphQLDescription("InputData type for updating media item")]
public class UpdateMediaItemInput
{
    [GraphQLDescription("The unique identifier for the media item to update")]
    public long Id { get; set; }

    [GraphQLDescription("German name for this media item")]
    public string NameGerman { get; private set; } = string.Empty;

    [GraphQLDescription("English name for this media item")]
    public string NameEnglish { get; private set; } = string.Empty;

    [GraphQLDescription("German description for this media item")]
    public string? DescriptionGerman { get; private set; } = string.Empty;

    [GraphQLDescription("English description for this media item")]
    public string? DescriptionEnglish { get; private set; } = string.Empty;

    [GraphQLDescription("Is this media item only visible for family users")]
    public bool OnlyFamily { get; private set; }

    [GraphQLDescription("The list of unique identifiers for the category values added to this media item")]
    public List<long> CategoryValueIds { get; private set; } = [];
}
