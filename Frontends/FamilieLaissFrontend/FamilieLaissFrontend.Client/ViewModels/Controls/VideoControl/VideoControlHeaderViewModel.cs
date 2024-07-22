using CommunityToolkit.Mvvm.Input;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using FamilieLaissSharedUI.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FamilieLaissFrontend.Client.ViewModels.Controls.VideoControl;

public partial class VideoControlHeaderViewModel(ISnackbar snackbarService, IMessageBoxService messageBoxService)
    : ViewModelBase(snackbarService, messageBoxService)
{
    #region Parameters
    public IUploadVideoModel? UploadItem { get; set; }
    public EventCallback ToggleChanged { get; set; }
    #endregion

    #region Commands
    [RelayCommand]
    public async Task ToggledChanged()
    {
        if (UploadItem is not null)
        {
            UploadItem.IsSelected = !UploadItem.IsSelected;

            if (ToggleChanged.HasDelegate)
            {
                await ToggleChanged.InvokeAsync();
            }
        }
    }
    #endregion

    #region Abstract overrides
    public override void Dispose()
    {
    }
    #endregion
}
