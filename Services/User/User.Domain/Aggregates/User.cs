using DomainHelper.AbstractClasses;
using FamilieLaissSharedObjects.Enums;
using HotChocolate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations;
using User.Domain.DomainEvents.User;

namespace User.Domain.Aggregates;

[GraphQLDescription("User")]
public class UserAccount : EntityModify<string>
{
    #region Private Members
    private ILazyLoader? _LazyLoader;
    #endregion

    #region Properties
    /// <summary>
    /// eMail-Adress for user
    /// </summary>
    [GraphQLDescription("The eMail-Adress for the user")]
    [Required]
    [MaxLength(100)]
    public string EMail { get; private set; } = string.Empty;

    /// <summary>
    /// The user name (Display Name)
    /// </summary>
    [GraphQLDescription("The user name for the user")]
    [Required]
    [MaxLength(15)]
    public string UserName { get; private set; } = string.Empty;

    /// <summary>
    /// The gender for the user
    /// </summary>
    [GraphQLDescription("The gender for the user")]
    public EnumGenderType Gender { get; private set; }

    /// <summary>
    /// The given name
    /// </summary>
    [GraphQLDescription("The given name for the user")]
    [MaxLength(100)]
    public string? GivenName { get; private set; }

    /// <summary>
    /// The family name
    /// </summary>
    [GraphQLDescription("The family name for the user")]
    [MaxLength(100)]
    public string? FamilyName { get; private set; }

    /// <summary>
    /// The name of the street
    /// </summary>
    [GraphQLDescription("The street name for the adress of the user")]
    [MaxLength(100)]
    public string? Street { get; private set; }

    /// <summary>
    /// The house number
    /// </summary>
    [GraphQLDescription("The house number for the adress of the user")]
    [MaxLength(20)]
    public string? Hnr { get; private set; }

    /// <summary>
    /// The postal code
    /// </summary>
    [GraphQLDescription("The ZIP-Code for the adress of the user")]
    [MaxLength(20)]
    public string? Zip { get; private set; }

    /// <summary>
    /// The name of the city
    /// </summary>
    [GraphQLDescription("The name of the city the user lives in")]
    [MaxLength(100)]
    public string? City { get; private set; }

    /// <summary>
    /// The ID of the assigned country
    /// </summary>
    [GraphQLIgnore]
    public string? CountryId { get; private set; }

    /// <summary>
    /// The country where the user lives
    /// </summary>
    [GraphQLDescription("The assigned country the user lives in")]
    [UseFiltering]
    public Country? Country { get; private set; }
    #endregion

    #region C'tor
    /// <summary>
    /// C'tor would be used by GraphQl
    /// </summary>
    private UserAccount()
    {
    }

    /// <summary>
    /// C'tor  would be used by EF-Core
    /// </summary>
    private UserAccount(ILazyLoader lazyLoader)
    {
        _LazyLoader = lazyLoader;
    }

    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="userId">The ID for the user from Auth0</param>
    /// <param name="eMail">The eMail-Adress for the user</param>
    /// <param name="userName">The user name for the user</param>
    /// <param name="gender">The gender for the user</param>
    /// <param name="givenName">The given name for the user</param>
    /// <param name="familyName">The family name for the user</param>
    /// <param name="street">The street name for the adress of the user</param>
    /// <param name="hnr">The house number for the adress of the user</param>
    /// <param name="zip">The ZIP-Code for the adress of the user</param>
    /// <param name="city">The name of the city the user lives in</param>
    /// <param name="countryCode">The country code for the country the user lives in</param>
    public UserAccount(string userId, string userName, string eMail, EnumGenderType gender, string givenName, string familyName,
        string street, string hnr, string zip, string city, string countryCode)
    {
        //Übernehmen der Werte
        Id = userId;
        EMail = eMail;
        UserName = userName;
        EMail = eMail;
        Gender = gender;
        GivenName = givenName;
        FamilyName = familyName;
        Street = street;
        Hnr = hnr;
        Zip = zip;
        City = city;
        CountryId = countryCode;
    }
    #endregion

    #region Domain Methods
    /// <summary>
    /// Update the user
    /// </summary>
    /// <param name="gender">The Gender</param>
    /// <param name="givenName">The given name</param>
    /// <param name="familyName">The family name</param>
    /// <param name="street">The name of the street</param>
    /// <param name="hnr">The house number</param>
    /// <param name="zip">The postal code</param>
    /// <param name="city">The name of the city</param>
    /// <param name="countryId">The Country-ID for the country</param>
    public void Update(EnumGenderType gender, string givenName, string familyName, string street, string hnr, string zip, string city,
        string countryId)
    {
        //Übernehmen der Properties
        Gender = gender;
        GivenName = givenName;
        FamilyName = familyName;
        Street = street;
        Hnr = hnr;
        Zip = zip;
        City = city;
        CountryId = countryId;
    }


    #endregion

    #region Called from Change-Tracker
    public override Task EntityModifiedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
    {
        AddDomainEvent(new DomainEventUserCreated(Id));

        return Task.CompletedTask;
    }

    public override Task EntityAddedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
    {
        AddDomainEvent(new DomainEventUserChanged(Id));

        return Task.CompletedTask;
    }

    public override Task EntityDeletedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams)
    {
        AddDomainEvent(new DomainEventUserDeleted(Id));

        return Task.CompletedTask;
    }
    #endregion
}
