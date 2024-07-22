using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EventAggregator.Blazor;
using FamilieLaissFrontend.Client.Dialogs.BaseData.MediaItem;
using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.DataServices;
using FamilieLaissInterfaces.Enums;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using FamilieLaissModels.EventAggregator.MediaItem;
using FamilieLaissResources.Resources.ViewModels.Pages.BaseData.Media;
using FamilieLaissServices.Extensions;
using FamilieLaissSharedUI.Helper;
using FamilieLaissSharedUI.Interfaces;
using FamilieLaissSharedUI.ViewModels;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace FamilieLaissFrontend.Client.ViewModels.Pages.BaseData.Media;

public partial class MediaItemListViewModel : ViewModelBase, IHandle<AggMediaItemCreated>, IHandle<AggMediaItemChanged>
{
    #region Services
    private readonly IEventAggregator _eventAggregator;
    private readonly IMediaGroupDataService _mediaGroupService;
    private readonly IMediaItemDataService _mediaItemDataService;
    private readonly IUserSettingsService _userSettingsService;
    private readonly IDialogService _dialogService;
    private readonly IMvvmNavigationManager _navManager;
    #endregion

    #region Parameters
    public Task<AuthenticationState> AuthenticationState { get; set; } = default!;
    #endregion

    #region Query Parameters
    public long MediaGroupId { get; set; }
    #endregion

    #region Properties
    [ObservableProperty]
    private IMediaGroupModel? _mediaGroup;

    public SortableObservableCollection<IMediaItemModel> MediaItems { get; } = [];
    #endregion

    #region C'tor
    public MediaItemListViewModel(ISnackbar snackbarService, IMessageBoxService messageBoxService,
        IMediaGroupDataService mediaGroupDataService, IMediaItemDataService mediaItemDataService,
        IUserSettingsService userSettingsService,
        IDialogService dialogService, IMvvmNavigationManager navManager, IEventAggregator eventAggregator) : base(snackbarService, messageBoxService)
    {
        this.QueryStringParameters.Add(nameof(MediaGroupId), typeof(long));

        this._eventAggregator = eventAggregator;
        this._mediaGroupService = mediaGroupDataService;
        this._mediaItemDataService = mediaItemDataService;
        this._userSettingsService = userSettingsService;
        this._dialogService = dialogService;
        this._navManager = navManager;

        this.MediaItems.CollectionChanged += MediaItems_CollectionChanged;
    }
    #endregion

    #region Lifecycle overrides
    public override void OnInitialized()
    {
        base.OnInitialized();

        _eventAggregator.Subscribe(this);
    }

    public override async Task OnInitializedAsync()
    {
        IsLoading = true;

        await LoadMediaGroup();

        await LoadMediaItems();

        IsLoading = false;
    }
    #endregion

    #region Private Methods
    private async Task LoadMediaGroup()
    {
        await _mediaGroupService.GetMediaGroupAsync(MediaGroupId)
            .HandleSuccess((result) =>
            {
                MediaGroup = result;

                return Task.CompletedTask;
            })
            .HandleErrors((_) =>
            {
                ShowErrorToast(MediaItemListViewModelRes.ToastInitializeError);

                return Task.CompletedTask;
            });
    }

    private async Task LoadMediaItems()
    {
        if (MediaGroup is not null)
        {
            await _mediaItemDataService.GetMediaItemsForGroupAsync(MediaGroup)
                .HandleSuccess((result) =>
                {
                    MediaItems.Clear();
                    MediaItems.AddRange(result);

                    return Task.CompletedTask;
                })
                .HandleErrors((_) =>
                {
                    ShowErrorToast(MediaItemListViewModelRes.ToastInitializeError);

                    return Task.CompletedTask;
                });
        }
    }
    #endregion

    #region Commands   
    [RelayCommand]
    private async Task AddNewItem()
    {
        DialogParameters dialogParam = new() { { "IsInEditMode", false }, { "MediaGroupId", MediaGroupId } };

        var dialogRef = await _dialogService.ShowAsync<MediaItemEditDialog>(
            MediaItemListViewModelRes.DialogTitleAdd, dialogParam, GetDialogOptions());
        await dialogRef.Result;
    }

    [RelayCommand]
    private async Task EditItem(IMediaItemModel model)
    {
        DialogParameters dialogParam = new() { { "IsInEditMode", true }, { "Model", model }, { "MediaGroupId", MediaGroupId } };

        await _dialogService.ShowAsync<MediaItemEditDialog>(
            MediaItemListViewModelRes.DialogTitleEdit, dialogParam, GetDialogOptions());
    }

