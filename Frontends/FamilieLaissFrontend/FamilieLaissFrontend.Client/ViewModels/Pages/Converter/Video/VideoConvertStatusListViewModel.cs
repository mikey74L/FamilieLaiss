using FamilieLaissInterfaces.Services;
using FamilieLaissSharedUI.ViewModels;
using MudBlazor;

namespace FamilieLaissFrontend.Client.ViewModels.Pages.Converter.Video;

public class VideoConvertStatusListViewModel(ISnackbar snackbarService, IMessageBoxService messageBoxService)
    : ViewModelBase(snackbarService, messageBoxService)
{
    #region Abstract overrides
    public override void Dispose()
    {
    }
    #endregion
}
