using AutoMapper;
using DomainHelper.Interfaces;
using MediatR;
using Upload.Domain.Entities;
using Upload.DTO.UploadPicture;

namespace Upload.API.Mediator.Queries;

/// <summary>
/// Represents a query to get all upload pictures.
/// </summary>
public class GetAllUploadPicturesQuery : IRequest<IEnumerable<UploadPictureDto>>
{
}

/// <summary>
/// Represents a query handler to get all upload pictures.
/// </summary>
public class GetAllCategoriesQueryHandler(
    iUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<GetAllCategoriesQueryHandler> logger)
    : IRequestHandler<GetAllUploadPicturesQuery, IEnumerable<UploadPictureDto>>
{
    /// <summary>
    /// Handles the GetAllUploadPicturesQuery and returns a collection of UploadPictureDto.
    /// </summary>
    /// <param name="request">The GetAllUploadPicturesQuery request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of UploadPictureDto.</returns>
    public async Task<IEnumerable<UploadPictureDto>> Handle(GetAllUploadPicturesQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for get all upload pictures query was called");

        logger.LogDebug("Get repository for upload picture");
        var repository = unitOfWork.GetRepository<UploadPicture>();

        logger.LogDebug("Get all categories from repository");
        var allUploadPictures = await repository.GetAll();

        logger.LogDebug("Mapping with auto mapper");
        var mappedUploadPictures = mapper.Map<IEnumerable<UploadPictureDto>>(allUploadPictures);

        return mappedUploadPictures;
    }
}