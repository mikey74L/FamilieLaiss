using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EventAggregator.Blazor;
using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.DataServices;
using FamilieLaissInterfaces.Enums;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using FamilieLaissModels.EventAggregator.Filter;
using FamilieLaissModels.Models.UploadPicture;
using FamilieLaissResources.Resources.ViewModels.Pages.BaseData.Upload.Picture;
using FamilieLaissServices.Extensions;
using FamilieLaissSharedUI.Helper;
using FamilieLaissSharedUI.ViewModels;
using Microsoft.AspNetCore.Components.Routing;
using MudBlazor;

namespace FamilieLaissFrontend.Client.ViewModels.Pages.BaseData.Upload.Picture;

public partial class PictureUploadPageViewModel(
    IUploadPictureDataService uploadPictureDataService,
    IGraphQlSortAndFilterServiceFactory graphQlSortAndFilterServiceFactory,
    IEventAggregator eventAggregator,
    ISnackbar snackbarService,
    IMessageBoxService messageBoxService)
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
    private ExtendedObservableCollection<IUploadPictureModel> _uploadPictureItems = [];

    [ObservableProperty]
    private int _currentCountUploadPictures;

    [ObservableProperty]
    private IGraphQlSortAndFilterService<IUploadPictureModel, UploadPictureSortInput, UploadPictureFilterInput> _sortAndFilterService = default!;
    #endregion

    #region Lifecycle Overrides
    public override void OnInitialized()
    {
        base.OnInitialized();

        SortAndFilterService = graphQlSortAndFilterServiceFactory.GetService<IUploadPictureModel, UploadPictureSortInput, UploadPictureFilterInput>("UploadView", ProvideNumberList);

        SortAndFilterService.SelectedSortCriteriaChanged = SortCriteriaChanged;

        eventAggregator.Subscribe(this);
    }

    public override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        await GetUploadPictureCount();
    }
    #endregion

    #region Private Methods
    private Task SortCriteriaChanged()
    {
        LoadUploadPictures();

        return Task.CompletedTask;
    }

    private async Task GetUploadPictureCount()
    {
        await uploadPictureDataService.GetUploadPictureCount()
            .HandleSuccess((value) =>
            {
                CurrentCountUploadPictures = value;

                return Task.CompletedTask;
            });
    }

    private Dictionary<int, string> ProvideNumberList(string propertyName)
    {
        return UploadPictureExifInfoModel.GetNumberValues(propertyName);
    }
    #endregion

    #region Commands
    [RelayCommand]
    private async Task LoadFilterValuesAsync()
    {
        var filterData = await uploadPictureDataService.GetUploadPictureExifInfoFilterData();

        if (filterData is not null)
        {
            await SortAndFilterService.SetFilterDataOnly(nameof(IUploadPictureExifInfoModel.Make), filterData.Makes);
            await SortAndFilterService.SetFilterDataOnly(nameof(IUploadPictureExifInfoModel.Model), filterData.Models);
            await SortAndFilterService.SetFilterDataOnly(nameof(IUploadPictureExifInfoModel.IsoSensitivity), filterData.ISOSensitivities);
            await SortAndFilterService.SetFilterDataOnly(nameof(IUploadPictureExifInfoModel.FNumber), filterData.FNumbers);

            Dictionary<double, string> targetDictExposure = [];
            foreach (var sourceItem in filterData.ExposureTimes)
            {
                targetDictExposure.Add(sourceItem, UploadPictureExifInfoModel.GetExposureTimeText(sourceItem));
            }
            await SortAndFilterService.SetFilterData(nameof(IUploadPictureExifInfoModel.ExposureTime), targetDictExposure);

            Dictionary<double, string> targetDictShutter = [];
            foreach (var sourceItem in filterData.ShutterSpeeds)
            {
                targetDictShutter.Add(sourceItem, UploadPictureExifInfoModel.GetShutterSpeedText(sourceItem));
            }
            await SortAndFilterService.SetFilterData(nameof(IUploadPictureExifInfoModel.ShutterSpeed), targetDictShutter);

            Dictionary<double, string> targetDictFocal = [];
            foreach (var sourceItem in filterData.FocalLengths)
            {
                targetDictFocal.Add(sourceItem, UploadPictureExifInfoModel.GetFocalLengthText(sourceItem));
            }
            await SortAndFilterService.SetFilterData(nameof(IUploadPictureExifInfoModel.FocalLength), targetDictFocal);
        }
    }

    [RelayCommand]
    private void LoadUploadPictures()
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

            await GetUploadPictureCount();
            await LoadFilterValuesAsync();
            LoadUploadPictures();

            IsLoading = false;
        }
        CurrentTabIndex = tabIndex;
    }

    [RelayCommand]
    private async Task BeforeInternalNavigation(LocationChangingContext context)
    {
        if (IsUploading)
        {
            await Message(PictureUploadPageViewModelRes.MessageTitleUploadInProgress,
                PictureUploadPageViewModelRes.MessageContentUploadInProgress,
                PictureUploadPageViewModelRes.MessageButtonUploadInProgress, false);

            context.PreventNavigation();
        }
    }
    #endregion

    #region Event-Aggregator
    public Task HandleAsync(AggFilterChanged message)
    {
        LoadUploadPictures();

        return Task.CompletedTask;
    }
    #endregion

    #region Abstract overrides
    protected override async void DebouncedLoading()
    {
        bool changeState = !IsLoading;

        IsLoading = true;

        await uploadPictureDataService.GetUploadPicturesForUploadViewAsync([SortAndFilterService.SelectedSortCriteria.GraphQlSortInput],
                SortAndFilterService.GetGraphQlFilterCriteria())
            .HandleStatus(APIResultErrorType.NoError, (result) =>
            {
                UploadPictureItems.Clear();
                UploadPictureItems.AddRange(result);

                if (changeState)
                {
                    IsLoading = false;
                }

                return Task.CompletedTask;
            })
            .HandleStatus(APIResultErrorType.NotAuthorized, (_) =>
            {
                ShowErrorToast(PictureUploadPageViewModelRes.ToastLoadingErrorNotAuthorized);

                HasError = true;

                if (changeState)
                {
                    IsLoading = false;
                }

                return Task.CompletedTask;
            })
            .HandleStatus(APIResultErrorType.ServerError, (_) =>
            {
                ShowErrorToast(PictureUploadPageViewModelRes.ToastLoadingErrorServerError);

                HasError = true;

                if (changeState)
                {
                    IsLoading = false;
                }

                return Task.CompletedTask;
            })
            .HandleStatus(APIResultErrorType.CommunicationError, (_) =>
            {
                ShowErrorToast(PictureUploadPageViewModelRes.ToastLoadingErrorCommunication);

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
