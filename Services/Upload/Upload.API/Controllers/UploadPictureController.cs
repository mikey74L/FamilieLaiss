using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Upload.API.Mediator.Queries;
using Upload.DTO.UploadPicture;

namespace Upload.API.Controllers;

/// <summary>
/// API-Controller for upload picture operations
/// </summary>
/// <param name="logger">The logger for this controller</param>
/// <param name="mediator">The mediator to delegate requests to</param>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class UploadPictureController(ILogger<UploadPictureController> logger, IMediator mediator) : ControllerBase
{
    #region Get Data

    /// <summary>
    /// Get all upload pictures
    /// </summary>
    /// <returns>A list of all upload pictures</returns>
    /// <response code="200">Returns list of all upload pictures</response>
    /// <response code="401">Not authorized</response>
    /// <response code="500">Internal server error. Please contact support if so.</response>
    [HttpGet()]
    [Route("GetAllUploadPictures")]
    [ProducesResponseType(typeof(IEnumerable<UploadPictureDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<UploadPictureDto>>> GetAllUploadPictures()
    {
        logger.LogInformation("GetAllUploadPictures called");

        var allUploadPicturesResult = await mediator.Send(new GetAllUploadPicturesQuery());

        return Ok(allUploadPicturesResult);
    }

    /// <summary>
    /// Get unassigned upload pictures
    /// </summary>
    /// <returns>A list of unassigned upload pictures</returns>
    /// <response code="200">Returns list of unassigned upload pictures</response>
    /// <response code="401">Not authorized</response>
    /// <response code="500">Internal server error. Please contact support if so.</response>
    [HttpGet()]
    [Route("GetUnassignedUploadPictures")]
    [ProducesResponseType(typeof(IEnumerable<UploadPictureDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<UploadPictureDto>>> GetUnassignedUploadPictures()
    {
        logger.LogInformation("GetUnassignedUploadPictures called");

        var unassignedUploadPicturesResult = await mediator.Send(new GetUnassignedUploadPicturesQuery());

        return Ok(unassignedUploadPicturesResult);
    }

    #endregion
}