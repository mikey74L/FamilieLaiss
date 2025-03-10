using CommunityToolkit.Mvvm.Input;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using FamilieLaissSharedUI.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FamilieLaissFrontend.Client.ViewModels.Controls.PictureControl;

public partial class PictureControlActionsViewModel(ISnackbar snackbarService, IMessageBoxService messageBoxService)
    : ViewModelBase(snackbarService, messageBoxService)
{
    #region Parameters

    public IUploadPictureModel? UploadItem { get; set; }
    public IMediaItemModel? MediaItem { get; set; }
    public EventCallback<IUploadPictureModel> DeleteUploadClicked { get; set; }
    public EventCallback<IMediaItemModel> DeleteMediaClicked { get; set; }
    public EventCallback<IUploadPictureModel> ChooseClicked { get; set; }
    public EventCallback<IMediaItemModel> EditClicked { get; set; }

    #endregion

    #region Commands

    [RelayCommand]
    public async Task OnDelete()
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
    public async Task OnChoose()
    {
        if (ChooseClicked.HasDelegate)
        {
            await ChooseClicked.InvokeAsync(UploadItem);
        }
    }

    [RelayCommand]
    public async Task OnEdit()
    {
        if (EditClicked.HasDelegate)
        {
            await EditClicked.InvokeAsync(MediaItem);
        }
    }

    #endregion

    #region Abstract overrides

    public override void Dispose()
    {
    }

    #endregion
}