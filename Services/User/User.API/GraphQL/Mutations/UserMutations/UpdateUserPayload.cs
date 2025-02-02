using HotChocolate;

namespace User.API.GraphQL.Mutations.UserMutations;

[GraphQLDescription("The result for a changed user")]
public class UpdateUserPayload
{
    [GraphQLDescription("The changed user")]
    public User.Domain.Aggregates.UserAccount UserAccount { get; set; }
}
