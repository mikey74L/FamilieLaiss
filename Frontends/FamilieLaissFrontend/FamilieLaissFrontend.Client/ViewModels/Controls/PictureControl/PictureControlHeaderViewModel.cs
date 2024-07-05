using CommunityToolkit.Mvvm.Input;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using FamilieLaissSharedUI.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FamilieLaissFrontend.Client.ViewModels.Controls.PictureControl;

public partial class PictureControlHeaderViewModel : ViewModelBase
{
    #region Parameters
    public IUploadPictureModel? UploadItem { get; set; }
    public EventCallback ToggleChanged { get; set; }
    #endregion

    #region Properties
    public string ClassAttributeToggleButton
    {
        get
        {
            string result = "fl-card-picture-toggle-button ";

            if (UploadItem?.IsSelected ?? false)
            {
                result += "fl-card-picture-toggle-button-selected ";
            }

            return result;
        }
    }
    #endregion

    #region C'tor
    public PictureControlHeaderViewModel(ISnackbar snackbarService, IMessageBoxService messageBoxService) : base(snackbarService, messageBoxService)
    {
    }
    #endregion

    #region Commands
    [RelayCommand]
    public async Task ChangeToggle()
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
