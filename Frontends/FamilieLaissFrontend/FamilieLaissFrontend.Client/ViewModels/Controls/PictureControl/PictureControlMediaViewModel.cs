using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FamilieLaissEnums;
using FamilieLaissFrontend.Client.Dialogs.PictureInfo;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using FamilieLaissSharedUI.ViewModels;
using MudBlazor;

namespace FamilieLaissFrontend.Client.ViewModels.Controls.PictureControl;

public partial class PictureControlMediaViewModel(
    IUrlHelperService urlHelperService,
    IDialogService dialogService,
    ISnackbar snackbarService,
    IMessageBoxService messageBoxService)
    : ViewModelBase(snackbarService, messageBoxService)
{
    #region Parameters
    public EnumPictureControlType ControlType { get; set; }
    public IUploadPictureModel? UploadItem { get; set; }
    public IMediaItemModel? MediaItem { get; set; }
    #endregion

    #region Public Properties
    [ObservableProperty]
    private bool _isOverlayPictureActive;

    public string ImageUrl => UploadItem is not null ? urlHelperService.GetUrlForUploadPictureCard(UploadItem) : "";
    #endregion

    #region Commands
    [RelayCommand]
    public async Task ShowInfoDialog()
    {
        var dialogParams = new DialogParameters
        {
            { "ControlType", ControlType },
            { "UploadItem", UploadItem },
            { "MediaItem", MediaItem }
        };

        var dialogOptions = GetDialogOptions();
        dialogOptions.CloseButton = true;
        dialogOptions.CloseOnEscapeKey = true;
        dialogOptions.Position = DialogPosition.Center;
        dialogOptions.MaxWidth = MaxWidth.ExtraExtraLarge;

        await dialogService.ShowAsync<PictureInfoDialog>("", dialogParams, dialogOptions);
    }

    [RelayCommand]
    public void ToggleOverlayPicture()
    {
        IsOverlayPictureActive = !IsOverlayPictureActive;
    }
    #endregion

    #region Abstract overrides
    public override void Dispose()
    {
    }
    #endregion
}
