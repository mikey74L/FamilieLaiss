namespace Catalog.API.GraphQL.Mutations.MediaGroup;

[GraphQLDescription("The result for a deleted media group")]
public class DeleteMediaGroupPayload
{
    [GraphQLDescription("The deleted media group")]
    public Domain.Aggregates.MediaGroup MediaGroup { get; set; } = default!;
}