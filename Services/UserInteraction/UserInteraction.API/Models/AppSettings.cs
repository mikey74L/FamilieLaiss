namespace UserInteraction.API.Models;

/// <summary>
/// App-Settings - Class
/// </summary>
public class AppSettings
{
    #region RabbitMQ
    #region Endpoints
    /// <summary>
    /// Endpoint for User-Interaction-Service
    /// </summary>
    public string EndpointUserInteractionService { get; set; }
    #endregion
    #endregion
}