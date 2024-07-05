using CommunityToolkit.Mvvm.Input;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using FamilieLaissSharedUI.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FamilieLaissFrontend.Client.ViewModels.Controls.VideoControl;

public partial class VideoControlActionsViewModel : ViewModelBase
{
    #region Parameters
    public IUploadVideoModel? UploadItem { get; set; }
    public EventCallback<IUploadVideoModel> DeleteClicked { get; set; }
    public EventCallback<IUploadVideoModel> ChooseClicked { get; set; }
    public EventCallback<IUploadVideoModel> EditClicked { get; set; }
    #endregion

    #region C'tor
    public VideoControlActionsViewModel(ISnackbar snackbarService, IMessageBoxService messageBoxService) : base(snackbarService, messageBoxService)
    {
    }
    #endregion

    #region Commands
    [RelayCommand]
    public async Task Delete()
    {
        if (DeleteClicked.HasDelegate)
        {
            await DeleteClicked.InvokeAsync(UploadItem);
        }
    }

    [RelayCommand]
    public async Task Choose()
    {
        if (ChooseClicked.HasDelegate)
        {
            await ChooseClicked.InvokeAsync(UploadItem);
        }
    }

    [RelayCommand]
    public async Task Edit()
    {
        if (EditClicked.HasDelegate)
        {
            await EditClicked.InvokeAsync(UploadItem);
        }
    }
    #endregion

    #region Abstract overrides
    public override void Dispose()
    {
    }
    #endregion
}
