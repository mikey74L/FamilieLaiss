using FamilieLaissSharedObjects.Enums;
using HotChocolate;

namespace User.API.GraphQL.Mutations.UserMutations
{
    [GraphQLDescription("Input type for changing users")]
    public class UpdateUserInput
    {
        [GraphQLDescription("The Id for the user")]
        public string Id { get; set; }

        [GraphQLDescription("Gender for user")]
        public EnumGenderType Gender { get; set; }

        [GraphQLDescription("Givenname of user")]
        public string GivenName { get; set; }

        [GraphQLDescription("Family name of user")]
        public string FamilyName { get; set; }

        [GraphQLDescription("Streetname of living adress")]
        public string Street { get; set; }

        [GraphQLDescription("Housenumber of living adress")]
        public string Hnr { get; set; }

        [GraphQLDescription("ZIP-Code of living adress")]
        public string Zip { get; set; }

        [GraphQLDescription("Cityname of living adress")]
        public string City { get; set; }

        [GraphQLDescription("CountryId for the country of living adress")]
        public string CountryId { get; set; }
    }
}
