namespace Google.DTO;

/// <summary>
/// DTO-Class for google geocoding result
/// </summary>
public class GoogleGeoCodingAdressDTO
{
    /// <summary>
    /// The GPS-Longitude value
    /// </summary>
    public double Longitude { get; set; }

    /// <summary>
    /// The GPS-Latitude value
    /// </summary>
    public double Latitude { get; set; }

    /// <summary>
    /// The street name
    /// </summary>
    public string StreetName { get; set; } = string.Empty;

    /// <summary>
    /// The house number
    /// </summary>
    public string HNR { get; set; } = string.Empty;

    /// <summary>
    /// The ZIP code
    /// </summary>
    public string ZIP { get; set; } = string.Empty;

    /// <summary>
    /// The name of the city
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// The name of the country
    /// </summary>
    public string Country { get; set; } = string.Empty;
}
