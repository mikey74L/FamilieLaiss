using DomainHelper.Interfaces;
using FamilieLaissMassTransitDefinitions.Contracts.Commands.UploadVideo;
using MediatR;

namespace Upload.API.Mediator.Commands.UploadVideo;

/// <summary>
/// Mediatr Command for set video info
/// </summary>
public class MtrSetVideoInfoCmd : IRequest
{
    /// <summary>
    /// Message data
    /// </summary>
    public required IMassSetVideoInfoDataCmd Message { get; init; }
}

/// <summary>
/// Mediatr Command-Handler for set video info
/// </summary>
/// <remarks>
/// Primary constructor 
/// </remarks>
/// <param name="unitOfWork">UnitOfWork. Injected by DI</param>
/// <param name="logger">Logger. Injected by DI</param>
public class MtrSetVideoInfoCmdHandler(
    iUnitOfWork unitOfWork,
    ILogger<MtrSetVideoInfoCmdHandler> logger)
    : IRequestHandler<MtrSetVideoInfoCmd>
{
    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task</returns>
    public async Task Handle(MtrSetVideoInfoCmd request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for set video info command was called for {@Message}",
            request.Message);

        logger.LogInformation("Get repository from unit of work");
        var repo = unitOfWork.GetRepository<Domain.Entities.UploadVideo>();

        logger.LogInformation($"Get entity from repository for ID = {request.Message.Id}");
        var entity = await repo.GetOneAsync(request.Message.Id);

        logger.LogInformation($"Set video information");
        entity.UpdateVideoInfo(request.Message.VideoType, request.Message.Height, request.Message.Width,
            request.Message.Hours,
            request.Message.Minutes, request.Message.Seconds, request.Message.Longitude, request.Message.Latitude);

        logger.LogInformation("Save changes");
        await unitOfWork.SaveChangesAsync();
    }
}