namespace FLBackEnd.API.GraphQL.Mutations.MediaGroup;

[GraphQLDescription("The result for a new added media group")]
public class AddMediaGroupPayload
{
    [GraphQLDescription("The new added media group")]
    public Domain.Entities.MediaGroup MediaGroup { get; set; } = default!;
}
