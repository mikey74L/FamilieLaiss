namespace Google.API.Models;

public class AppSettings
{
    #region RabbitMQ

    /// <summary>
    /// RabbitMQ Connection-String - Filename 
    /// </summary>
    public string RabbitMqConnectionFile { get; set; } = string.Empty;

    /// <summary>
    /// RabbitMQ Connection-String for CloudAMP
    /// </summary>
    public string RabbitMqConnection => System.IO.File.ReadAllText(RabbitMqConnectionFile);


    #region Endpoints

    public string EndpointGoogleApiService { get; set; } = string.Empty;

    #endregion

    #endregion

    #region Google Geo-Coding

    public string BaseUrlGoogleGeoCodingApi { get; set; } = string.Empty;

    public string GoogleApiKeyFile { get; set; } = string.Empty;

    public string GoogleApiKey => System.IO.File.ReadAllText(GoogleApiKeyFile);

    #endregion
}