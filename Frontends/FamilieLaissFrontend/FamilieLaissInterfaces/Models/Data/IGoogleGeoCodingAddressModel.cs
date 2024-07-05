namespace FamilieLaissInterfaces.Models.Data;

public interface IGoogleGeoCodingAddressModel
{
    /// <summary>
    /// Longitude for GPS-Position
    /// </summary>
    public double? Longitude { get; set; }

    /// <summary>
    /// Latitude for GPS-Position
    /// </summary>
    public double? Latitude { get; set; }

    /// <summary>
    /// Street-Name
    /// </summary>
    public string? StreetName { get; set; }

    /// <summary>
    /// The house number 
    /// </summary>
    public string? Hnr { get; set; }

    /// <summary>
    /// The ZIP-Code
    /// </summary>
    public string? Zip { get; set; }

    /// <summary>
    /// City-Name
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// Country-Name
    /// </summary>
    public string? Country { get; set; }
}
