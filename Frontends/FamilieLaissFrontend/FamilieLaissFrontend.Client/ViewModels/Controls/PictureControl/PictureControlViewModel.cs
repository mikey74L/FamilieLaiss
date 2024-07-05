using CommunityToolkit.Mvvm.Input;
using EventAggregator.Blazor;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using FamilieLaissModels.EventAggregator.PictureControl;
using FamilieLaissSharedUI.ViewModels;
using MudBlazor;

namespace FamilieLaissFrontend.Client.ViewModels.Controls.PictureControl;

public partial class PictureControlViewModel : ViewModelBase, IHandle<AggSelectAllPicture>, IHandle<AggDeSelectAllPicture>
{
    #region Private Services
    private readonly IEventAggregator eventAggregator;
    #endregion

    #region Parameters
    public IUploadPictureModel? UploadItem;

    public IMediaItemModel? MediaItem;

    public bool ShowSelectionMode;
    #endregion

    #region C'tor
    public PictureControlViewModel(ISnackbar snackbarService, IMessageBoxService messageBoxService,
        IEventAggregator eventAggregator) : base(snackbarService, messageBoxService)
    {
        this.eventAggregator = eventAggregator;
    }
    #endregion

    #region Lifecycle
    public override void OnInitialized()
    {
        base.OnInitialized();

        eventAggregator.Subscribe(this);

        if (MediaItem is not null)
        {
            UploadItem = MediaItem.UploadPicture;
        }
    }

    public override void OnParametersSet()
    {
        base.OnParametersSet();

        if (!ShowSelectionMode && UploadItem is not null)
        {
            UploadItem.IsSelected = false;
        }
    }
    #endregion

    #region Commands
    [RelayCommand]
    private void ToggleChanged()
    {
        NotifyStateChanged();
    }
    #endregion

    #region EventAggregator
    public Task HandleAsync(AggSelectAllPicture message)
    {
        if (UploadItem is not null)
        {
            UploadItem.IsSelected = true;

            NotifyStateChanged();
        }

        return Task.CompletedTask;
    }

    public Task HandleAsync(AggDeSelectAllPicture message)
    {
        if (UploadItem is not null)
        {
            UploadItem.IsSelected = false;

            NotifyStateChanged();
        }

        return Task.CompletedTask;
    }
    #endregion

    #region Abstract overrides
    public override void Dispose()
    {
        eventAggregator.Unsubscribe(this);
    }
    #endregion
}
