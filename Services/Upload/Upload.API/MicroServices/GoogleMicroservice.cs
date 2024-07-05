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
public class GoogleMicroService : MicroserviceBase, IGoogleMicroService
{
    #region Private Members
    private readonly AppSettings appSettings;
    #endregion

    #region C'tor
    /// <summary>
    /// C'tor
    /// </summary>
    /// <param name="steeltoeDiscoClient">Steeltoe disco client. Injected by DI</param>
    /// <param name="appSettings">App-Settings. Injected by DI</param>
    public GoogleMicroService(IDiscoveryClient steeltoeDiscoClient, IOptions<AppSettings> appSettings) : base(steeltoeDiscoClient)
    {
        //Übernehmen der injected objects
        this.appSettings = appSettings.Value;
    }
    #endregion

    #region Interface IIdentityMicroService
    /// <inheritdoc />
    public async Task<GoogleGeoCodingAdressDTO?> GetGoogleGeoCodingAdressAsync(GoogleGeoCodingRequestDTO request)
    {
        //Abfrage ausführen
        try
        {
            var httpClient = await GetHTTPClient(appSettings.GoogleMicroserviceUrl);

            var result = await httpClient.PostAsJsonAsync($"api/{appSettings.GoogleMicroserviceVersion}/GeoCoding/GetGeoCodingAdress", request);

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
    #endregion
}
