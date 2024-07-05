namespace FLBackEnd.API.GraphQL.Mutations.MediaGroup;

[GraphQLDescription("The result for a updated media group")]
public class UpdateMediaGroupPayload
{
    [GraphQLDescription("The updated media group")]
    public Domain.Entities.MediaGroup MediaGroup { get; set; } = default!;
}
