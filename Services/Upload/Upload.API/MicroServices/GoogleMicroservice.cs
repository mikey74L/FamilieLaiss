using Google.DTO;
using Microsoft.Extensions.Options;
using ServiceLayerHelper;
using Steeltoe.Discovery;
using Upload.API.Interfaces;
using Upload.API.Models;

namespace Upload.API.MicroServices;

/// <summary>
/// Service for Google-Microservice operations
/// </summary>
/// <remarks>
/// Primary constructor
/// </remarks>
/// <param name="httpClient">Typed HTTp-Client from factory</param>
/// <param name="appSettings">App-Settings. Injected by DI</param>
public class GoogleMicroService(HttpClient httpClient, IOptions<AppSettings> appSettings)
    : IGoogleMicroService
{
    /// <inheritdoc />
    public async Task<GoogleGeoCodingAdressDTO?> GetGoogleGeoCodingAddressAsync(GoogleGeoCodingRequestDTO request)
    {
        try
        {
            var result =
                await httpClient.PostAsJsonAsync(
                    $"api/{appSettings.Value.GoogleMicroserviceVersion}/GeoCoding/GetGeoCodingAddress", request);

            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<GoogleGeoCodingAdressDTO?>();
            }
            else
            {
                return null;
            }
        }
        catch
        {
            return null;
        }
    }
}