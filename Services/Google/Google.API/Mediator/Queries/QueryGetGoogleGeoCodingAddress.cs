using Google.API.Interfaces;
using Google.DTO;
using MediatR;

namespace Google.API.Mediator.Queries;

/// <summary>
/// Query for get Google-Geo-Coding-Data
/// </summary>
public class QueryGetGoogleGeoCodingAddress : IRequest<GoogleGeoCodingAdressDTO>
{
    /// <summary>
    /// Request-Data
    /// </summary>
    public required GoogleGeoCodingRequestDTO Model { get; init; }
}

/// <summary>
/// Mediatr-Query-Handler for get ConvertStatus 
/// </summary>
public class QueryHandlerGetGoogleGeoCodingAddress(
    IWsGoogleGeoCoding webService,
    ILogger<QueryHandlerGetGoogleGeoCodingAddress> logger)
    : IRequestHandler<QueryGetGoogleGeoCodingAddress, GoogleGeoCodingAdressDTO>
{
    #region Query-Handler

    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task</returns>
    public async Task<GoogleGeoCodingAdressDTO> Handle(QueryGetGoogleGeoCodingAddress request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Query-Handler for convert status was called with following parameters:");

        logger.LogDebug("Get data from Google Geo Coding API");
        var wsResult = await webService.GetResultFromGoogle(request.Model.Longitude, request.Model.Latitude);

        logger.LogDebug("Create and fill receive object");
        var result = new GoogleGeoCodingAdressDTO()
        {
            Longitude = wsResult?.Longitude ?? request.Model.Longitude,
            Latitude = wsResult?.Latitude ?? request.Model.Latitude,
            StreetName = wsResult?.StreetName ?? string.Empty,
            HNR = wsResult?.Hnr ?? string.Empty,
            ZIP = wsResult?.Zip ?? string.Empty,
            City = wsResult?.City ?? string.Empty,
            Country = wsResult?.Country ?? string.Empty
        };

        logger.LogDebug("Return result");
        return result;
    }

    #endregion
}