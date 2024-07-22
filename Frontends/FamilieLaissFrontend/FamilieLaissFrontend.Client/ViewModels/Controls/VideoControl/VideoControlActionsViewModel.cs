using CommunityToolkit.Mvvm.Input;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using FamilieLaissSharedUI.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FamilieLaissFrontend.Client.ViewModels.Controls.VideoControl;

public partial class VideoControlActionsViewModel(ISnackbar snackbarService, IMessageBoxService messageBoxService)
    : ViewModelBase(snackbarService, messageBoxService)
{
    #region Parameters

    public IUploadVideoModel? UploadItem { get; set; }
    public IMediaItemModel? MediaItem { get; set; }
    public EventCallback<IUploadVideoModel> DeleteUploadClicked { get; set; }
    public EventCallback<IMediaItemModel> DeleteMediaClicked { get; set; }
    public EventCallback<IUploadVideoModel> ChooseClicked { get; set; }
    public EventCallback<IUploadVideoModel> EditClicked { get; set; }

    #endregion

    #region Commands

    [RelayCommand]
    public async Task Delete()
    {
        if (MediaItem is not null)
        {
            if (DeleteMediaClicked.HasDelegate)
            {
                await DeleteMediaClicked.InvokeAsync(MediaItem);
            }
        }
        else
        {
            if (DeleteUploadClicked.HasDelegate)
            {
                await DeleteUploadClicked.InvokeAsync(UploadItem);
            }
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