using DomainHelper.Interfaces;
using FamilieLaissMassTransitDefinitions.Contracts.Events.UploadVideo;
using MediatR;

namespace Catalog.API.Mediator.Commands.UploadVideo;

/// <summary>
/// Mediatr Command for add upload video
/// </summary>
public class MtrAddUploadVideoCmd : IRequest
{
    /// <summary>
    /// The message data from mass transit
    /// </summary>
    public required IMassVideoUploadedEvent Message { get; init; }
}

/// <summary>
/// Mediatr Command-Handler for add upload video command
/// </summary>
/// <remarks>
/// Primary constructor
/// </remarks>
/// <param name="unitOfWork">UnitOfWork. Injected by DI</param>
/// <param name="logger">Logger. Injected by DI</param>
public class MtrAddUploadVideoCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrAddUploadVideoCmdHandler> logger)
    : IRequestHandler<MtrAddUploadVideoCmd>
{
    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task</returns>
    public async Task Handle(MtrAddUploadVideoCmd request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for add upload video command was called for {@Message}",
            request.Message);

        logger.LogDebug("Get repository for upload video");
        var repository = unitOfWork.GetRepository<Domain.Entities.UploadVideo>();

        logger.LogDebug("Adding new upload video domain model to repository");
        var uploadVideo = new Domain.Entities.UploadVideo(request.Message.Id, request.Message.Filename);
        await repository.AddAsync(uploadVideo);

        logger.LogDebug("Saving changes to data store");
        await unitOfWork.SaveChangesAsync();
    }
}