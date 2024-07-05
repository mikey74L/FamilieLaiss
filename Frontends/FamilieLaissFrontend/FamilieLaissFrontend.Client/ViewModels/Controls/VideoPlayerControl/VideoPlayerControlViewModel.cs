using CommunityToolkit.Mvvm.ComponentModel;
using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using FamilieLaissModels.Models;
using FamilieLaissSharedUI.ViewModels;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using MudBlazor;

namespace FamilieLaissFrontend.Client.ViewModels.Controls.VideoPlayerControl;

public partial class VideoPlayerControlViewModel : ViewModelBase
{
    #region Services
    private readonly IJSRuntime jsRuntime;
    private readonly IOptions<AppSettings> appSettings;
    private readonly IUserSettingsService userSettingsService;
    #endregion

    #region Private Members
    private IJSObjectReference? module;
    private IUploadVideoModel? currentVideoToPlay;
    private IUserSettingsModel? userSettingsModel;
    #endregion

    #region Parameters
    public IUploadVideoModel VideoItemToPlay { get; set; } = default!;
    public Task<AuthenticationState> AuthenticationState { get; set; } = default!;
    #endregion

    #region Public Properties
    [ObservableProperty]
    private string _idPlayer = string.Empty;
    #endregion

    #region C'tor
    public VideoPlayerControlViewModel(ISnackbar snackbarService, IMessageBoxService messageBoxService,
        IJSRuntime jsRuntime, IOptions<AppSettings> appSettings,
        IUserSettingsService userSettingsService) : base(snackbarService, messageBoxService)
    {
        this.jsRuntime = jsRuntime;
        this.appSettings = appSettings;
        this.userSettingsService = userSettingsService;
    }
    #endregion

    #region Lifecycle
    public override void OnInitialized()
    {
        base.OnInitialized();

        //Generieren einer GUID
        IdPlayer = "video-" + Guid.NewGuid().ToString();

        //Zum Start des Players wird das Video ermittelt welches als erstes abgespielt werden soll
        GetCurrentVideoToPlay();
    }

    public override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            //Laden des Moduls für die JavaScript-Routinen des Video-Players
            module = await jsRuntime.InvokeAsync<IJSObjectReference>("import", "./scripts/VideoJsOperations.js");

            userSettingsModel = await userSettingsService.GetCurrentUserSettings(AuthenticationState);

            //Initialisieren des Players
            await module.InvokeVoidAsync("videoJsOperations.initializePlayer",
            IdPlayer,
            "de",
                GetVideoUrl(),
                GetVideoType(),
                GetVttUrl(),
                userSettingsModel);
        }
    }
    #endregion

    #region Private Methods
    private string GetVideoUrl()
    {
        //Deklaration
        string ReturnValue = "";

        //Zusammensetzen der URL
        if (currentVideoToPlay is not null)
        {
            switch (currentVideoToPlay.VideoType)
            {
                case EnumVideoType.Progressive:
                    ReturnValue = appSettings.Value.UrlVideo + $"/{currentVideoToPlay.Id}.mp4";
                    break;
                case EnumVideoType.Hls:
                    ReturnValue = appSettings.Value.UrlVideo + $"/{currentVideoToPlay.Id}.m3u8";
                    break;
            }
        }

        //Funktionsergebnis
        return ReturnValue;
    }

    private string GetVideoType()
    {
        //Deklaration
        string ReturnValue = "";

        //Ermitteln des Videotypes
        if (currentVideoToPlay is not null)
        {
            switch (currentVideoToPlay.VideoType)
            {
                case EnumVideoType.Progressive:
                    ReturnValue = "video/mp4";
                    break;
                case EnumVideoType.Hls:
                    ReturnValue = "application/x-mpegURL";
                    break;
            }
        }

        //Funktionsergebnis
        return ReturnValue;
    }

    private string GetVttUrl()
    {
        //Deklaration
        string ReturnValue = "";

        //Zusammenstellen der Url
        if (currentVideoToPlay is not null)
        {
            ReturnValue = appSettings.Value.UrlVideoVtt + $"?filenameVTT={currentVideoToPlay.Id}.vtt";
        }

        //Funktionsergebnis
        return ReturnValue;
    }

    private void GetCurrentVideoToPlay()
    {
        if (VideoItemToPlay != null)
        {
            currentVideoToPlay = VideoItemToPlay;
        }
    }
    #endregion

    #region Abstract overrides
    public override void Dispose()
    {
    }
    #endregion
}
