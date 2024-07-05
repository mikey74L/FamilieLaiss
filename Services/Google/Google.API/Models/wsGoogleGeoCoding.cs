namespace Google.API.Models;

/// <summary>
/// Google-Geo-Coding
/// </summary>
public class GoogleGeoCodingAddress
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
    public string Zip { get; set; } = string.Empty;

    /// <summary>
    /// City-Name
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// Country-Name
    /// </summary>
    public string Country { get; set; } = string.Empty;
}

/// <summary>
/// Google-Geo-Coding Address Component (WS-Result)
/// </summary>
internal class WsRsGoogleGeoCodingAddressComponent
{
    /// <summary>
    /// Long name of the address component
    /// </summary>
    public string long_name { get; set; } = string.Empty;

    /// <summary>
    /// Short name of the adress component
    /// </summary>
    public string short_name { get; set; } = string.Empty;

    /// <summary>
    /// List of types to what this adress component belongs to
    /// </summary>
    public List<string>? types { get; set; }
}

/// <summary>
/// Google-Geo-Coding Location-Information (WS-Result)
/// </summary>
internal class WsRsGoogleGeoCodingLocation
{
    /// <summary>
    /// Latitude for the location
    /// </summary>
    public double lat { get; set; }

    /// <summary>
    /// longitude for the location
    /// </summary>
    public double lng { get; set; }
}

/// <summary>
/// Google-Geo-Coding Northeast (WS-Result)
/// </summary>
internal class WsRsGoogleGeoCodingNortheast
{
    /// <summary>
    /// Latitude value
    /// </summary>
    public double lat { get; set; }

    /// <summary>
    /// Longitude value
    /// </summary>
    public double lng { get; set; }
}

/// <summary>
/// Google-Geo-Coding Southwest (WS-Result)
/// </summary>
internal class WsRsGoogleGeoCodingSouthwest
{
    /// <summary>
    /// Latitude value
    /// </summary>
    public double lat { get; set; }

    /// <summary>
    /// Longitude value
    /// </summary>
    public double lng { get; set; }
}

/// <summary>
/// Google-Geo-Coding Viewport (WS-Result)
/// </summary>
internal class WsRsGoogleGeoCodingViewport
{
    /// <summary>
    /// Not used
    /// </summary>
    public WsRsGoogleGeoCodingNortheast? northeast { get; set; }

    /// <summary>
    /// Not used
    /// </summary>
    public WsRsGoogleGeoCodingSouthwest? southwest { get; set; }
}

/// <summary>
/// Google-Geo-Coding Northeast2 (WS-Result)
/// </summary>
internal class WsRsGoogleGeoCodingNortheast2
{
    /// <summary>
    /// Latitude value
    /// </summary>
    public double lat { get; set; }

    /// <summary>
    /// Longitude value
    /// </summary>
    public double lng { get; set; }
}

/// <summary>
/// Google-Geo-Coding Southwest2 (WS-Result)
/// </summary>
internal class WsRsGoogleGeoCodingSouthwest2
{
    /// <summary>
    /// Latitude value
    /// </summary>
    public double lat { get; set; }

    /// <summary>
    /// Longitude value
    /// </summary>
    public double lng { get; set; }
}

/// <summary>
/// Google-Geo-Coding Bounds (WS-Result)
/// </summary>
internal class WsRsGoogleGeoCodingBounds
{
    /// <summary>
    /// Not used
    /// </summary>
    public WsRsGoogleGeoCodingNortheast2? northeast { get; set; }

    /// <summary>
    /// Not used
    /// </summary>
    public WsRsGoogleGeoCodingSouthwest2? southwest { get; set; }
}

/// <summary>
/// Google-Geo-Coding Geometry (WS-Result)
/// </summary>
internal class WsRsGoogleGeoCodingGeometry
{
    /// <summary>
    /// Not used
    /// </summary>
    public WsRsGoogleGeoCodingLocation? location { get; set; }

    /// <summary>
    /// Not used
    /// </summary>
    public string location_type { get; set; } = string.Empty;

    /// <summary>
    /// Not used
    /// </summary>
    public WsRsGoogleGeoCodingViewport? viewport { get; set; }

    /// <summary>
    /// Not used
    /// </summary>
    public WsRsGoogleGeoCodingBounds? bounds { get; set; }
}

/// <summary>
/// Google-Geo-Coding Result (WS-Result)
/// </summary>
internal class WsRsGoogleGeoCodingResult
{
    /// <summary>
    /// List of address components
    /// </summary>
    public List<WsRsGoogleGeoCodingAddressComponent>? address_components { get; set; }

    /// <summary>
    /// The formatted address from the address components in a human-readable format
    /// </summary>
    public string formatted_address { get; set; } = string.Empty;

    /// <summary>
    /// Not Used
    /// </summary>
    public WsRsGoogleGeoCodingGeometry? geometry { get; set; }

    /// <summary>
    /// Not used
    /// </summary>
    public string place_id { get; set; } = string.Empty;

    /// <summary>
    /// Not Used
    /// </summary>
    public List<string>? types { get; set; }
}

/// <summary>
/// Google-Geo-Coding Result (WS-Result)
/// </summary>
internal class WsRsGoogleGeoCoding
{
    /// <summary>
    /// List of Results
    /// </summary>
    public List<WsRsGoogleGeoCodingResult>? results { get; set; }

    /// <summary>
    /// Status for the result. When successfully the result is "OK"
    /// </summary>
    public string status { get; set; } = string.Empty;
}