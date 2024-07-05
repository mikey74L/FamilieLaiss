using AutoMapper;
using DomainHelper.Interfaces;
using FamilieLaissSharedObjects.Enums;
using MediatR;
using Upload.Domain.Entities;
using Upload.DTO.UploadPicture;

namespace Upload.API.Mediator.Queries;

/// <summary>
/// Represents a query to get unassigned upload pictures.
/// </summary>
public class GetUnassignedUploadPicturesQuery : IRequest<IEnumerable<UploadPictureDto>>
{
}

/// <summary>
/// Represents a query handler to get unassigned upload pictures.
/// </summary>
public class GetUnassignedUploadPicturesQueryHandler(
    iUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<GetUnassignedUploadPicturesQueryHandler> logger)
    : IRequestHandler<GetUnassignedUploadPicturesQuery, IEnumerable<UploadPictureDto>>
{
    /// <summary>
    /// Handles the GetUnassignedUploadPicturesQuery and returns a collection of UploadPictureDto.
    /// </summary>
    /// <param name="request">The GetUnassignedUploadPicturesQuery request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of UploadPictureDto.</returns>
    public async Task<IEnumerable<UploadPictureDto>> Handle(GetUnassignedUploadPicturesQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for get unassigned upload pictures query was called");

        logger.LogDebug("Get repository for upload picture");
        var repository = unitOfWork.GetRepository<UploadPicture>();

        logger.LogDebug("Get unassigned upload pictures from repository");
        var allUploadPictures = await repository.GetAll(x => x.Status == EnumUploadStatus.Converted);

        logger.LogDebug("Mapping with auto mapper");
        var mappedUploadPictures = mapper.Map<IEnumerable<UploadPictureDto>>(allUploadPictures);

        return mappedUploadPictures;
    }
}