using CommunityToolkit.Mvvm.Input;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using FamilieLaissSharedUI.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FamilieLaissFrontend.Client.ViewModels.Controls.PictureControl;

public partial class PictureControlActionsViewModel : ViewModelBase
{
    #region Parameters
    public IUploadPictureModel? UploadItem;
    public IMediaItemModel? MediaItem;
    public EventCallback<IUploadPictureModel> DeleteUploadClicked;
    public EventCallback<IMediaItemModel> DeleteMediaClicked;
    public EventCallback<IUploadPictureModel> ChooseClicked;
    public EventCallback<IMediaItemModel> EditClicked;
    #endregion

    #region C'tor
    public PictureControlActionsViewModel(ISnackbar snackbarService, IMessageBoxService messageBoxService) : base(snackbarService, messageBoxService)
    {
    }
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
