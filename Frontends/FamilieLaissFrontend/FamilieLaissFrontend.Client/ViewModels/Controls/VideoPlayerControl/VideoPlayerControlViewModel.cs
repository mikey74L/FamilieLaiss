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

public partial class VideoPlayerControlViewModel(
    ISnackbar snackbarService,
    IMessageBoxService messageBoxService,
    IJSRuntime jsRuntime,
    IOptions<AppSettings> appSettings,
    IUserSettingsService userSettingsService)
    : ViewModelBase(snackbarService, messageBoxService)
{
    #region Private Members
    private IJSObjectReference? _module;
    private IUploadVideoModel? _currentVideoToPlay;
    private IUserSettingsModel? _userSettingsModel;
    #endregion

    #region Parameters
    public IUploadVideoModel VideoItemToPlay { get; set; } = default!;
    public Task<AuthenticationState> AuthenticationState { get; set; } = default!;
    #endregion

    #region Public Properties
    [ObservableProperty]
    private string _idPlayer = string.Empty;
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
            _module = await jsRuntime.InvokeAsync<IJSObjectReference>("import", "./scripts/VideoJsOperations.js");

            _userSettingsModel = await userSettingsService.GetCurrentUserSettings(AuthenticationState);

            //Initialisieren des Players
            await _module.InvokeVoidAsync("videoJsOperations.initializePlayer",
            IdPlayer,
            "de",
                GetVideoUrl(),
                GetVideoType(),
                GetVttUrl(),
                _userSettingsModel);
        }
    }
    #endregion

    #region Private Methods
    private string GetVideoUrl()
    {
        //Deklaration
        string returnValue = "";

        //Zusammensetzen der URL
        if (_currentVideoToPlay is not null)
        {
            switch (_currentVideoToPlay.VideoType)
            {
                case EnumVideoType.Progressive:
                    returnValue = appSettings.Value.UrlVideo + $"/{_currentVideoToPlay.Id}.mp4";
                    break;
                case EnumVideoType.Hls:
                    returnValue = appSettings.Value.UrlVideo + $"/{_currentVideoToPlay.Id}.m3u8";
                    break;
            }
        }

        //Funktionsergebnis
        return returnValue;
    }

    private string GetVideoType()
    {
        //Deklaration
        string returnValue = "";

        //Ermitteln des Videotyps
        if (_currentVideoToPlay is not null)
        {
            switch (_currentVideoToPlay.VideoType)
            {
                case EnumVideoType.Progressive:
                    returnValue = "video/mp4";
                    break;
                case EnumVideoType.Hls:
                    returnValue = "application/x-mpegURL";
                    break;
            }
        }

        //Funktionsergebnis
        return returnValue;
    }

    private string GetVttUrl()
    {
        //Deklaration
        string returnValue = "";

        //Zusammenstellen der Url
        if (_currentVideoToPlay is not null)
        {
            returnValue = appSettings.Value.UrlVideoVtt + $"?filenameVTT={_currentVideoToPlay.Id}.vtt";
        }

        //Funktionsergebnis
        return returnValue;
    }

    private void GetCurrentVideoToPlay()
    {
        _currentVideoToPlay = VideoItemToPlay;
    }
    #endregion

    #region Abstract overrides
    public override void Dispose()
    {
    }
    #endregion
}
