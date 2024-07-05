using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EventAggregator.Blazor;
using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.DataServices;
using FamilieLaissInterfaces.Enums;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using FamilieLaissModels.EventAggregator.Filter;
using FamilieLaissModels.EventAggregator.PictureControl;
using FamilieLaissResources.Resources.ViewModels.Pages.BaseData.Upload.Picture;
using FamilieLaissServices.Extensions;
using FamilieLaissSharedUI.Helper;
using FamilieLaissSharedUI.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FamilieLaissFrontend.Client.ViewModels.Pages.BaseData.Upload.Picture;

public partial class PictureUploadListViewModel : ViewModelBase, IHandle<AggFilterChanged>, IHandle<AggEditSortCriteria>, IHandle<AggEditFilterCriteria>
{
    #region Services
    private readonly IUploadPictureDataService uploadPictureService;
    private readonly IEventAggregator eventAggregator;
    #endregion

    #region Public Parameters
    public ExtendedObservableCollection<IUploadPictureModel> UploadItems { get; set; } = [];
    public IGraphQlSortAndFilterService<IUploadPictureModel, UploadPictureSortInput, UploadPictureFilterInput> SortAndFilterService { get; set; } = default!;
    public bool ShowLoading { get; set; }
    public bool ShowError { get; set; }
    public EventCallback ReloadUploadPictures { get; set; }
    #endregion

    #region Public Properties
    [ObservableProperty]
    private bool _isSortSidebarVisible;
    [ObservableProperty]
    private bool _isFilterSidebarVisible;
    [ObservableProperty]
    private bool _isFilterActive;
    [ObservableProperty]
    private bool _showSelectionMode;
    #endregion

    #region C'tor
    public PictureUploadListViewModel(ISnackbar snackbarService, IMessageBoxService messageBoxService,
        IUploadPictureDataService uploadPictureService, IEventAggregator eventAggregator) : base(snackbarService, messageBoxService)
    {
        this.uploadPictureService = uploadPictureService;
        this.eventAggregator = eventAggregator;
    }
    #endregion

    #region Lifecycle
    public override void OnInitialized()
    {
        base.OnInitialized();

        eventAggregator.Subscribe(this);
    }

    public override void OnParametersSet()
    {
        base.OnParametersSet();

        IsLoading = ShowLoading;
        HasError = ShowError;
    }
    #endregion

    #region Commands
    [RelayCommand]
    private async Task DeleteUploadPicture(IUploadPictureModel model)
    {
        var result = await QuestionConfirmRed(PictureUploadListViewModelRes.QuestionDeleteTitle,
            string.Format(PictureUploadListViewModelRes.QuestionDeleteMessage, model.Filename),
            PictureUploadListViewModelRes.QuestionDeleteConfirm,
            PictureUploadListViewModelRes.QuestionDeleteCancel);

        if (result.HasValue && result.Value)
        {
            IsSaving = true;

            await uploadPictureService.DeleteUploadPictureAsync(model)
                .HandleSuccess((_) =>
                {
                    UploadItems.Remove(model);

                    ShowErrorToast(string.Format(PictureUploadListViewModelRes.ToastDeleteSuccess,
                        model.Filename));

                    return Task.CompletedTask;
                })
                .HandleStatus(APIResultErrorType.NotAuthorized, (_) =>
                {
                    ShowErrorToast(string.Format(PictureUploadListViewModelRes.ToastDeleteErrorNotAuthorized,
                        model.Filename));

                    return Task.CompletedTask;
                })
                .HandleStatus(APIResultErrorType.NotFound, (_) =>
                {
                    ShowErrorToast(string.Format(PictureUploadListViewModelRes.ToastDeleteErrorNotFound,
                        model.Filename));

                    return Task.CompletedTask;
                })
                .HandleStatus(APIResultErrorType.CommunicationError, (_) =>
                {
                    ShowErrorToast(string.Format(PictureUploadListViewModelRes.ToastDeleteErrorCommunication,
                        model.Filename));

                    return Task.CompletedTask;
                });

            IsSaving = false;
        }
    }

