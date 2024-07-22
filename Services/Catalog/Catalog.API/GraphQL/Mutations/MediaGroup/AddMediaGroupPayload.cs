namespace Catalog.API.GraphQL.Mutations.MediaGroup;

[GraphQLDescription("The result for a new added media group")]
public class AddMediaGroupPayload
{
    [GraphQLDescription("The new added media group")]
    public Domain.Aggregates.MediaGroup MediaGroup { get; set; } = default!;
}