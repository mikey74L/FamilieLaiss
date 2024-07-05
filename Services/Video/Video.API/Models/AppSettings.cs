using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Video.API.Models
{
    /// <summary>
    /// App-Settings class
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// Absolute path to the root folder where the content files are located
        /// </summary>
        public string RootFolder { get; set; }

        /// <summary>
        /// URL for Sprite-Image over SPA-Gateway
        /// </summary>
        public string SPAGatewayURLSprite { get; set; }
    }
}
