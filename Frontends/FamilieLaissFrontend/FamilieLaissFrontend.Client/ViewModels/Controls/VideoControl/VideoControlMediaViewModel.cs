﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FamilieLaissFrontend.Client.Dialogs.VideoPlayer;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using FamilieLaissSharedUI.ViewModels;
using MudBlazor;

namespace FamilieLaissFrontend.Client.ViewModels.Controls.VideoControl;

public partial class VideoControlMediaViewModel : ViewModelBase
{
    #region Services
    private readonly IUrlHelperService urlHelperService;
    private readonly IDialogService dialogService;
    #endregion

    #region Parameters
    public IUploadVideoModel? UploadItem { get; set; }
    #endregion

    #region Public Properties
    public string ImageUrlForVideo => UploadItem is not null ? urlHelperService.GetUrlForUploadVideoCard(UploadItem) : "";

    [ObservableProperty]
    private bool _isOverlayVideoActive;
    #endregion

    #region C'tor
    public VideoControlMediaViewModel(ISnackbar snackbarService, IMessageBoxService messageBoxService,
        IUrlHelperService urlHelperService, IDialogService dialogService) : base(snackbarService, messageBoxService)
    {
        this.urlHelperService = urlHelperService;
        this.dialogService = dialogService;
    }
    #endregion

    #region Commands
    [RelayCommand]
    private void ToggleOverlayVideo()
    {
        IsOverlayVideoActive = !IsOverlayVideoActive;
    }

    [RelayCommand]
    private async Task ShowPlayerDialog()
    {
        var dialogParams = new DialogParameters
        {
            { "UploadVideoItem", UploadItem }
        };

        var dialogOptions = GetDialogOptions();
        dialogOptions.CloseButton = true;
        dialogOptions.CloseOnEscapeKey = true;
        dialogOptions.Position = DialogPosition.Center;
        dialogOptions.MaxWidth = MaxWidth.ExtraExtraLarge;

        await dialogService.ShowAsync<VideoPlayerDialog>("", dialogParams, dialogOptions);
    }
    #endregion

    #region Abstract overrides
    public override void Dispose()
    {
    }
    #endregion
}