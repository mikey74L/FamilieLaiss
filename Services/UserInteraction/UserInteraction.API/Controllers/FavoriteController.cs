using DomainHelper.Exceptions;
using FamilieLaissCoreHelpers.Extensions;
using FamilieLaissSharedObjects.Classes;
using InfrastructureHelper.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using ServiceLayerHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserInteraction.API.Commands;
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
    public class FavoriteController: ControllerBase
    {
        #region Private Members
        private readonly IMediator _Mediator;
        private readonly ILogger<FavoriteController> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="mediator">Mediatr Object. Will be injected by DI-Container</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public FavoriteController(IMediator mediator, ILogger<FavoriteController> logger)
        {
            //Übernehmen der injected Objects
            _Mediator = mediator;
            _Logger = logger;
        }
        #endregion

        #region REST API
        #region Get
        /// <summary>
        /// Get all favorites
        /// </summary>
        /// <param name="queryParams">Additional query params like additional where clause to get data</param>
        /// <returns>Get all favorites</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FavoriteDTOModel>))]
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
                var Query = new QueryFavorite(queryParams);

                //Ausführen der Query
                _Logger.LogDebug("Execute query with mediator");
                var Result = await _Mediator.Send<IEnumerable<FavoriteDTOModel>>(Query);

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
        /// Get favorite
        /// </summary>
        /// <param name="id">Identifier for favorite</param>
        /// <param name="queryParams">Additional query params like additional where clause to get data</param>
        /// <returns>Get a single favorite by identifier</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FavoriteDTOModel))]
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
                var Query = new QueryFavorite(queryParams, id);

                //Ausführen der Query
                _Logger.LogDebug("Execute query with mediator");
                var Result = await _Mediator.Send<IEnumerable<FavoriteDTOModel>>(Query);

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

        /// <summary>
        /// Get all favorites for user
        /// </summary>
        /// <param name="queryParams">Additional query params like additional where clause to get data</param>
        /// <returns>Get all favorites for the current user</returns>
        [HttpGet("GetFavoritesForUser")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FavoriteDTOModel>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetFavoritesForUser([FromQuery] QueryParams queryParams)
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
                var Query = new QueryFavorite(queryParams, null, null, HttpContext.User.Identity.GetUserName());

                //Ausführen der Query
                _Logger.LogDebug("Execute query with mediator");
                var Result = await _Mediator.Send<IEnumerable<FavoriteDTOModel>>(Query);

                //Result zurückliefern
                _Logger.LogDebug("Return result");
                return Ok(Result);
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

        /// <summary>
        /// Get count of favorites
        /// </summary>
        /// <returns>Action-Result</returns>
        [HttpGet("GetCountFavorites")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetCountFavorites()
        {
            //Logging ausgeben
            _Logger.LogInformation("Action called");

            try
            {
                //Zusammenstellen der Query
                _Logger.LogDebug("Creating query");
                var Query = new QueryCountFavorite(HttpContext.User.Identity.GetUserName());

                //Ausführen der Query
                _Logger.LogDebug("Execute query with mediator");
                var Result = await _Mediator.Send<long>(Query);

                //Result zurückliefern
                _Logger.LogDebug("Return result");
                return Ok(Result);
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
        /// Add new favorite
        /// </summary>
        /// <param name="model">Favorite data</param>
        /// <remarks>Adding a new favorite to the store.</remarks>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(FavoriteDTOModel))]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Post([FromBody, BindRequired] FavoriteCreateDTOModel model)
        {
            //Logging ausgeben
            _Logger.LogInformation("Action called with following Parameters:");
            _Logger.LogDebug($"UserInteractionInfoID: {model.UserInteractionInfoID}");

            if (ModelState.IsValid)
            {
                try
                {
                    //Zusammenbauen des Commands
                    _Logger.LogDebug("Creating command");
                    var Command = new MtrCreateFavoriteCmd(model.UserInteractionInfoID, HttpContext.User.Identity.GetUserName());

                    //Senden des Commands für Mediatr
                    _Logger.LogDebug("Send command with mediatr");
                    var NewModel = await _Mediator.Send<FavoriteDTOModel>(Command);

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

        #region Delete
        /// <summary>
        /// Delete favorite
        /// </summary>
        /// <param name="id">Identifier for favorite</param>
        /// <param name="model">The delete model</param>
        /// <remarks>Deleting an existing favorite from the store.</remarks>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(long id, [FromBody, BindRequired] FavoriteDeleteDTOModel model)
        {
            //Logging ausgeben
            _Logger.LogInformation($"Action called with ID = {id} and following parameters:");
            _Logger.LogDebug($"UserInteractionInfoID: {model.UserInteractionInfoID}");

            try
            {
                //Zusammenbauen des Commands
                _Logger.LogDebug("Creating command");
                var Command = new MtrDeleteFavoriteCmd(id, model.UserInteractionInfoID);

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
        #endregion
    }
}