    [RelayCommand]
    private async Task DeleteItem(IMediaItemModel model)
    {
        var result = await QuestionConfirmRed(
            MediaItemListViewModelRes.QuestionDeleteTitle,
            string.Format(MediaItemListViewModelRes.QuestionDeleteMessage,
                model.MediaType == EnumMediaType.Picture ? model.UploadPicture?.Filename : model.LocalizedName),
            MediaItemListViewModelRes.QuestionDeleteButtonConfirm,
            MediaItemListViewModelRes.QuestionDeleteButtonCancel);

        if (result.HasValue && result.Value)
        {
            bool keepUploadItem;
            var userSettings = await _userSettingsService.GetCurrentUserSettings(AuthenticationState);
            if (userSettings?.QuestionKeepUploadWhenDelete is not null && userSettings.QuestionKeepUploadWhenDelete.Value)
            {
                var title = model.MediaType == EnumMediaType.Picture ?
                    MediaItemListViewModelRes.QuestionKeepUploadPhotoTitle :
                    MediaItemListViewModelRes.QuestionKeepUploadVideoTitle;
                var message = model.MediaType == EnumMediaType.Picture ?
                    MediaItemListViewModelRes.QuestionKeepUploadPhotoMessage :
                    MediaItemListViewModelRes.QuestionKeepUploadVideoMessage;
                var confirmButton = model.MediaType == EnumMediaType.Picture ?
                    MediaItemListViewModelRes.QuestionKeepUploadPhotoButtonConfirm :
                    MediaItemListViewModelRes.QuestionKeepUploadVideoButtonConfirm;
                var cancelButton = model.MediaType == EnumMediaType.Picture ?
                    MediaItemListViewModelRes.QuestionKeepUploadPhotoButtonCancel :
                    MediaItemListViewModelRes.QuestionKeepUploadVideoButtonCancel;
                var resultKeepUploadQuestion = await Question(title, message, confirmButton, cancelButton, false, false);
                keepUploadItem = resultKeepUploadQuestion.HasValue && resultKeepUploadQuestion.Value;
            }
            else
            {
                keepUploadItem = userSettings?.DefaultKeepUploadWhenDelete is not null && userSettings.DefaultKeepUploadWhenDelete.Value;
            }

            IsSaving = true;

            await _mediaItemDataService.DeleteMediaItemAsync(model, keepUploadItem)
                .HandleSuccess(() =>
                {
                    var itemToRemove = MediaItems.SingleOrDefault(x => x.Id == model.Id);
                    if (itemToRemove is not null)
                    {
                        MediaItems.Remove(itemToRemove);
                    }

                    ShowSuccessToast(string.Format(MediaItemListViewModelRes.ToastDeleteSuccess,
                        model.MediaType == EnumMediaType.Picture ? model.UploadPicture?.Filename : model.LocalizedName));

                    return Task.CompletedTask;
                })
                .HandleStatus(APIResultErrorType.NotAuthorized, () =>
                {
                    ShowErrorToast(string.Format(MediaItemListViewModelRes.ToastDeleteErrorNotAuthorized,
                        model.MediaType == EnumMediaType.Picture ? model.UploadPicture?.Filename : model.LocalizedName));

                    return Task.CompletedTask;
                })
                .HandleStatus(APIResultErrorType.NotFound, () =>
                {
                    ShowErrorToast(string.Format(MediaItemListViewModelRes.ToastDeleteErrorNotFound,
                        model.MediaType == EnumMediaType.Picture ? model.UploadPicture?.Filename : model.LocalizedName));

                    return Task.CompletedTask;
                })
                .HandleStatus(APIResultErrorType.CommunicationError, () =>
                {
                    ShowErrorToast(string.Format(MediaItemListViewModelRes.ToastDeleteErrorCommunication,
                        model.MediaType == EnumMediaType.Picture ? model.UploadPicture?.Filename : model.LocalizedName));

                    return Task.CompletedTask;
                });

            IsSaving = false;
        }
    }

    [RelayCommand]
    private void NavigateBack()
    {
        _navManager.NavigateTo<MediaListViewModel>();
    }
    #endregion

    #region Event-Handler
    private void MediaItems_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        NotifyStateChanged();
    }
    #endregion

    #region Event-Aggregator
    public async Task HandleAsync(AggMediaItemCreated message)
    {
        await _mediaItemDataService.GetMediaItemAsync(message.MediaItem.Id)
            .HandleSuccess((result) =>
            {
                if (result is not null)
                {
                    MediaItems.Add(result);
                }

                return Task.CompletedTask;
            })
            .HandleErrors((_) =>
            {
                ShowErrorToast(MediaItemListViewModelRes.ToastAddItemError);

                return Task.CompletedTask;
            });
    }

    public async Task HandleAsync(AggMediaItemChanged message)
    {
        await _mediaItemDataService.GetMediaItemAsync(message.MediaItem.Id)
            .HandleSuccess(result =>
            {
                if (result is not null)
                {
                    MediaItems.Replace(result, item => item.Id == result.Id);
                }

                return Task.CompletedTask;
            })
            .HandleErrors(_ =>
            {
                ShowErrorToast(MediaItemListViewModelRes.ToastReplaceItemError);

                return Task.CompletedTask;
            });
    }
    #endregion

    #region Abstract overrides
    public override void SetParameter(string name, object value)
    {
        if (name == nameof(MediaGroupId))
        {
            MediaGroupId = (long)value;
        }
    }

    public override void Dispose()
    {
        _eventAggregator.Unsubscribe(this);

        MediaItems.CollectionChanged -= MediaItems_CollectionChanged;
    }
    #endregion
}
