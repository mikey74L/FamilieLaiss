using Google.API.Models;

namespace Google.API
{
    /// <summary>
    /// Startup-Class for ASP.NET Core
    /// </summary>
    public static class Startup
    {
        #region MassTransit EndpointConventions
        public static void ConfigureEndpointConventions(AppSettings? appSettings)
        {
            //Setzen der Sending-Endpoint-Mappings
            //EndpointConvention.Map<iConvertPictureCmd>(new Uri("queue:" + appSettings.Endpoint_PictureConverterServiceExecute));
        }
        #endregion
    }
}
