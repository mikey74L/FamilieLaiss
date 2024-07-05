using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilieLaissSharedObjects.Classes;
using Mail.API.Models.DTO;
using Mail.API.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceLayerHelper;

namespace Mail.API.Controllers
{
    /// <summary>
    /// API-Controller for Mail
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Authorize("MailPolicy")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class MailController : ControllerBase
    {
        #region Private Members
        private readonly IMediator _Mediator;
        private readonly ILogger<MailController> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="mediator">Mediatr object. Will be injected by DI-Container</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public MailController(IMediator mediator, ILogger<MailController> logger)
        {
            //Übernehmen der injected Objects
            _Mediator = mediator;
            _Logger = logger;
        }
        #endregion
      
        #region REST API
        /// <summary>
        /// Get all sended mails
        /// </summary>
        /// <param name="queryParams">Additional query params like additional where clause to get data</param>
        /// <returns>Get all sended mails</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MailDTOModel>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Get([FromQuery] QueryParams queryParams)
        {
            //Logging ausgeben
            _Logger.LogInformation("Action called with following Query-Params:");
            _Logger.LogDebug($"Where-Clause: {queryParams.WhereClause}");
            _Logger.LogDebug($"Include-Nav : {queryParams.IncludeNav}");
            _Logger.LogDebug($"Order-By    : {queryParams.OrderBy}");
            _Logger.LogDebug($"Take        : {queryParams.Take}");
            _Logger.LogDebug($"Skip        : {queryParams.Skip}");

            try
            {
                //Zusammenstellen der Query
                _Logger.LogDebug("Creating query");
                var Query = new QueryMail(queryParams);

                //Ausführen der Query
                _Logger.LogDebug("Execute query with mediator");
                var Result = await _Mediator.Send<IEnumerable<MailDTOModel>>(Query);

                //Result zurückliefern
                _Logger.LogDebug("Return result");
                return Ok(Result);
            }
            catch (Exception ex)
            {
                _Logger.LogCritical(ex, "Unexpected error occurred");
                return StatusCode(500);
            }
        }
        #endregion
    }
}