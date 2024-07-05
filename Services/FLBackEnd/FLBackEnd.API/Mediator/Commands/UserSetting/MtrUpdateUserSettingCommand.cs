using DomainHelper.Exceptions;
using DomainHelper.Interfaces;
using FLBackEnd.API.GraphQL.Mutations.UserSetting;
using MediatR;

namespace FLBackEnd.API.Mediator.Commands.UserSetting;

/// <summary>
/// Mediatr Command for update user setting 
/// </summary>
public class MtrUpdateUserSettingCmd : IRequest<Domain.Entities.UserSetting>
{
    /// <summary>
    /// GraphQL input data
    /// </summary>
    public required UpdateUserSettingInput InputData { get; init; }
}

/// <summary>
/// Mediatr Command-Handler for update user setting
/// </summary>
/// <remarks>
/// Primary constructor
/// </remarks>
/// <param name="unitOfWork">Unit of work. Injected by DI</param>
/// <param name="logger">Logger. Injected by DI</param>
public class MtrUpdateUserSettingCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrUpdateUserSettingCmdHandler> logger)
    : IRequestHandler<MtrUpdateUserSettingCmd, Domain.Entities.UserSetting>
{
    /// <summary>
    /// Will be called by Mediatr
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>Task</returns>
    public async Task<Domain.Entities.UserSetting> Handle(MtrUpdateUserSettingCmd request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Mediatr-Handler for update user setting command was called for {@InputData}",
            request.InputData);

        logger.LogDebug("Get repository for user setting");
        var repository = unitOfWork.GetRepository<Domain.Entities.UserSetting>();

        logger.LogDebug("Get find model to update in store");
        var modelToUpdate = await repository.GetOneAsync(request.InputData.Id);
        if (modelToUpdate == null)
        {
            logger.LogError("Could not find user setting with {ID}", request.InputData.Id);
            throw new DomainException(DomainExceptionType.NoDataFound,
                $"Could not find user setting with id = {request.InputData.Id}");
        }

        logger.LogDebug("Update user setting data");
        modelToUpdate.UpdateSettings(request.InputData.VideoAutoPlay, request.InputData.VideoVolume, request.InputData.VideoLoop,
            request.InputData.VideoAutoPlayOtherVideos, request.InputData.VideoTimeToPlayNextVideo, request.InputData.VideoTimeSeekForwardRewind,
            request.InputData.GalleryCloseEsc, request.InputData.GalleryCloseDimmer,
            request.InputData.GalleryMouseWheelChangeSlide, request.InputData.GalleryShowThumbnails, request.InputData.GalleryShowFullScreen,
            request.InputData.GalleryTransitionDuration, request.InputData.GalleryTransitionType,
            request.InputData.QuestionKeepUploadWhenDelete, request.InputData.DefaultKeepUploadWhenDelete,
            request.InputData.ShowButtonForward, request.InputData.ShowButtonRewind, request.InputData.ShowZoomMenu,
            request.InputData.ShowPlayRateMenu, request.InputData.ShowMirrorButton, request.InputData.ShowQualityMenu,
            request.InputData.ShowZoomInfo, request.InputData.AllowZoomingWithMouseWheel,
            request.InputData.ShowTooltipForCurrentPlaytime, request.InputData.ShowTooltipForPlaytimeOnMouseCursor);
        repository.Update(modelToUpdate);

        logger.LogDebug("Saving changes to data store");
        await unitOfWork.SaveChangesAsync();

        return modelToUpdate;
    }
}