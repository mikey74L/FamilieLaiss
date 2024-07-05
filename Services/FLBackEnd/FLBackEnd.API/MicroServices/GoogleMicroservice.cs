using FLBackEnd.API.Interfaces;
using FlBackEnd.API.Models;
using Google.DTO;
using Microsoft.Extensions.Options;
using ServiceLayerHelper;
using Steeltoe.Discovery;

namespace FLBackEnd.API.MicroServices;

/// <summary>
/// Service for Google-Microservice operations
/// </summary>
/// <remarks>
/// Primary constructor
/// </remarks>
/// <param name="steeltoeDiscoClient">Steeltoe disco client. Injected by DI</param>
/// <param name="appSettings">App-Settings. Injected by DI</param>
public class GoogleMicroService(IDiscoveryClient steeltoeDiscoClient, IOptions<AppSettings> appSettings)
    : MicroserviceBase(steeltoeDiscoClient), IGoogleMicroService
{
    /// <inheritdoc />
    public async Task<GoogleGeoCodingAdressDTO?> GetGoogleGeoCodingAddressAsync(GoogleGeoCodingRequestDTO request)
    {
        try
        {
            var httpClient = await GetHttpClient(appSettings.Value.GoogleMicroserviceUrl);

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