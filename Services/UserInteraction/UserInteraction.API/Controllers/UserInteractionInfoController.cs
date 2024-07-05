using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainHelper.Exceptions;
using FamilieLaissSharedObjects.Classes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceLayerHelper;
using UserInteraction.API.Queries;
using UserInteraction.DTO;

namespace UserInteraction.API.Controllers
{
    /// <summary>
    /// API-Controller for blog entries
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Authorize("UserInteractionPolicy")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class UserInteractionInfoController : ControllerBase
    {
        #region Private Members
        private readonly IMediator _Mediator;
        private readonly ILogger<UserInteractionInfoController> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="mediator">Mediatr Object. Will be injected by DI-Container</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public UserInteractionInfoController(IMediator mediator, ILogger<UserInteractionInfoController> logger)
        {
            //Übernehmen der injected Objects
            _Mediator = mediator;
            _Logger = logger;
        }
        #endregion

        #region REST API
        #region Get
        /// <summary>
        /// Get all user interaction infos
        /// </summary>
        /// <param name="queryParams">Additional query params like additional where clause to get data</param>
        /// <returns>Get all user interaction infos</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserInteractionInfoDTOModel>))]
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
                var Query = new QueryUserInteractionInfo(queryParams);

                //Ausführen der Query
                _Logger.LogDebug("Execute query with mediator");
                var Result = await _Mediator.Send<IEnumerable<UserInteractionInfoDTOModel>>(Query);

                //Result zurückliefern
                _Logger.LogDebug("Return result");
                return Ok(Result);
            }
            catch (Exception ex)
            {
                _Logger.LogCritical(ex, "Unexpected error occurred");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get user interaction info
        /// </summary>
        /// <param name="queryParams">Additional query params like additional where clause to get data</param>
        /// <param name="id">Unique identifier for entity</param>
        /// <returns>Get a single user interaction info by identifier</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserInteractionInfoDTOModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetOne(long id, [FromQuery] QueryParams queryParams)
        {
            //Logging ausgeben
            _Logger.LogInformation($"Action called for id = {id} with following Query-Params: ");
            _Logger.LogDebug($"Where-Clause: {queryParams.WhereClause}");
            _Logger.LogDebug($"Include-Nav : {queryParams.IncludeNav}");
            _Logger.LogDebug($"Order-By    : {queryParams.OrderBy}");
            _Logger.LogDebug($"Take        : {queryParams.Take}");
            _Logger.LogDebug($"Skip        : {queryParams.Skip}");

            try
            {
                //Zusammenstellen der Query
                _Logger.LogDebug("Creating query");
                var Query = new QueryUserInteractionInfo(queryParams, id);

                //Ausführen der Query
                _Logger.LogDebug("Execute query with mediator");
                var Result = await _Mediator.Send<IEnumerable<UserInteractionInfoDTOModel>>(Query);

                //Result zurückliefern
                _Logger.LogDebug("Return result");
                return Ok(Result.First());
            }
            catch (NoDataFoundException)
            {
                _Logger.LogError("No data found");
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                _Logger.LogCritical(ex, "Unexpected error occurred");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion
        #endregion
    }
}