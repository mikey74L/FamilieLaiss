using HotChocolate;

namespace User.API.GraphQL.Mutations.UserMutations
{
    [GraphQLDescription("Input type for adding users")]
    public class AddUserInput
    {
        [GraphQLDescription("The ID for the user")]
        public string ID { get; set; }

        [GraphQLDescription("The eMail-Adress for the user")]
        public string EMail { get; set; }

        [GraphQLDescription("Username for the user")]
        public string UserName { get; set; }
    }
}
