namespace Catalog.API.GraphQL.Mutations.MediaGroup;

[GraphQLDescription("The result for a updated media group")]
public class UpdateMediaGroupPayload
{
    [GraphQLDescription("The updated media group")]
    public Domain.Aggregates.MediaGroup MediaGroup { get; set; } = default!;
}