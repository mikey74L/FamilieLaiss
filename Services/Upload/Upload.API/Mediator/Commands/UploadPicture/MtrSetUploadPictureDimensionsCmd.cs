using DomainHelper.Interfaces;
using FamilieLaissMassTransitDefinitions.Contracts.Commands.UploadPicture;
using MediatR;

namespace Upload.API.Mediator.Commands.UploadPicture;

/// <summary>
/// Mediatr Command for set dimensions for upload picture
/// </summary>
public class MtrSetUploadPictureDimensionsCmd : IRequest
{
    /// <summary>
    /// Message data
    /// </summary>
    public required IMassSetUploadPictureDimensionsCmd Message { get; init; }
}

/// <summary>
/// Mediatr Command-Handler for set dimensions for upload picture
/// </summary>
/// <remarks>
/// Primary constructor 
/// </remarks>
/// <param name="unitOfWork">UnitOfWork. Injected by DI</param>
/// <param name="logger">Logger. Injected by DI</param>
public class MtrSetUploadPictureDimensionsCmdHandler(
    iUnitOfWork unitOfWork,
    ILogger<MtrSetUploadPictureDimensionsCmdHandler> logger)
    : IRequestHandler<MtrSetUploadPictureDimensionsCmd>
{
    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task</returns>
    public async Task Handle(MtrSetUploadPictureDimensionsCmd request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for set dimensions for upload picture command was called for {@Message}",
            request.Message);

        logger.LogInformation("Get repository from unit of work");
        var repo = unitOfWork.GetRepository<Domain.Entities.UploadPicture>();

        logger.LogInformation($"Get entity from repository for ID = {request.Message.Id}");
        var entity = await repo.GetOneAsync(request.Message.Id);

        logger.LogInformation($"Set EXIF-Info for picture");
        entity.UpdateSize(request.Message.Height, request.Message.Width);

        logger.LogInformation("Save changes");
        await unitOfWork.SaveChangesAsync();
    }
}