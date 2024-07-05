namespace FLBackEnd.API.GraphQL.Mutations.MediaGroup;

[GraphQLDescription("The result for a deleted media group")]
public class DeleteMediaGroupPayload
{
    [GraphQLDescription("The deleted media group")]
    public Domain.Entities.MediaGroup MediaGroup { get; set; } = default!;
}
