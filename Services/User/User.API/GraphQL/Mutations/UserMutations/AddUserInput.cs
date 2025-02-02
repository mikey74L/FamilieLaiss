using FamilieLaissSharedObjects.Enums;
using HotChocolate;

namespace User.API.GraphQL.Mutations.UserMutations;

[GraphQLDescription("Input type for adding users")]
public class AddUserInput
{
    [GraphQLDescription("The Id for the user")]
    public string Id { get; set; } = string.Empty;

    [GraphQLDescription("The eMail-Adress for the user")]
    public string EMail { get; private set; } = string.Empty;

    [GraphQLDescription("The user name for the user")]
    public string UserName { get; private set; } = string.Empty;

    [GraphQLDescription("Gender for user")]
    public EnumGenderType Gender { get; set; }

    [GraphQLDescription("Givenname of user")]
    public string GivenName { get; set; } = string.Empty;

    [GraphQLDescription("Family name of user")]
    public string FamilyName { get; set; } = string.Empty;

    [GraphQLDescription("Streetname of living adress")]
    public string Street { get; set; } = string.Empty;

    [GraphQLDescription("Housenumber of living adress")]
    public string Hnr { get; set; } = string.Empty;

    [GraphQLDescription("ZIP-Code of living adress")]
    public string Zip { get; set; } = string.Empty;

    [GraphQLDescription("Cityname of living adress")]
    public string City { get; set; } = string.Empty;

    [GraphQLDescription("CountryId for the country of living adress")]
    public string CountryId { get; set; } = string.Empty;
}
