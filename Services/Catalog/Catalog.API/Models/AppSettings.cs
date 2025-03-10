namespace Catalog.API.Models;

/// <summary>
/// App-Settings - Class
/// </summary>
public class AppSettings
{
    #region RabbitMQ

    #region Endpoints

    /// <summary>
    /// Endpoint for Catalog-Service
    /// </summary>
    public string EndpointCatalogService { get; set; } = string.Empty;

    #endregion

    #endregion
}