    [RelayCommand]
    private async Task DeleteAllUploadPictures()
    {
        bool? result = null;
        List<IUploadPictureModel> UploadPictureIds = [];
        if (ShowSelectionMode && UploadItems.Any(x => x.IsSelected))
        {
            result = await QuestionConfirmWithCancel(PictureUploadListViewModelRes.QuestionDeleteAllSelectedTitle,
            PictureUploadListViewModelRes.QuestionDeleteAllSelectedMessage,
            PictureUploadListViewModelRes.QuestionDeleteAllButtonSelected,
                PictureUploadListViewModelRes.QuestionDeleteAllButtonExisting,
                PictureUploadListViewModelRes.QuestionDeleteAllButtonCancel, true, true, false);

            if (result.HasValue && result.Value)
            {
                UploadPictureIds = UploadItems.Where(x => x.IsSelected).ToList();
            }
            if (result.HasValue && !result.Value)
            {
                UploadPictureIds = UploadItems.ToList();
            }
        }
        else
        {
            result = await QuestionConfirmRed(PictureUploadListViewModelRes.QuestionDeleteAllTitle,
            PictureUploadListViewModelRes.QuestionDeleteAllMessage,
                PictureUploadListViewModelRes.QuestionDeleteAllButtonExisting,
                PictureUploadListViewModelRes.QuestionDeleteAllButtonCancel);

            if (result.HasValue && result.Value)
            {
                UploadPictureIds = UploadItems.ToList();
            }
        }

        if (UploadPictureIds.Any())
        {
            IsSaving = true;

            await uploadPictureService.DeleteAllUploadPicturesAsync(UploadPictureIds)
                .HandleSuccess((_) =>
                {
                    UploadItems?.Clear();

                    ShowSuccessToast(PictureUploadListViewModelRes.ToastDeleteAllSuccess);

                    return Task.CompletedTask;
                })
                .HandleStatus(APIResultErrorType.NotAuthorized, (_) =>
                {
                    ShowErrorToast(PictureUploadListViewModelRes.ToastDeleteAllErrorNotAuthorized);

                    return Task.CompletedTask;
                })
                .HandleStatus(APIResultErrorType.CommunicationError, (_) =>
                {
                    ShowErrorToast(PictureUploadListViewModelRes.ToastDeleteAllErrorCommunication);

                    return Task.CompletedTask;
                });

            IsSaving = false;
        }
    }

    [RelayCommand]
    private async Task RefreshItems()
    {
        await ReloadUploadPictures.InvokeAsync();
    }

    [RelayCommand]
    private void ShowSortSidebar()
    {
        IsSortSidebarVisible = true;
    }

    [RelayCommand]
    private void ShowFilterSidebar()
    {
        IsFilterSidebarVisible = true;
    }

    [RelayCommand]
    private void ToggleSelectionMode()
    {
        ShowSelectionMode = !ShowSelectionMode;
    }

    [RelayCommand]
    private async Task SelectAll()
    {
        await eventAggregator.PublishAsync(new AggSelectAllPicture());
    }

    [RelayCommand]
    public async Task DeSelectAll()
    {
        await eventAggregator.PublishAsync(new AggDeSelectAllPicture());
    }
    #endregion

    #region EventAggregator
    public Task HandleAsync(AggEditSortCriteria message)
    {
        IsSortSidebarVisible = true;

        NotifyStateChanged();

        return Task.CompletedTask;
    }

    public Task HandleAsync(AggEditFilterCriteria message)
    {
        IsFilterSidebarVisible = true;

        NotifyStateChanged();

        return Task.CompletedTask;
    }

    public Task HandleAsync(AggFilterChanged message)
    {
        IsFilterActive = SortAndFilterService.GetGraphQlFilterCriteria() != null;

        NotifyStateChanged();

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
