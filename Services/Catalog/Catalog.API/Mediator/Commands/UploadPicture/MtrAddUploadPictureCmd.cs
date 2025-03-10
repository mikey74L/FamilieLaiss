using DomainHelper.Interfaces;
using FamilieLaissMassTransitDefinitions.Contracts.Events.UploadPicture;
using MediatR;

namespace Catalog.API.Mediator.Commands.UploadPicture;

/// <summary>
/// Mediatr Command for add upload picture
/// </summary>
public class MtrAddUploadPictureCmd : IRequest
{
    /// <summary>
    /// The message data from mass transit
    /// </summary>
    public required IMassPictureUploadedEvent Message { get; init; }
}

/// <summary>
/// Mediatr Command-Handler for add upload picture command
/// </summary>
/// <remarks>
/// Primary constructor
/// </remarks>
/// <param name="unitOfWork">UnitOfWork. Injected by DI</param>
/// <param name="logger">Logger. Injected by DI</param>
public class MtrAddUploadPictureCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrAddUploadPictureCmdHandler> logger)
    : IRequestHandler<MtrAddUploadPictureCmd>
{
    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task</returns>
    public async Task Handle(MtrAddUploadPictureCmd request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for add upload picture command was called for {@Message}",
            request.Message);

        logger.LogDebug("Get repository for upload picture");
        var repository = unitOfWork.GetRepository<Domain.Entities.UploadPicture>();

        logger.LogDebug("Adding new upload picture domain model to repository");
        var newUploadPicture = new Domain.Entities.UploadPicture(request.Message.Id, request.Message.Filename);
        await repository.AddAsync(newUploadPicture);

        logger.LogDebug("Saving changes to data store");
        await unitOfWork.SaveChangesAsync();
    }
}