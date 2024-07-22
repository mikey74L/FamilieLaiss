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
using FamilieLaissResources.Resources.ViewModels.Pages.BaseData.Upload.Video;
using FamilieLaissServices.Extensions;
using FamilieLaissSharedUI.Helper;
using FamilieLaissSharedUI.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FamilieLaissFrontend.Client.ViewModels.Pages.BaseData.Upload.Video;

public partial class VideoUploadListViewModel(
    ISnackbar snackbarService,
    IMessageBoxService messageBoxService,
    IUploadVideoDataService uploadVideoService,
    IEventAggregator eventAggregator)
    : ViewModelBase(snackbarService, messageBoxService), IHandle<AggFilterChanged>, IHandle<AggEditSortCriteria>,
        IHandle<AggEditFilterCriteria>
{
    #region Parameters
    public ExtendedObservableCollection<IUploadVideoModel> UploadItems { get; set; } = [];
    public IGraphQlSortAndFilterService<IUploadVideoModel, UploadVideoSortInput, UploadVideoFilterInput> SortAndFilterService { get; set; } = default!;
    public bool ShowLoading { get; set; }
    public bool ShowError { get; set; }
    public EventCallback ReloadUploadVideos { get; set; }
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
    private async Task DeleteUploadVideo(IUploadVideoModel model)
    {
        var result = await QuestionConfirmRed(VideoUploadListViewModelRes.QuestionDeleteTitle,
            string.Format(VideoUploadListViewModelRes.QuestionDeleteMessage, model.Filename),
            VideoUploadListViewModelRes.QuestionDeleteConfirm,
            VideoUploadListViewModelRes.QuestionDeleteCancel);

        if (result.HasValue && result.Value)
        {
            IsSaving = true;

            await uploadVideoService.DeleteUploadVideoAsync(model)
                .HandleSuccess((_) =>
                {
                    UploadItems.Remove(model);

                    ShowErrorToast(string.Format(VideoUploadListViewModelRes.ToastDeleteSuccess,
                        model.Filename));

                    return Task.CompletedTask;
                })
                .HandleStatus(APIResultErrorType.NotAuthorized, (_) =>
                {
                    ShowErrorToast(string.Format(VideoUploadListViewModelRes.ToastDeleteErrorNotAuthorized,
                        model.Filename));

                    return Task.CompletedTask;
                })
                .HandleStatus(APIResultErrorType.NotFound, (_) =>
                {
                    ShowErrorToast(string.Format(VideoUploadListViewModelRes.ToastDeleteErrorNotFound,
                        model.Filename));

                    return Task.CompletedTask;
                })
                .HandleStatus(APIResultErrorType.CommunicationError, (_) =>
                {
                    ShowErrorToast(string.Format(VideoUploadListViewModelRes.ToastDeleteErrorCommunication,
                        model.Filename));

                    return Task.CompletedTask;
                });

            IsSaving = false;
        }
    }

    [RelayCommand]
    private async Task DeleteAllUploadVideos()
    {
        bool? result;
        List<IUploadVideoModel> uploadVideoIds = [];
        if (ShowSelectionMode && UploadItems.Any(x => x.IsSelected))
        {
            result = await QuestionConfirmWithCancel(VideoUploadListViewModelRes.QuestionDeleteAllSelectedTitle,
                VideoUploadListViewModelRes.QuestionDeleteAllSelectedMessage,
                VideoUploadListViewModelRes.QuestionDeleteAllButtonSelected,
                VideoUploadListViewModelRes.QuestionDeleteAllButtonExisting,
                VideoUploadListViewModelRes.QuestionDeleteAllButtonCancel, true, true, false);

            if (result.HasValue && result.Value)
            {
                uploadVideoIds = UploadItems.Where(x => x.IsSelected).ToList();
            }
            if (result.HasValue && !result.Value)
            {
                uploadVideoIds = UploadItems.ToList();
            }
        }
        else
        {
            result = await QuestionConfirmRed(VideoUploadListViewModelRes.QuestionDeleteAllTitle,
                VideoUploadListViewModelRes.QuestionDeleteAllMessage,
                VideoUploadListViewModelRes.QuestionDeleteAllButtonExisting,
                VideoUploadListViewModelRes.QuestionDeleteAllButtonCancel);

            if (result.HasValue && result.Value)
            {
                uploadVideoIds = UploadItems.ToList();
            }
        }

        if (uploadVideoIds.Any())
        {
            IsSaving = true;

            await uploadVideoService.DeleteAllUploadVideosAsync(uploadVideoIds)
                .HandleSuccess((_) =>
                {
                    UploadItems.Clear();

                    ShowSuccessToast(VideoUploadListViewModelRes.ToastDeleteAllSuccess);

                    return Task.CompletedTask;
                })
                .HandleStatus(APIResultErrorType.NotAuthorized, (_) =>
                {
                    ShowErrorToast(VideoUploadListViewModelRes.ToastDeleteAllErrorNotAuthorized);

                    return Task.CompletedTask;
                })
                .HandleStatus(APIResultErrorType.CommunicationError, (_) =>
                {
                    ShowErrorToast(VideoUploadListViewModelRes.ToastDeleteAllErrorCommunication);

                    return Task.CompletedTask;
                });

            IsSaving = false;
        }
    }

    [RelayCommand]
    private async Task RefreshItems()
    {
        await ReloadUploadVideos.InvokeAsync();
    }

    [RelayCommand]
    private void ToggleSortSidebar()
    {
        IsSortSidebarVisible = !IsSortSidebarVisible;
    }

    [RelayCommand]
    private void ToggleFilterSidebar()
    {
        IsFilterSidebarVisible = !IsFilterSidebarVisible;
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
    private async Task DeSelectAll()
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
