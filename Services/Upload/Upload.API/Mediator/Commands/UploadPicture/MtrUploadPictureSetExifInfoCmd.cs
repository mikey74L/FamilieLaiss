using DomainHelper.Interfaces;
using FamilieLaissMassTransitDefinitions.Contracts.Commands;
using MediatR;

namespace Upload.API.Mediator.Commands.UploadPicture;

/// <summary>
/// Mediatr Command for set exif info for upload picture
/// </summary>
public class MtrUploadPictureSetExifInfoCmd : IRequest
{
    /// <summary>
    /// Message data
    /// </summary>
    public required IMassSetUploadPictureExifInfoCmd Message { get; init; }
}

/// <summary>
/// Mediatr Command-Handler for set exif info for upload picture
/// </summary>
/// <remarks>
/// Primary constructor 
/// </remarks>
/// <param name="unitOfWork">UnitOfWork. Injected by DI</param>
/// <param name="logger">Logger. Injected by DI</param>
public class MtrUploadPictureSetExifInfoCmdHandler(
    iUnitOfWork unitOfWork,
    ILogger<MtrUploadPictureSetExifInfoCmdHandler> logger)
    : IRequestHandler<MtrUploadPictureSetExifInfoCmd>
{
    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task</returns>
    public async Task Handle(MtrUploadPictureSetExifInfoCmd request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for delete upload picture command was called for {@Message}",
            request.Message);

        logger.LogInformation("Get repository from unit of work");
        var repo = unitOfWork.GetRepository<Domain.Entities.UploadPicture>();

        logger.LogInformation($"Get entity from repository for ID = {request.Message.Id}");
        var entity = await repo.GetOneAsync(request.Message.Id);

        logger.LogInformation($"Set EXIF-Info for picture");
        entity.SetExifData(request.Message.Make ?? "", request.Message.Model ?? "", request.Message.ResolutionX,
            request.Message.ResolutionY, request.Message.ResolutionUnit, request.Message.Orientation,
            request.Message.DdlRecorded, request.Message.ExposureTime, request.Message.ExposureProgram,
            request.Message.ExposureMode, request.Message.FNumber, request.Message.IsoSensitivity,
            request.Message.ShutterSpeed, request.Message.MeteringMode, request.Message.FlashMode,
            request.Message.FocalLength, request.Message.SensingMode, request.Message.WhiteBalanceMode,
            request.Message.Sharpness, request.Message.GpsLongitude, request.Message.GpsLatitude,
            request.Message.Contrast, request.Message.Saturation);

        logger.LogInformation("Save changes");
        await unitOfWork.SaveChangesAsync();
    }
}