namespace Upload.DTO.ValueData;

/// <summary>
/// DTO-Class for Geo Coding Address
/// </summary>
public class GoogleGeoCodingAddressDto
{
    /// <summary>
    /// Longitude for GPS-Position
    /// </summary>
    public double Longitude { get; set; }

    /// <summary>
    /// Latitude for GPS-Position
    /// </summary>
    public double Latitude { get; set; }

    /// <summary>
    /// Street-Name
    /// </summary>
    public string StreetName { get; set; } = string.Empty;

    /// <summary>
    /// The house number 
    /// </summary>
    public string Hnr { get; set; } = string.Empty;

    /// <summary>
    /// The ZIP-Code
    /// </summary>
    public string Plz { get; set; } = string.Empty;

    /// <summary>
    /// City-Name
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// Country-Name
    /// </summary>
    public string Country { get; set; } = string.Empty;
}
