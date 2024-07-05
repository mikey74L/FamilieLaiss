using Google.API.Models;

namespace Google.API.Interfaces;

/// <summary>
/// Interface for communication with Google Geo Coding web service
/// </summary>
public interface IWsGoogleGeoCoding
{
    /// <summary>
    /// Get Google-Geo-Coding-Address information from the Google web service
    /// </summary>
    /// <param name="longitude">The longitude for the GPS-Position</param>
    /// <param name="latitude">The Latitude for the GPS-Position</param>
    /// <returns>A Google-Geo-Coding-Address data with the exact address information. When no address is found an empty record is returned. When an error occurred then null will be returned.</returns>
    Task<GoogleGeoCodingAddress?> GetResultFromGoogle(double longitude, double latitude);
}