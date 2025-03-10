using Catalog.API.GraphQL.Mutations.MediaItem;
using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using FamilieLaissSharedObjects.Enums;
using MediatR;

namespace Catalog.API.Mediator.Commands.MediaItem;

/// <summary>
/// Mediatr Command for make new media item entry
/// </summary>
public class MtrAddMediaItemCommand : IRequest<Domain.Aggregates.MediaItem>
{
    #region Properties

    /// <summary>
    /// The input data from GraphQL Mutation
    /// </summary>
    public required AddMediaItemInput InputData { get; init; }

    #endregion
}

/// <summary>
/// Mediatr Command-Handler for Make new media item entry command
/// </summary>
public class MtrAddMediaItemCommandHandler(iUnitOfWork unitOfWork, ILogger<MtrAddMediaItemCommandHandler> logger)
    : IRequestHandler<MtrAddMediaItemCommand, Domain.Aggregates.MediaItem>
{
    #region Mediatr-Handler

    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task</returns>
    public async Task<Domain.Aggregates.MediaItem> Handle(MtrAddMediaItemCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for make new media item entry command was called for {@Input}",
            request.InputData);

        logger.LogDebug("Get repository for media group");
        var repositoryGroup = unitOfWork.GetRepository<Domain.Aggregates.MediaGroup>();

        logger.LogDebug("Find media group model in store");
        var mediaGroupModel = await repositoryGroup.GetOneAsync(request.InputData.MediaGroupId);
        if (mediaGroupModel == null)
        {
            logger.LogError("Could not find media group with {ID}", request.InputData.MediaGroupId);
            throw new DomainException(DomainExceptionType.NoDataFound,
                $"Could not find media group with id = {request.InputData.MediaGroupId}");
        }

        string? uploadPictureName = null;
        if (request.InputData.MediaType == EnumMediaType.Picture)
        {
            logger.LogDebug("Get repository for upload picture");
            var repositoryUploadPicture = unitOfWork.GetRepository<Domain.Entities.UploadPicture>();

            logger.LogDebug("Find upload picture item in store");
            var uploadPictureModel = await repositoryUploadPicture.GetOneAsync(request.InputData.UploadItemId);

            uploadPictureName = uploadPictureModel?.Filename;
        }

        logger.LogDebug("Adding new media item domain model to media group");
        var newMediaItem = mediaGroupModel.AddMediaItem(request.InputData.MediaType, request.InputData.NameGerman,
            request.InputData.NameEnglish, request.InputData.DescriptionGerman, request.InputData.DescriptionEnglish,
            request.InputData.OnlyFamily, request.InputData.UploadItemId, uploadPictureName);

        logger.LogDebug("Assign category values to media item");
        foreach (var item in request.InputData.CategoryValueIds)
        {
            newMediaItem.AddCategoryValue(item);
        }

        logger.LogDebug("Saving changes to data store");
        await unitOfWork.SaveChangesAsync();

        return newMediaItem;
    }

    #endregion
}