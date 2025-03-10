using HotChocolate;

namespace User.API.GraphQL.Mutations.UserMutations;

[GraphQLDescription("The result for a new added user")]
public class AddUserPayload
{
    [GraphQLDescription("The new added user")]
    public User.Domain.Aggregates.UserAccount UserAccount { get; set; }
}
