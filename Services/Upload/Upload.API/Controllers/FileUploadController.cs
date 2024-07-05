using Asp.Versioning;
using DomainHelper.Exceptions;
using InfrastructureHelper.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Upload.API.Commands;
using Upload.API.Mediator.Commands;
using Upload.DTO.FileUpload;

namespace Upload.API.Controllers;

/// <summary>
/// API-Controller for category operations
/// </summary>
/// <param name="logger">The logger for this controller</param>
/// <param name="mediator">The mediator to delegate requests to</param>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class FileUploadController(ILogger<FileUploadController> logger, IMediator mediator) : ControllerBase
{
    #region GetData
    /// <summary>
    /// Get next free upload id
    /// </summary>
    /// <returns>The next free upload id</returns>
    /// <response code="200">The next free upload id</response>
    /// <response code="401">Not authorized</response>
    /// <response code="500">Internal server error. Please contact support if so.</response>
    [HttpGet()]
    [Route("GetUploadId")]
    [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<long>> GetUploadId()
    {
        try
        {
            logger.LogInformation("GetUploadId called");

            var getUploadIdResult = await mediator.Send(new MtrCreateIDForUploadCmd());

            return Ok(getUploadIdResult);
        }
        catch
        {
            throw;
        }
    }
    #endregion

    #region Upload
    #region Picture
    /// <summary>
    /// Add an upload chunk for a picture on the server
    /// </summary>
    /// <param name="model">The model with data for the upload chunk</param>
    /// <returns>The status True = Successful / False = Error</returns>
    /// <response code="200">The status True = Successful / False = Error</response>
    /// <response code="401">Not authorized</response>
    /// <response code="500">Internal server error. Please contact support if so.</response>
    [HttpPost]
    [Route("AddUploadChunckPicture")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<bool>> AddUploadChunckPicture([FromBody] AddUploadChunckDto model)
    {
        try
        {
            logger.LogInformation("AddUploadChunckPicture called");

            var addUploadChunkResult = await mediator.Send(new MtrAddUploadChunckPictureCmd() { Data = model });

            return Ok(addUploadChunkResult);
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Finishes an upload for a picture after als chunks has been uploaded
    /// </summary>
    /// <param name="model">The model with data for the finish operation</param>
    /// <returns>The status True = Successful / False = Error</returns>
    /// <response code="200">The status True = Successful / False = Error</response>
    /// <response code="401">Not authorized</response>
    /// <response code="500">Internal server error. Please contact support if so.</response>
    [HttpPost]
    [Route("FinishUploadPicture")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<bool>> FinishUploadPicture([FromBody] FinishUploadDto model)
    {
        try
        {
            logger.LogInformation("FinishUploadPicture called");

            var finishUploadResult = await mediator.Send(new MtrFinishPictureUploadCmd() { Data = model });

            return Ok(finishUploadResult);
        }
        catch
        {
            throw;
        }
    }
    #endregion

    #region Video
    /// <summary>
    /// Add an upload chunk for a video on the server
    /// </summary>
    /// <param name="model">The model with data for the upload chunk</param>
    /// <returns>The status True = Successful / False = Error</returns>
    /// <response code="200">The status True = Successful / False = Error</response>
    /// <response code="401">Not authorized</response>
    /// <response code="500">Internal server error. Please contact support if so.</response>
    [HttpPost]
    [Route("AddUploadChunckVideo")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<bool>> AddUploadChunckVideo([FromBody] AddUploadChunckDto model)
    {
        try
        {
            logger.LogInformation("AddUploadChunckVideo called");

            var addUploadChunkResult = await mediator.Send(new MtrAddUploadChunckVideoCmd() { Data = model });

            return Ok(addUploadChunkResult);
        }
        catch (DomainException ex) when (ex.ExceptionType == DomainExceptionType.WrongParameter)
        {
            return BadRequest();
        }
        catch (DomainException ex) when (ex.ExceptionType == DomainExceptionType.NoDataFound)
        {
            return NotFound();
        }
        catch (DataDuplicatedValueException)
        {
            return Conflict();
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Finishes an upload for a video after als chunks has been uploaded
    /// </summary>
    /// <param name="model">The model with data for the finish operation</param>
    /// <returns>The status True = Successful / False = Error</returns>
    /// <response code="200">The status True = Successful / False = Error</response>
    /// <response code="401">Not authorized</response>
    /// <response code="500">Internal server error. Please contact support if so.</response>
    [HttpPost]
    [Route("FinishUploadVideo")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<bool>> FinishUploadVideo([FromBody] FinishUploadDto model)
    {
        try
        {
            logger.LogInformation("FinishUploadVideo called");

            var finishUploadResult = await mediator.Send(new MtrFinishVideoUploadCmd() { Data = model });

            return Ok(finishUploadResult);
        }
        catch
        {
            throw;
        }
    }
    #endregion
    #endregion
}
