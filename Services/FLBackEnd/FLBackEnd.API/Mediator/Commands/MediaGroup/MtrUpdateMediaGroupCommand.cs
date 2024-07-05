using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using FLBackEnd.API.GraphQL.Mutations.MediaGroup;
using MediatR;

namespace FLBackEnd.API.Mediator.Commands.MediaGroup;

/// <summary>
/// Mediatr Command for update media group 
/// </summary>
public class MtrUpdateMediaGroupCommand : IRequest<Domain.Entities.MediaGroup>
{
    #region Properties
    /// <summary>
    /// GraphQL input data
    /// </summary>
    public required UpdateMediaGroupInput InputData { get; init; }
    #endregion
}

/// <summary>
/// Mediatr Command-Handler for update media group
/// </summary>
public class MtrUpdateMediaGroupCommandHandler(iUnitOfWork unitOfWork, ILogger<MtrUpdateMediaGroupCommandHandler> logger) : IRequestHandler<MtrUpdateMediaGroupCommand, Domain.Entities.MediaGroup>
{
    #region Mediatr-Handler
    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancelation token</param>
    /// <returns>Task</returns>
    public async Task<Domain.Entities.MediaGroup> Handle(MtrUpdateMediaGroupCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for update media group command was called for {@Input}", request.InputData);

        logger.LogDebug("Get repository for media group");
        var repository = unitOfWork.GetRepository<Domain.Entities.MediaGroup>();

        logger.LogDebug("Find model to update in store");
        var modelToUpdate = await repository.GetOneAsync(request.InputData.Id);
        if (modelToUpdate == null)
        {
            logger.LogError("Could not find media group with {ID}", request.InputData.Id);
            throw new DomainException(DomainExceptionType.NoDataFound,
                $"Could not find media group with id = {request.InputData.Id}");
        }

        logger.LogDebug("Update media group data");
        modelToUpdate.Update(request.InputData.NameGerman, request.InputData.NameEnglish, request.InputData.DescriptionGerman,
            request.InputData.DescriptionEnglish, request.InputData.EventDate);
        repository.Update(modelToUpdate);

        logger.LogDebug("Saving changes to data store");
        await unitOfWork.SaveChangesAsync();

        return modelToUpdate;
    }
    #endregion
}
