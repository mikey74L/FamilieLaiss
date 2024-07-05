using CommunityToolkit.Mvvm.ComponentModel;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using FamilieLaissSharedUI.ViewModels;
using MudBlazor;

namespace FamilieLaissFrontend.Client.ViewModels.Dialogs.PictureInfo;

public partial class PictureInfoGeneralPageViewModel : ViewModelBase
{
    #region Services
    private readonly IUrlHelperService urlHelperService;
    #endregion

    #region Parameters
    public IUploadPictureModel? UploadItem { get; set; }
    public IMediaItemModel? MediaItem { get; set; }
    #endregion

    #region Public Properties
    [ObservableProperty]
    private IUploadPictureModel _pictureModel = default!;

    public string UrlPicture => urlHelperService.GetUrlForUploadPictureInfo(PictureModel);
    #endregion

    #region C'tor
    public PictureInfoGeneralPageViewModel(ISnackbar snackbarService, IMessageBoxService messageBoxService,
        IUrlHelperService urlHelperService) : base(snackbarService, messageBoxService)
    {
        this.urlHelperService = urlHelperService;
    }
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
