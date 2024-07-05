using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Video.API.Models;

namespace Video.API.Controller
{
    /// <summary>
    /// API-Controller for category entries
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class VTTFileController : ControllerBase
    {
        #region Private Members
        private readonly AppSettings _AppSettings;
        #endregion

        #region C'tor
        public VTTFileController(IOptions<AppSettings> appSettings)
        {
            _AppSettings = appSettings.Value;
        }
        #endregion

        #region REST
        #region Get
        [HttpGet]
        public async Task<FileContentResult> GetVTTFileForVideo([FromQuery] string filenameVTT)
        {
            //Set filename for file
            var Filename = System.IO.Path.Combine(_AppSettings.RootFolder, "Video", filenameVTT);

            //Create URL for sprite image
            var UrlSprite = new Uri(_AppSettings.SPAGatewayURLSprite);

            //Read File from disk and write to stream
            using var Reader = new System.IO.StreamReader(Filename);
            using var TargetStream = new System.IO.MemoryStream();
            using var Writer = new System.IO.StreamWriter(TargetStream);
            while (!Reader.EndOfStream)
            {
                var CurrentLine = await Reader.ReadLineAsync();

                CurrentLine = string.Format(CurrentLine, UrlSprite);

                await Writer.WriteLineAsync(CurrentLine);
            }

            await Writer.FlushAsync();

            return new FileContentResult(TargetStream.ToArray(), "text/vtt");
        }
        #endregion
        #endregion
    }
}
