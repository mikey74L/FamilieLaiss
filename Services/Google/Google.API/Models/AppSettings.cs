namespace Google.API.Models;

public class AppSettings
{
    #region RabbitMQ

    /// <summary>
    /// RabbitMQ Connection-String - Filename 
    /// </summary>
    public string RabbitMQConnection_FILE { get; set; } = string.Empty;

    /// <summary>
    /// RabbitMQ Connection-String for CloudAMP
    /// </summary>
    public string RabbitMQConnection
    {
        get { return System.IO.File.ReadAllText(RabbitMQConnection_FILE); }
    }


    #region Endpoints

    public string EndpointGoogleApiService { get; set; }

    #endregion

    #endregion

    #region Google Geo-Coding

    public string BaseUrlGoogleGeoCodingApi { get; set; } = string.Empty;

    public string GoogleAPIKey_FILE { get; set; } = string.Empty;

    public string GoogleApiKey
    {
        get { return System.IO.File.ReadAllText(GoogleAPIKey_FILE); }
    }

    #endregion
}