using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EventAggregator.Blazor;
using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.DataServices;
using FamilieLaissInterfaces.Enums;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using FamilieLaissModels.EventAggregator.Filter;
using FamilieLaissResources.Resources.ViewModels.Pages.BaseData.Upload.Video;
using FamilieLaissServices.Extensions;
using FamilieLaissSharedUI.Helper;
using FamilieLaissSharedUI.ViewModels;
using Microsoft.AspNetCore.Components.Routing;
using MudBlazor;

namespace FamilieLaissFrontend.Client.ViewModels.Pages.BaseData.Upload.Video;

public partial class VideoUploadPageViewModel(
    ISnackbar snackbarService,
    IMessageBoxService messageBoxService,
    IUploadVideoDataService uploadVideoDataService,
    IGraphQlSortAndFilterServiceFactory graphQlSortAndFilterServiceFactory,
    IEventAggregator eventAggregator)
    : ViewModelBase(snackbarService, messageBoxService), IHandle<AggFilterChanged>
{
    #region Public Properties
    [ObservableProperty]
    // ReSharper disable once InconsistentNaming
    public int _uploadFileCount;

    [ObservableProperty]
    private bool _isUploading;

    [ObservableProperty]
    private int _currentTabIndex;

    [ObservableProperty]
    private ExtendedObservableCollection<IUploadVideoModel> _uploadVideoItems = [];

    [ObservableProperty]
    private int _currentCountUploadVideos;

    [ObservableProperty]
    private IGraphQlSortAndFilterService<IUploadVideoModel, UploadVideoSortInput, UploadVideoFilterInput> _sortAndFilterService = default!;
    #endregion

    #region Lifecycle Overrides
    public override void OnInitialized()
    {
        base.OnInitialized();

        SortAndFilterService = graphQlSortAndFilterServiceFactory.GetService<IUploadVideoModel, UploadVideoSortInput, UploadVideoFilterInput>("UploadView", ProvideNumberList);

        SortAndFilterService.SelectedSortCriteriaChanged = SortCriteriaChanged;

        eventAggregator.Subscribe(this);
    }

    public override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        await GetUploadVideoCount();
    }
    #endregion

    #region Private Methods
    private Task SortCriteriaChanged()
    {
        LoadUploadVideos();

        return Task.CompletedTask;
    }

    private async Task GetUploadVideoCount()
    {
        await uploadVideoDataService.GetUploadVideoCount()
            .HandleSuccess((value) =>
            {
                CurrentCountUploadVideos = value;

                return Task.CompletedTask;
            });
    }

    private Dictionary<int, string> ProvideNumberList(string propertyName)
    {
        return new Dictionary<int, string>();
    }
    #endregion

    #region Commands
    [RelayCommand]
    private void LoadUploadVideos()
    {
        StartLoading();
    }

    [RelayCommand]
    private void UploadCountChanged(int count)
    {
        UploadFileCount = count;
    }

    [RelayCommand]
    private void UploadStarted()
    {
        IsUploading = true;
    }

    [RelayCommand]
    private void UploadFinished()
    {
        IsUploading = false;
    }

    [RelayCommand]
    private async Task TabIndexChanged(int tabIndex)
    {
        if (tabIndex == 1)
        {
            IsLoading = true;

            await GetUploadVideoCount();
            LoadUploadVideos();

            IsLoading = false;
        }
        CurrentTabIndex = tabIndex;
    }

    [RelayCommand]
    private async Task BeforeInternalNavigation(LocationChangingContext context)
    {
        if (IsUploading)
        {
            await Message(VideoUploadPageViewModelRes.MessageTitleUploadInProgress,
                VideoUploadPageViewModelRes.MessageContentUploadInProgress,
                VideoUploadPageViewModelRes.MessageButtonUploadInProgress, false);

            context.PreventNavigation();
        }
    }
    #endregion

    #region Event-Aggregator
    public Task HandleAsync(AggFilterChanged message)
    {
        LoadUploadVideos();

        return Task.CompletedTask;
    }
    #endregion

    #region Abstract overrides
    protected override async void DebouncedLoading()
    {
        bool changeState = !IsLoading;

        IsLoading = true;

        await uploadVideoDataService.GetUploadVideosForUploadViewAsync([SortAndFilterService.SelectedSortCriteria.GraphQlSortInput],
                SortAndFilterService.GetGraphQlFilterCriteria())
            .HandleStatus(APIResultErrorType.NoError, (result) =>
            {
                UploadVideoItems.Clear();
                UploadVideoItems.AddRange(result);

                if (changeState)
                {
                    IsLoading = false;
                }

                return Task.CompletedTask;
            })
            .HandleStatus(APIResultErrorType.NotAuthorized, (_) =>
            {
                ShowErrorToast(VideoUploadPageViewModelRes.ToastLoadingErrorNotAuthorized);

                HasError = true;

                if (changeState)
                {
                    IsLoading = false;
                }

                return Task.CompletedTask;
            })
            .HandleStatus(APIResultErrorType.ServerError, (_) =>
            {
                ShowErrorToast(VideoUploadPageViewModelRes.ToastLoadingErrorServerError);

                HasError = true;

                if (changeState)
                {
                    IsLoading = false;
                }

                return Task.CompletedTask;
            })
            .HandleStatus(APIResultErrorType.CommunicationError, (_) =>
            {
                ShowErrorToast(VideoUploadPageViewModelRes.ToastLoadingErrorCommunication);

                HasError = true;

                if (changeState)
                {
                    IsLoading = false;
                }

                return Task.CompletedTask;
            });
    }

    public override void Dispose()
    {
        eventAggregator.Unsubscribe(this);
    }
    #endregion
}