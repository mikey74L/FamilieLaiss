using CommunityToolkit.Mvvm.ComponentModel;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using FamilieLaissSharedUI.ViewModels;
using MudBlazor;

namespace FamilieLaissFrontend.Client.ViewModels.Dialogs.PictureInfo;

public partial class PictureInfoDialogViewModel : ViewModelBase
{
    #region Parameters
    public IMediaItemModel? MediaItem { get; set; }
    public IUploadPictureModel? UploadItem { get; set; }
    #endregion

    #region Public Properties
    [ObservableProperty]
    private IUploadPictureModel _pictureModel = default!;
    #endregion

    #region C'tor
    public PictureInfoDialogViewModel(ISnackbar snackbarService, IMessageBoxService messageBoxService) : base(snackbarService, messageBoxService)
    {
    }
    #endregion

    #region Lifecycle
    public override void OnInitialized()
    {
        base.OnInitialized();

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
