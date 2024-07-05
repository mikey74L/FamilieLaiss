using CommunityToolkit.Mvvm.Input;
using FamilieLaissInterfaces.Services;
using FamilieLaissSharedUI.ViewModels;
using MudBlazor;

namespace FamilieLaissFrontend.Client.ViewModels.Controls.VideoControl;

public partial class VideoControlViewModel : ViewModelBase
{
    #region C'tor
    public VideoControlViewModel(ISnackbar snackbarService, IMessageBoxService messageBoxService) : base(snackbarService, messageBoxService)
    {
    }
    #endregion

    #region Commands
    [RelayCommand]
    private void ToggleChanged()
    {
        NotifyStateChanged();
    }
    #endregion

    #region Abstract overrides
    public override void Dispose()
    {
    }
    #endregion
}
