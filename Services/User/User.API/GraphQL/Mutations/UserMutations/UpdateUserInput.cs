using HotChocolate;

namespace User.API.GraphQL.Mutations.UserMutations
{
    [GraphQLDescription("Input type for changing users")]
    public class UpdateUserInput
    {
        [GraphQLDescription("The ID for the user")]
        public string ID { get; set; }

        [GraphQLDescription("Gender ID of user")]
        public string GenderID { get; set; }

        [GraphQLDescription("Givenname of user")]
        public string GivenName { get; set; }

        [GraphQLDescription("Family name of user")]
        public string FamilyName { get; set; }

        [GraphQLDescription("Streetname of living adress")]
        public string Street { get; set; }

        [GraphQLDescription("Housenumber of living adress")]
        public string HNR { get; set; }

        [GraphQLDescription("ZIP-Code of living adress")]
        public string ZIP { get; set; }

        [GraphQLDescription("Cityname of living adress")]
        public string City { get; set; }    

        [GraphQLDescription("CountryID for the country of living adress")]
        public string CountryID { get; set; }
    }
}
