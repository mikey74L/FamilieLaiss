namespace FLBackEnd.API.GraphQL.Mutations.MediaGroup;

[GraphQLDescription("InputData type for adding media group")]
public class AddMediaGroupInput
{
    [GraphQLDescription("German name for this media group")]
    public string NameGerman { get; private set; } = string.Empty;

    [GraphQLDescription("English name for this media group")]
    public string NameEnglish { get; private set; } = string.Empty;

    [GraphQLDescription("German description for this media group")]
    public string DescriptionGerman { get; private set; } = string.Empty;

    [GraphQLDescription("English description for this media group")]
    public string DescriptionEnglish { get; private set; } = string.Empty;

    [GraphQLDescription("The date on which the event took place")]
    public DateTimeOffset EventDate { get; private set; }
}
