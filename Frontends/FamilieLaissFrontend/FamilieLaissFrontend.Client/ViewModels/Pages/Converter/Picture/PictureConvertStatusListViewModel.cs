using FamilieLaissInterfaces.Services;
using FamilieLaissSharedUI.ViewModels;
using MudBlazor;

namespace FamilieLaissFrontend.Client.ViewModels.Pages.Converter.Picture
{
    public partial class PictureConvertStatusListViewModel : ViewModelBase
    {
        #region C'tor
        public PictureConvertStatusListViewModel(ISnackbar snackbarService, IMessageBoxService messageBoxService) : base(snackbarService, messageBoxService)
        {
        }
        #endregion

        #region Abstract overrides
        public override void Dispose()
        {
        }
        #endregion
    }
}
