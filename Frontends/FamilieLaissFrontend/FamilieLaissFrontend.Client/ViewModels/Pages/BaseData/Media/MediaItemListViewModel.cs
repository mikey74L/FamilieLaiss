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
    private readonly IEventAggregator eventAggregator;
    private readonly IMediaGroupDataService mediaGroupService;
    private readonly IMediaItemDataService mediaItemDataService;
    private readonly IUserSettingsService userSettingsService;
    private readonly IDialogService dialogService;
    private readonly IMvvmNavigationManager navManager;
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

        this.eventAggregator = eventAggregator;
        this.mediaGroupService = mediaGroupDataService;
        this.mediaItemDataService = mediaItemDataService;
        this.userSettingsService = userSettingsService;
        this.dialogService = dialogService;
        this.navManager = navManager;

        this.MediaItems.CollectionChanged += MediaItems_CollectionChanged;
    }
    #endregion

    #region Lifecycle overrides
    public override void OnInitialized()
    {
        base.OnInitialized();

        eventAggregator.Subscribe(this);
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
        await mediaGroupService.GetMediaGroupAsync(MediaGroupId)
            .HandleSuccess((result) =>
            {
                MediaGroup = result;

                return Task.CompletedTask;
            })
            .HandleErrors((result) =>
            {
                ShowErrorToast(MediaItemListViewModelRes.ToastInitializeError);

                return Task.CompletedTask;
            });
    }

    private async Task LoadMediaItems()
    {
        if (MediaGroup is not null)
        {
            await mediaItemDataService.GetMediaItemsForGroupAsync(MediaGroup)
                .HandleSuccess((result) =>
                {
                    MediaItems.Clear();
                    MediaItems.AddRange(result);

                    return Task.CompletedTask;
                })
                .HandleErrors((result) =>
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

        var dialogRef = await dialogService.ShowAsync<MediaItemEditDialog>(
            MediaItemListViewModelRes.DialogTitleAdd, dialogParam, GetDialogOptions());
        await dialogRef.Result;
    }

    [RelayCommand]
    private async Task EditItem(IMediaItemModel model)
    {
        DialogParameters dialogParam = new() { { "IsInEditMode", true }, { "Model", model }, { "MediaGroupId", MediaGroupId } };

        var dialogRef = await dialogService.ShowAsync<MediaItemEditDialog>(
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
            bool keepUploadItem = false;
            var userSettings = await userSettingsService.GetCurrentUserSettings(AuthenticationState);
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
                if (resultKeepUploadQuestion.HasValue)
                {
                    keepUploadItem = resultKeepUploadQuestion.Value;
                }
                else
                {
                    keepUploadItem = false;
                }
            }
            else
            {
                keepUploadItem = userSettings?.DefaultKeepUploadWhenDelete is not null && userSettings.DefaultKeepUploadWhenDelete.Value;
            }

            IsSaving = true;

            await mediaItemDataService.DeleteMediaItemAsync(model, keepUploadItem)
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
        navManager.NavigateTo<MediaListViewModel>();
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
        await mediaItemDataService.GetMediaItemAsync(message.MediaItem.Id)
            .HandleSuccess((result) =>
            {
                if (result is not null)
                {
                    MediaItems.Add(result);
                }

                return Task.CompletedTask;
            })
            .HandleErrors((result) =>
            {
                ShowErrorToast(MediaItemListViewModelRes.ToastAddItemError);

                return Task.CompletedTask;
            });
    }

    public async Task HandleAsync(AggMediaItemChanged message)
    {
        await mediaItemDataService.GetMediaItemAsync(message.MediaItem.Id)
            .HandleSuccess((result) =>
            {
                if (result is not null)
                {
                    MediaItems.Replace(result, (IMediaItemModel item) => item.Id == result.Id);
                }

                return Task.CompletedTask;
            })
            .HandleErrors((result) =>
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
        eventAggregator.Unsubscribe(this);

        MediaItems.CollectionChanged -= MediaItems_CollectionChanged;
    }
    #endregion
}
