using DomainHelper.AbstractClasses;
using DomainHelper.Exceptions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Upload.Domain.ValueObjects;

/// <summary>
/// Value-Object for Google-Geo-Coding-Adresses
/// </summary>
public class GoogleGeoCodingAddress : ValueObject
{
    #region Public Properties
    /// <summary>
    /// Longitude for GPS-Position
    /// </summary>
    public double Longitude { get; private set; }

    /// <summary>
    /// Latitude for GPS-Position
    /// </summary>
    public double Latitude { get; private set; }

    /// <summary>
    /// Street-Name
    /// </summary>
    [MaxLength(100)]
    public string StreetName { get; private set; } = string.Empty;

    /// <summary>
    /// The house number 
    /// </summary>
    [MaxLength(10)]
    public string Hnr { get; private set; } = string.Empty;

    /// <summary>
    /// The ZIP-Code
    /// </summary>
    [MaxLength(10)]
    public string Plz { get; private set; } = string.Empty;

    /// <summary>
    /// City-Name
    /// </summary>
    [MaxLength(100)]
    public string City { get; private set; } = string.Empty;

    /// <summary>
    /// Country-Name
    /// </summary>
    [MaxLength(100)]
    public string Country { get; private set; } = string.Empty;
    #endregion

    #region C'tor
    /// <summary>
    /// C'tor without parameters would be used by EF-Core
    /// </summary>
    private GoogleGeoCodingAddress()
    {

    }

    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="longitude">Longitude for GPS-Position</param>
    /// <param name="latitude">Latitude for GPS-Position</param>
    /// <param name="streetName">Street-Name</param>
    /// <param name="hnr">The house number</param>
    /// <param name="plz">The ZIP-Code</param>
    /// <param name="city">City-Name</param>
    /// <param name="country">Country-Name</param>
    public GoogleGeoCodingAddress(double longitude, double latitude, string streetName, string hnr, string plz, string city,
        string country)
    {
        //Überprüfen der Werte
        if (!string.IsNullOrEmpty(hnr) && string.IsNullOrEmpty(streetName))
        {
            throw new DomainException("When HNR is provided a street name must be provided too");
        }

        if (string.IsNullOrEmpty(plz) && !string.IsNullOrEmpty(city))
        {
            throw new DomainException("When city is provided a PLZ code must be provided too");
        }

        if (!string.IsNullOrEmpty(plz) && string.IsNullOrEmpty(city))
        {
            throw new DomainException("When PLZ is provided a city must be provided too");
        }

        if (string.IsNullOrEmpty(country))
        {
            throw new DomainException("A country must be provided");
        }

        //Übernehmen der Werte
        Longitude = longitude;
        Latitude = latitude;
        StreetName = streetName;
        Hnr = hnr;
        Plz = plz;
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
        yield return Plz;
        yield return City;
        yield return Country;
    }
    #endregion
}
