using Asp.Versioning;
using Google.API.Mediator.Queries;
using Google.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Google.API.Controllers;

/// <summary>
/// API-Controller for geo coding operations
/// </summary>
/// <param name="logger">The logger for this controller</param>
/// <param name="mediator">The mediator to delegate requests to</param>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class GeoCodingController(ILogger<GeoCodingController> logger, IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Get geo coding address from Google API
    /// </summary>
    /// <returns>Geo-Coding address</returns>
    /// <response code="200">Geo-Coding address</response>
    /// <response code="401">Not authorized</response>
    /// <response code="500">Internal server error. Please contact support if so.</response>
    [HttpPost()]
    [Route("GetGeoCodingAddress")]
    [ProducesResponseType(typeof(IEnumerable<GoogleGeoCodingAdressDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GoogleGeoCodingAdressDTO>> GetGeoCodingAddress(
        [FromBody] GoogleGeoCodingRequestDTO model)
    {
        logger.LogInformation("GetGeoCodingAddress called");

        var geoCodingResult = await mediator.Send(new QueryGetGoogleGeoCodingAddress() { Model = model });

        return Ok(geoCodingResult);
    }
}