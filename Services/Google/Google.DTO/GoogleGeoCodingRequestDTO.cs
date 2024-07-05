namespace Google.DTO;

/// <summary>
/// DTO-Class for requesting a geo coding adress from google
/// </summary>
public class GoogleGeoCodingRequestDTO
{
    /// <summary>
    /// GPS-Longitude value
    /// </summary>
    public double Longitude { get; set; }

    /// <summary>
    /// GPS-Latitude value
    /// </summary>
    public double Latitude { get; set; }
}
