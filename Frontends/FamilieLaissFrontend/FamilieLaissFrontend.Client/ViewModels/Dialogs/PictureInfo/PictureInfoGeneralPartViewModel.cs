using CommunityToolkit.Mvvm.ComponentModel;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using FamilieLaissSharedUI.ViewModels;
using MudBlazor;

namespace FamilieLaissFrontend.Client.ViewModels.Dialogs.PictureInfo;

public partial class PictureInfoGeneralPartViewModel(ISnackbar snackbarService, IMessageBoxService messageBoxService)
    : ViewModelBase(snackbarService, messageBoxService)
{
    #region Parameters
    public IUploadPictureModel? UploadItem { get; set; }
    public IMediaItemModel? MediaItem { get; set; }
    #endregion

    #region Public Properties
    [ObservableProperty]
    private IUploadPictureModel _pictureModel = default!;
    #endregion

    #region Lifecycle
    public override void OnParametersSet()
    {
        base.OnParametersSet();

        if (MediaItem?.UploadPicture is not null)
        {
            PictureModel = MediaItem.UploadPicture;
        }
        else
        {
            if (UploadItem is not null)
            {
                PictureModel = UploadItem;
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
