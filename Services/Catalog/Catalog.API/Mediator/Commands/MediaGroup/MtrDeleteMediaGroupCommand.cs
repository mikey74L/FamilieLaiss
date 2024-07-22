using Catalog.API.GraphQL.Mutations.MediaGroup;
using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using MediatR;

namespace Catalog.API.Mediator.Commands.MediaGroup;

/// <summary>
/// Mediatr Command for delete media group
/// </summary>
public class MtrDeleteMediaGroupCommand : IRequest<Domain.Aggregates.MediaGroup>
{
    #region Properties

    /// <summary>
    /// Input data from GraphQL
    /// </summary>
    public required DeleteMediaGroupInput InputData { get; init; }

    #endregion
}

/// <summary>
/// Mediatr Command-Handler for delete media group
/// </summary>
public class MtrDeleteMediaGroupCommandHandler(
    iUnitOfWork unitOfWork,
    ILogger<MtrDeleteMediaGroupCommandHandler> logger)
    : IRequestHandler<MtrDeleteMediaGroupCommand, Domain.Aggregates.MediaGroup>
{
    #region Mediatr-Handler

    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancelation token</param>
    /// <returns>Task</returns>
    public async Task<Domain.Aggregates.MediaGroup> Handle(MtrDeleteMediaGroupCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for delete media group command was called for {@Input}",
            request.InputData);

        logger.LogDebug("Set context parameter");
        unitOfWork.AddContextParameter("KeepUploadItem", request.InputData.KeepUploadItems);

        logger.LogDebug("Get repository for media group");
        var repository = unitOfWork.GetRepository<Domain.Aggregates.MediaGroup>();

        logger.LogDebug("Find model to delete in store");
        var modelToDelete = await repository.GetOneAsync(request.InputData.Id);
        if (modelToDelete == null)
        {
            logger.LogError("Could not find media group with id {ID}", request.InputData.Id);
            throw new DomainException(DomainExceptionType.NoDataFound,
                $"Could not find media group with id = {request.InputData.Id}");
        }

        logger.LogDebug("Delete media group domain model from store");
        repository.Delete(modelToDelete);

        logger.LogDebug("Saving changes to data store");
        await unitOfWork.SaveChangesAsync();

        return modelToDelete;
    }

    #endregion
}