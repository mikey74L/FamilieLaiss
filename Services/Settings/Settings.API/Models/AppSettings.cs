namespace Settings.API.Models
{
    /// <summary>
    /// App-Settings - Class
    /// </summary>
    public class AppSettings
    {
        #region RabbitMQ
        #region Endpoints
        /// <summary>
        /// Endpoint for Settings-Service
        /// </summary>
        public string EndpointSettingsService { get; set; } = string.Empty;
        #endregion
        #endregion
    }
}
