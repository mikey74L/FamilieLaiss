using DomainHelper.Interfaces;
using InfrastructureHelper.Exceptions;
using MediatR;
using Settings.API.GraphQL.Mutations.UserSettings;
using Settings.Domain.Entities;

namespace Settings.API.Commands
{
    /// <summary>
    /// Mediatr Command for Change user settings
    /// </summary>
    public class MtrChangeUserSettingsCmd : IRequest<Settings.Domain.Entities.UserSettings>
    {
        #region Properties
        /// <summary>
        /// GraphQL input data
        /// </summary>
        public UpdateUserSettingsInput Input { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="input">GraphQL input data</param>
        public MtrChangeUserSettingsCmd(UpdateUserSettingsInput input)
        {
            Input = input;
        }
        #endregion
    }

    /// <summary>
    /// Mediatr Command-Handler for change user settings command
    /// </summary>
    public class MtrChangeUserSettingsCmdHandler : IRequestHandler<MtrChangeUserSettingsCmd, Settings.Domain.Entities.UserSettings>
    {
        #region Private Members
        private readonly iUnitOfWork _UnitOfWork;
        private readonly ILogger<MtrChangeUserSettingsCmdHandler> _Logger;
        #endregion

        #region C'tor

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="unitOfWork">The Unit of Work. Will be injected by DI.</param>
        /// <param name="logger">Logger. Injected by DI</param>
        public MtrChangeUserSettingsCmdHandler(iUnitOfWork unitOfWork, ILogger<MtrChangeUserSettingsCmdHandler> logger)
        {
            //Übernehmen der injected Classes
            _UnitOfWork = unitOfWork;
            _Logger = logger;
        }
        #endregion

        #region Mediatr-Handler
        /// <summary>
        /// Will be called by Mediatr
        /// </summary>
        /// <param name="request">The request data</param>
        /// <param name="cancellationToken">The cancelation token</param>
        /// <returns>Task</returns>
        public async Task<Settings.Domain.Entities.UserSettings> Handle(MtrChangeUserSettingsCmd request, CancellationToken cancellationToken)
        {
            //Logging ausgeben
            _Logger.LogInformation("Mediatr-Handler for update user settings command was called for {@Input}", request.Input);

            //Repository für User-Settings ermitteln
            _Logger.LogDebug("Get repository for UserSettings");
            var RepositorySettings = _UnitOfWork.GetRepository<UserSettings>();

            //Ermitteln des Models für die ID
            _Logger.LogDebug("Get user settings for requested user");
            var SettingModel = await RepositorySettings.GetOneAsync(request.Input.id);

            if (SettingModel != null)
            {
                //Aktualisieren der user settings
                _Logger.LogDebug("Calling update method on domain model");
                SettingModel.UpdateSettings(request.Input.VideoAutoPlay, request.Input.VideoVolume, request.Input.VideoLoop,
                                            request.Input.VideoAutoPlayOtherVideos, request.Input.VideoTimeToPlayNextVideo,
                                            request.Input.ShowButtonForward, request.Input.ShowButtonRewind,
                                            request.Input.ShowZoomMenu, request.Input.ShowPlayRateMenu,
                                            request.Input.ShowMirrorButton, request.Input.ShowContextMenu,
                                            request.Input.ShowQualityMenu, request.Input.ShowTooltipForPlaytimeOnMouseCursor,
                                            request.Input.ShowTooltipForCurrentPlaytime, request.Input.ShowZoomInfo,
                                            request.Input.AllowZoomingWithMouseWheel,
                                            request.Input.GalleryCloseEsc,
                                            request.Input.GalleryCloseDimmer, request.Input.GalleryMouseWheelChangeSlide, request.Input.GalleryShowThumbnails,
                                            request.Input.GalleryShowFullScreen, request.Input.GalleryTransitionDuration, request.Input.GalleryTransitionType,
                                            request.Input.SimpleFilter, request.Input.QuestionKeepUploadWhenDelete, request.Input.DefaultKeepUploadWhenDelete);

                //Aktualisieren des Models im Repository
                _Logger.LogDebug("Update model in repository");
                RepositorySettings.Update(SettingModel);

                //Speichern der Daten
                _Logger.LogDebug("Saving changes to data store");
                await _UnitOfWork.SaveChangesAsync();

                return SettingModel;
            }
            else
            {
                _Logger.LogError("Model not found");
                throw new DomainNotFoundException();
            }
        }
        #endregion
    }
}
