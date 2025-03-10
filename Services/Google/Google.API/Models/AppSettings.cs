namespace Google.API.Models;

public class AppSettings
{
    #region RabbitMQ
    #region Endpoints

    public string EndpointGoogleApiService { get; set; } = string.Empty;

    #endregion

    #endregion

    #region Google Geo-Coding
    public string BaseUrlGoogleGeoCodingApi { get; set; } = string.Empty;
    #endregion
}