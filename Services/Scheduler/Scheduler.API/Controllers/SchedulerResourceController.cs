using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Scheduler.API.Queries.SchedulerResource;
using DomainHelper.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scheduler.DTO;
using ServiceLayerHelper;
using InfrastructureHelper.Exceptions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Scheduler.API.Commands.SchedulerResource;
using Scheduler.DTO.SchedulerResource;
using Microsoft.Extensions.Logging;
using FamilieLaissSharedObjects.Classes;

namespace Scheduler.API.Controllers
{
    /// <summary>
    /// API-Controller for scheduler resources
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Authorize("SchedulerPolicy")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class SchedulerResourceController : ControllerBase
    {
        #region Private Members
        private readonly IMediator _Mediator;
        private readonly ILogger<SchedulerResourceController> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="mediator">Mediatr Object. Will be injected by DI-Container</param>
        /// <param name="logger">Logger. Injkected by DI</param>
        public SchedulerResourceController(IMediator mediator, ILogger<SchedulerResourceController> logger)
        {
            //Übernehmen der injected Objects
            _Mediator = mediator;
            _Logger = logger;
        }
        #endregion

        #region REST API
        #region Get
        /// <summary>
        /// Get all scheduler resources
        /// </summary>
        /// <param name="queryParams">Additional query params like additional where clause to get data</param>
        /// <returns>Get all scheduler resources</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SchedulerResourceDTOModel>))]
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
                var Query = new QuerySchedulerResource(queryParams);

                //Ausführen der Query
                _Logger.LogDebug("Execute query with mediator");
                var Result = await _Mediator.Send<IEnumerable<SchedulerResourceDTOModel>>(Query);

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
        /// Get scheduler resource
        /// </summary>
        /// <param name="queryParams">Additional query params like additional where clause to get data</param>
        /// <returns>Get a single scheduler resource by identifier</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SchedulerResourceDTOModel))]
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
                var Query = new QuerySchedulerResource(queryParams, id);

                //Ausführen der Query
                _Logger.LogDebug("Execute query with mediator");
                var Result = await _Mediator.Send<IEnumerable<SchedulerResourceDTOModel>>(Query);

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
    
        #region Add - Post
        /// <summary>
        /// Add new scheduler resource
        /// </summary>
        /// <param name="model">Scheduler resource data</param>
        /// <remarks>Adding a new scheduler resource to the store.</remarks>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SchedulerResourceDTOModel))]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Post([FromBody, BindRequired] SchedulerResourceCreateDTOModel model)
        {
            //Logging ausgeben
            _Logger.LogInformation("Action called with following Parameter:");
            _Logger.LogDebug($"Name         : {model.Name}");
            _Logger.LogDebug($"Color        : {model.Color}");
            _Logger.LogDebug($"Starting-Time: {model.StartingTime}");
            _Logger.LogDebug($"Ending-Time  : {model.EndingTime}");

            if (ModelState.IsValid)
            {
                try
                {
                    //Zusammenbauen des Commands
                    _Logger.LogDebug("Creating command");
                    var Command = new MtrMakeNewSchedulerResourceCmd(model.Name, model.Color, model.StartingTime, model.EndingTime);

                    //Senden des Commands für Mediatr
                    _Logger.LogDebug("Send command with mediatr");
                    var NewModel = await _Mediator.Send<SchedulerResourceDTOModel>(Command);

                    //OK zurückmelden
                    _Logger.LogDebug("Return OK with route");
                    return CreatedAtRoute(new { id = NewModel.ID }, NewModel);
                }
                catch (DataDuplicatedValueException)
                {
                    _Logger.LogError("Duplicated value");
                    return StatusCode(StatusCodes.Status409Conflict);
                }
                catch (Exception ex)
                {
                    _Logger.LogCritical(ex, "Unexpected error occurred");
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            else
            {
                _Logger.LogError("Model state not valid");
                return BadRequest(ModelState);
            }
        }
        #endregion

        #region Update - Put
        /// <summary>
        /// Update existing scheduler resource
        /// </summary>
        /// <param name="id">Primary key for scheduler resource to update</param>
        /// <param name="model">Scheduler resource data to update</param>
        /// <remarks>Updating an existing scheduler resource in the store.</remarks>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Put(long id, [FromBody, BindRequired] SchedulerResourceUpdateDTOModel model)
        {
            //Logging ausgeben
            _Logger.LogInformation($"Action called for id = {id} with following parameters:");
            _Logger.LogDebug($"Name         : {model.Name}");
            _Logger.LogDebug($"Color        : {model.Color}");
            _Logger.LogDebug($"Starting-Time: {model.StartingTime}");
            _Logger.LogDebug($"Ending-Time  : {model.EndingTime}");

            if (ModelState.IsValid)
            {
                try
                {
                    //Zusammenbauen des Commands
                    _Logger.LogDebug("Creating command");
                    var Command = new MtrUpdateSchedulerResourceCmd(id, model.Name, model.Color, model.StartingTime, model.EndingTime);

                    //Senden des Commands für Mediatr
                    _Logger.LogDebug("Send command with mediatr");
                    await _Mediator.Send(Command);

                    //OK zurückmelden
                    _Logger.LogDebug("Return OK");
                    return Ok();
                }
                catch (DomainNotFoundException)
                {
                    _Logger.LogError("No data found");
                    return NotFound();
                }
                catch (DataDuplicatedValueException)
                {
                    _Logger.LogError("Duplicated value");
                    return StatusCode(StatusCodes.Status409Conflict);
                }
                catch (Exception ex)
                {
                    _Logger.LogCritical(ex, "Unexpected error occurred");
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            else
            {
                _Logger.LogError("Model state not valid");
                return BadRequest(ModelState);
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete existing scheduler resource
        /// </summary>
        /// <param name="ID">Identifier for scheduler resource</param>
        /// <remarks>Deleting an existing scheduler resource from the store.</remarks>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(long id)
        {
            //Logging ausgeben
            _Logger.LogInformation($"Action called for id = {id}");

            try
            {
                //Zusammenbauen des Commands
                _Logger.LogDebug("Creating command");
                var Command = new MtrDeleteSchedulerResourceCmd(id);

                //Senden des Commands für Mediatr
                _Logger.LogDebug("Send command with mediatr");
                await _Mediator.Send(Command);

                //OK zurückmelden
                _Logger.LogDebug("Return OK with route");
                return Ok();
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

        #region Validation for Scheduler Resource
        /// <summary>
        /// Check if the name for a scheduler resource already exists
        /// </summary>
        /// <param name="value">Model with data to check</param>
        /// <remarks>Check if the name for a scheduler resource already exists in the store</remarks>
        [HttpPost("CheckSchedulerResourceName")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CheckValueResultDTOModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CheckSchedulerResourceName([FromBody, BindRequired]CheckValueDTOModel value)
        {
            //Logging ausgeben
            _Logger.LogInformation("Action called with following Query-Params:");
            _Logger.LogDebug($"ID             : {value.ID}");
            _Logger.LogDebug($"Value          : {value.Value}");
            _Logger.LogDebug($"Additional-Type: {value.AdditionalType}");

            try
            {
                //Zusammenstellen der Query
                _Logger.LogDebug("Creating query");
                var Query = new QuerySchedulerResourceCheckName(value.ID, value.AdditionalType, value.Value);

                //Ausführen der Query
                _Logger.LogDebug("Execute query with mediator");
                var MediatrResult = await _Mediator.Send<bool>(Query);

                //Result zurückliefern
                var Result = new CheckValueResultDTOModel() { Result = MediatrResult };
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
        #endregion
    }
}