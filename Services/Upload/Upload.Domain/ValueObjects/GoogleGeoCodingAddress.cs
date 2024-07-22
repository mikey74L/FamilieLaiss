using DomainHelper.AbstractClasses;
using DomainHelper.Exceptions;
using HotChocolate;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Upload.Domain.ValueObjects;

/// <summary>
/// Value-Object for Google-Geo-Coding-Addresses
/// </summary>
[GraphQLDescription("Geo coding address")]
public class GoogleGeoCodingAddress : ValueObject
{
    #region Public Properties

    /// <summary>
    /// Longitude for GPS-Position
    /// </summary>
    [GraphQLDescription("The longitude for the gps position")]
    public double Longitude { get; private set; }

    /// <summary>
    /// Latitude for GPS-Position
    /// </summary>
    [GraphQLDescription("The latitude for the gps position")]
    public double Latitude { get; private set; }

    /// <summary>
    /// Street-Name
    /// </summary>
    [MaxLength(100)]
    [GraphQLDescription("The street name for the geo coding address")]
    public string StreetName { get; private set; } = string.Empty;

    /// <summary>
    /// The house number 
    /// </summary>
    [MaxLength(10)]
    [GraphQLDescription("The house number for the geo coding address")]
    public string Hnr { get; private set; } = string.Empty;

    /// <summary>
    /// The ZIP-Code
    /// </summary>
    [MaxLength(10)]
    [GraphQLDescription("The zip code for the geo coding address")]
    public string Zip { get; private set; } = string.Empty;

    /// <summary>
    /// City-Name
    /// </summary>
    [MaxLength(100)]
    [GraphQLDescription("The city name for the geo coding address")]
    public string City { get; private set; } = string.Empty;

    /// <summary>
    /// Country-Name
    /// </summary>
    [MaxLength(100)]
    [GraphQLDescription("The country name for the geo coding address")]
    public string Country { get; private set; } = string.Empty;

    #endregion

    #region C'tor

    /// <summary>
    /// Constructor without parameters would be used by EF-Core
    /// </summary>
    private GoogleGeoCodingAddress()
    {
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="longitude">Longitude for GPS-Position</param>
    /// <param name="latitude">Latitude for GPS-Position</param>
    /// <param name="streetName">Street-Name</param>
    /// <param name="hnr">The house number</param>
    /// <param name="zip">The ZIP-Code</param>
    /// <param name="city">City-Name</param>
    /// <param name="country">Country-Name</param>
    public GoogleGeoCodingAddress(double longitude, double latitude, string streetName, string hnr, string zip,
        string city,
        string country)
    {
        if (!string.IsNullOrEmpty(hnr) && string.IsNullOrEmpty(streetName))
        {
            throw new DomainException("When HNR is provided a street name must be provided too");
        }

        if (string.IsNullOrEmpty(zip) && !string.IsNullOrEmpty(city))
        {
            throw new DomainException("When city is provided a PLZ code must be provided too");
        }

        if (!string.IsNullOrEmpty(zip) && string.IsNullOrEmpty(city))
        {
            throw new DomainException("When PLZ is provided a city must be provided too");
        }

        if (string.IsNullOrEmpty(country))
        {
            throw new DomainException("A country must be provided");
        }

        Longitude = longitude;
        Latitude = latitude;
        StreetName = streetName;
        Hnr = hnr;
        Zip = zip;
        City = city;
        Country = country;
    }

    #endregion

    #region Protected overrides

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Longitude;
        yield return Latitude;
        yield return StreetName;
        yield return Hnr;
        yield return Zip;
        yield return City;
        yield return Country;
    }

    #endregion
}