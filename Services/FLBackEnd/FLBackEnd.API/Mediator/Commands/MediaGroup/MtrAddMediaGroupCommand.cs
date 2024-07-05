using DomainHelper.Interfaces;
using FLBackEnd.API.GraphQL.Mutations.MediaGroup;
using MediatR;

namespace FLBackEnd.API.Mediator.Commands.MediaGroup;

/// <summary>
/// Mediatr Command for make new media group entry
/// </summary>
public class MtrAddMediaGroupCommand : IRequest<Domain.Entities.MediaGroup>
{
    #region Properties
    /// <summary>
    /// The input data from GraphQL Mutation
    /// </summary>
    public required AddMediaGroupInput InputData { get; init; }
    #endregion
}

/// <summary>
/// Mediatr Command-Handler for Make new media group entry command
/// </summary>
public class MtrAddMediaGroupCommandHandler(iUnitOfWork unitOfWork, ILogger<MtrAddMediaGroupCommandHandler> logger) : IRequestHandler<MtrAddMediaGroupCommand, Domain.Entities.MediaGroup>
{
    #region Mediatr-Handler
    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancelation token</param>
    /// <returns>Task</returns>
    public async Task<Domain.Entities.MediaGroup> Handle(MtrAddMediaGroupCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for make new media group entry command was called for {@Input}", request.InputData);

        logger.LogDebug("Get repository for media group");
        var repository = unitOfWork.GetRepository<Domain.Entities.MediaGroup>();

        logger.LogDebug("Adding new media group domain model to repository");
        var newMediaGroup = new Domain.Entities.MediaGroup(request.InputData.NameGerman,
            request.InputData.NameEnglish, request.InputData.DescriptionGerman, request.InputData.DescriptionEnglish,
            request.InputData.EventDate);
        await repository.AddAsync(newMediaGroup);

        logger.LogDebug("Saving changes to data store");
        await unitOfWork.SaveChangesAsync();

        return newMediaGroup;
    }
    #endregion
}
