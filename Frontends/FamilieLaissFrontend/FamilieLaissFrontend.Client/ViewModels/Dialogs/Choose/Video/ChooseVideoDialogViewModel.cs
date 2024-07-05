using FamilieLaissInterfaces.Services;
using FamilieLaissSharedUI.ViewModels;
using MudBlazor;

namespace FamilieLaissFrontend.Client.ViewModels.Dialogs.Choose.Video;

public partial class ChooseVideoDialogViewModel : ViewModelBase
{
    #region C'tor
    public ChooseVideoDialogViewModel(ISnackbar snackbarService, IMessageBoxService messageBoxService) : base(snackbarService, messageBoxService)
    {
    }
    #endregion

    #region Abstract overrides
    public override void Dispose()
    {
    }
    #endregion 
}
