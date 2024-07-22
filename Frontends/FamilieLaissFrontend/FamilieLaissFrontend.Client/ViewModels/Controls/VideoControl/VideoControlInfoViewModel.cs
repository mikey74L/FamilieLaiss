using CommunityToolkit.Mvvm.ComponentModel;
using FamilieLaissEnums;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using FamilieLaissSharedUI.ViewModels;
using MudBlazor;

namespace FamilieLaissFrontend.Client.ViewModels.Controls.VideoControl;

public partial class VideoControlInfoViewModel(
    ISnackbar snackbarService,
    IMessageBoxService messageBoxService,
    IUrlHelperService urlHelperService)
    : ViewModelBase(snackbarService, messageBoxService)
{
    #region Parameters

    public IUploadVideoModel? Model { get; set; }

    #endregion

    #region Properties

    [ObservableProperty] private string _urlForResolutionPng = string.Empty;

    #endregion

    #region Lifecycle

    public override void OnInitialized()
    {
        base.OnInitialized();

        if (Model is null) return;
        switch (Model.Height)
        {
            case <= 360:
                UrlForResolutionPng = urlHelperService.GetUrlForVideoResolutionIcon(EnumVideoResolutionIcon.P360);
                break;
            case <= 480:
                UrlForResolutionPng = urlHelperService.GetUrlForVideoResolutionIcon(EnumVideoResolutionIcon.P480);
                break;
            case <= 720:
                UrlForResolutionPng = urlHelperService.GetUrlForVideoResolutionIcon(EnumVideoResolutionIcon.P720);
                break;
            case <= 1080:
                UrlForResolutionPng = urlHelperService.GetUrlForVideoResolutionIcon(EnumVideoResolutionIcon.P1080);
                break;
            case <= 2160:
                UrlForResolutionPng = urlHelperService.GetUrlForVideoResolutionIcon(EnumVideoResolutionIcon.P2160);
                break;
        }
    }

    #endregion

    #region Abstract overrides

    public override void Dispose()
    {
    }

    #endregion
}