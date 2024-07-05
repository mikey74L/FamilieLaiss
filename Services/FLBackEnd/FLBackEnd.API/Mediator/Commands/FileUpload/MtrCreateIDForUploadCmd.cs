using FLBackEnd.API.Interfaces;
using MediatR;

namespace FLBackEnd.API.Mediator.Commands.FileUpload;

/// <summary>
/// Mediatr Command for Creating a unique UploadID
/// </summary>
public class MtrCreateIdForUploadCmd : IRequest<long>
{
}

/// <summary>
/// Mediatr Command-Handler for Creating a unique UploadID
/// </summary>
public class MtrCreateIdForUploadCmdHandler(
    ILogger<MtrCreateIdForUploadCmdHandler> logger,
    IUniqueIdentifierGenerator uniqueIdGenerator) : IRequestHandler<MtrCreateIdForUploadCmd, long>
{
    #region Mediatr-Handler

    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task</returns>
    public async Task<long> Handle(MtrCreateIdForUploadCmd request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for create unique upload ID command was called");

        return await uniqueIdGenerator.GetNextUploadIdAsync();
    }

    #endregion
}