using System.IO;

namespace User.API.Models
{
    /// <summary>
    /// App-Settings - Class
    /// </summary>
    public class AppSettings
    {
        #region RabbitMQ
        #region Endpoints
        /// <summary>
        /// Endpoint for User-Service
        /// </summary>
        public string EndpointUserService { get; set; }

        /// <summary>
        /// Endpoint for Settings-Service
        /// </summary>
        public string EndpointSettingsService { get; set; }
        #endregion
        #endregion
    }
}
