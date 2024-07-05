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
    public class CommentController: ControllerBase
    {
        #region Private Members
        private readonly IMediator _Mediator;
        private readonly ILogger<CommentController> _Logger;
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="mediator">Mediatr Object. Will be injected by DI-Container</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public CommentController(IMediator mediator, ILogger<CommentController> logger)
        {
            //Übernehmen der injected Objects
            _Mediator = mediator;
            _Logger = logger;
        }
        #endregion

        #region REST API
        #region Get
        /// <summary>
        /// Get all comments
        /// </summary>
        /// <param name="queryParams">Additional query params like additional where clause to get data</param>
        /// <returns>Get all comments</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CommentDTOModel>))]
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
                var Query = new QueryComment(queryParams);

                //Ausführen der Query
                _Logger.LogDebug("Execute query with mediator");
                var Result = await _Mediator.Send<IEnumerable<CommentDTOModel>>(Query);

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
        /// Get comment
        /// </summary>
        /// <param name="id">Identifier for comment</param>
        /// <param name="queryParams">Additional query params like additional where clause to get data</param>
        /// <returns>Get a single comment by identifier</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommentDTOModel))]
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
                var Query = new QueryComment(queryParams, id);

                //Ausführen der Query
                _Logger.LogDebug("Execute query with mediator");
                var Result = await _Mediator.Send<IEnumerable<CommentDTOModel>>(Query);

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
        /// Get all comments for user
        /// </summary>
        /// <param name="queryParams">Additional query params like additional where clause to get data</param>
        /// <returns>Get all comments for the current user</returns>
        [HttpGet("GetCommentsForUser")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CommentDTOModel>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetCommentsForUser([FromQuery] QueryParams queryParams)
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
                var Query = new QueryComment(queryParams, null, null, HttpContext.User.Identity.GetUserName());

                //Ausführen der Query
                _Logger.LogDebug("Execute query with mediator");
                var Result = await _Mediator.Send<IEnumerable<CommentDTOModel>>(Query);

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
        /// Get count of comments
        /// </summary>
        /// <returns>Action-Result</returns>
        [HttpGet("GetCountComments")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetCountComments()
        {
            //Logging ausgeben
            _Logger.LogInformation("Action called");

            try
            {
                //Zusammenstellen der Query
                _Logger.LogDebug("Creating query");
                var Query = new QueryCountComment(HttpContext.User.Identity.GetUserName());

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
        /// Add new comment
        /// </summary>
        /// <param name="model">Comment data</param>
        /// <remarks>Adding a new comment to the store.</remarks>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CommentDTOModel))]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Post([FromBody, BindRequired] CommentCreateDTOModel model)
        {
            //Logging ausgeben
            _Logger.LogInformation("Action called with following Parameter:");
            _Logger.LogDebug($"UserInteractionInfoID: {model.UserInteractionInfoID}");
            _Logger.LogDebug($"Content              : {model.Content}");

            if (ModelState.IsValid)
            {
                try
                {
                    //Zusammenbauen des Commands
                    _Logger.LogDebug("Creating command");
                    var Command = new MtrCreateCommentCmd(model.UserInteractionInfoID, HttpContext.User.Identity.GetUserName(), model.Content);

                    //Senden des Commands für Mediatr
                    _Logger.LogDebug("Send command with mediatr");
                    var NewModel = await _Mediator.Send<CommentDTOModel>(Command);

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
        #endregion
    }
}
