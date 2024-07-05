using Google.API.Interfaces;
using Google.API.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Google.API.Services;

/// <summary>
/// Helper-Class for getting Google-Geo-Coding-Information from Google-API
/// </summary>
public class WsGoogleGeoCodingService(IOptions<AppSettings> appSettings) : IWsGoogleGeoCoding
{
    #region Private Methods

    private GoogleGeoCodingAddress? ParseJsonToEntity(string json, double longitude, double latitude)
    {
        GoogleGeoCodingAddress? returnValue;

        var parsedObject = JsonConvert.DeserializeObject<WsRsGoogleGeoCoding>(json);

        if (parsedObject is { results: not null, status: "OK" })
        {
            returnValue = new GoogleGeoCodingAddress();
            foreach (var result in parsedObject.results)
            {
                if (result.types is not null && result.types.Contains("street_address") &&
                    result.address_components is not null &&
                    result.geometry?.location_type.ToUpper() == "ROOFTOP")
                {
                    foreach (var addressComp in result.address_components)
                    {
                        if (addressComp.types is not null)
                        {
                            if (addressComp.types.Contains("street_number"))
                            {
                                returnValue.Hnr = addressComp.long_name;
                            }

                            if (addressComp.types.Contains("route"))
                            {
                                returnValue.StreetName = addressComp.long_name;
                            }

                            if (addressComp.types.Contains("locality"))
                            {
                                returnValue.City = addressComp.long_name;
                            }

                            if (addressComp.types.Contains("country"))
                            {
                                returnValue.Country = addressComp.long_name;
                            }

                            if (addressComp.types.Contains("postal_code"))
                            {
                                returnValue.Zip = addressComp.long_name;
                            }
                        }
                    }
                }
            }

            returnValue.Longitude = longitude;
            returnValue.Latitude = latitude;
        }
        else
        {
            if (parsedObject is not null && parsedObject.status == "ZERO_RESULTS")
            {
                returnValue = new GoogleGeoCodingAddress
                {
                    City = "",
                    Country = "",
                    Hnr = "",
                    Zip = "",
                    StreetName = "",
                    Longitude = longitude,
                    Latitude = latitude
                };
            }
            else
            {
                returnValue = null;
            }
        }

        return returnValue;
    }

    #endregion

    #region Interface iWSGoogleGeoCoding

    /// <summary>
    /// Get Geo-Coding-Information from Google API
    /// </summary>
    /// <param name="longitude">The Longitude for Geo-Position</param>
    /// <param name="latitude">The Latitude for Geo-Position</param>
    /// <returns>The parsed and transformed Geo-Coding Information, or null if no Geo-Position is found</returns>
    public async Task<GoogleGeoCodingAddress?> GetResultFromGoogle(double longitude, double latitude)
    {
        GoogleGeoCodingAddress? returnValue;

        HttpClient client = new();

        var latitudeString = latitude.ToString();
        latitudeString = latitudeString.Replace(",", ".");
        var longitudeString = longitude.ToString();
        longitudeString = longitudeString.Replace(",", ".");
        var urlParams = $"/json?latlng={latitudeString},{longitudeString}&key={appSettings.Value.GoogleApiKey}";
        var response = await client.GetAsync(appSettings.Value.BaseUrlGoogleGeoCodingApi + urlParams);

        if (response.IsSuccessStatusCode)
        {
            var resultAsString = await response.Content.ReadAsStringAsync();
            returnValue = ParseJsonToEntity(resultAsString, longitude, latitude);
        }
        else
        {
            client.Dispose();
            throw new Exception("Error while reading from google api");
        }

        return returnValue;
    }

    #endregion
}