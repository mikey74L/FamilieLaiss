using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EventAggregator.Blazor;
using FamilieLaissFrontend.Client.Dialogs.BaseData.MediaGroup;
using FamilieLaissInterfaces.DataServices;
using FamilieLaissInterfaces.Enums;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using FamilieLaissModels.EventAggregator.MediaGroup;
using FamilieLaissResources.Resources.ViewModels.Pages.BaseData.Media;
using FamilieLaissServices.Extensions;
using FamilieLaissSharedUI.Helper;
using FamilieLaissSharedUI.Interfaces;
using FamilieLaissSharedUI.ViewModels;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace FamilieLaissFrontend.Client.ViewModels.Pages.BaseData.Media;

public partial class MediaListViewModel : ViewModelBase, IHandle<AggMediaGroupCreated>, IHandle<AggMediaGroupChanged>
{
    #region Services
    private readonly IMediaGroupDataService mediaGroupService;
    private readonly IDialogService dialogService;
    private readonly IUserSettingsService userSettingsService;
    private readonly IEventAggregator eventAggregator;
    private readonly IMvvmNavigationManager navigationManager;
    #endregion

    #region Parameters
    public Task<AuthenticationState> AuthenticationState { get; set; } = default!;
    #endregion

    #region Public Properties
    [ObservableProperty]
    private ExtendedObservableCollection<IMediaGroupModel> _items = [];

    public string? SearchString { get; set; }
    public MudDataGrid<IMediaGroupModel>? DataGrid { get; set; }
    #endregion

    #region C'tor
    public MediaListViewModel(ISnackbar snackbarService, IMessageBoxService messageBoxService,
        IMediaGroupDataService mediaGroupService, IDialogService dialogService,
        IUserSettingsService userSettingsService, IEventAggregator eventAggregator,
        IMvvmNavigationManager navigationManager) : base(snackbarService, messageBoxService)
    {
        this.mediaGroupService = mediaGroupService;
        this.dialogService = dialogService;
        this.userSettingsService = userSettingsService;
        this.eventAggregator = eventAggregator;
        this.navigationManager = navigationManager;
    }
    #endregion

    #region Lifecycle
    public override void OnInitialized()
    {
        base.OnInitialized();

        eventAggregator.Subscribe(this);
    }

    public override async Task OnInitializedAsync()
    {
        await RefreshDataAsync();

        if (DataGrid is not null)
        {
            await DataGrid.SetSortAsync(nameof(IMediaGroupModel.EventDateForInput), SortDirection.Descending, x => x.EventDateForInput, new MudBlazor.Utilities.NaturalComparer());
        }
    }
    #endregion

    #region Public Methods
    public Func<IMediaGroupModel, bool> QuickFilter => x =>
    {
        if (string.IsNullOrWhiteSpace(SearchString))
        {
            return true;
        }

        if (x.NameGerman is not null && x.NameGerman.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        if (x.NameEnglish is not null && x.NameEnglish.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return false;
    };
    #endregion

    #region Commands
    [RelayCommand]
    private async Task AddMediaGroupAsync()
    {
        DialogParameters dialogParam = new() { { "IsInEditMode", false } };

        var dialogRef = await dialogService.ShowAsync<MediaGroupEditDialog>(
            MediaListViewModelRes.DialogTitleAdd, dialogParam, GetDialogOptions());
        await dialogRef.Result;
    }

    [RelayCommand]
    private async Task EditMediaGroupAsync(IMediaGroupModel model)
    {
        DialogParameters dialogParam = new()
        {
            { "IsInEditMode", true },
            { "Model", model }
        };

        var dialogRef = await dialogService.ShowAsync<MediaGroupEditDialog>(
            MediaListViewModelRes.DialogTitleEdit, dialogParam, GetDialogOptions());

        await dialogRef.Result;
    }

    [RelayCommand]
    private async Task DeleteMediaGroupAsync(IMediaGroupModel model)
    {
        var result = await QuestionConfirmRed(
            MediaListViewModelRes.QuestionDeleteTitle,
            string.Format(MediaListViewModelRes.QuestionDeleteMessage, model.LocalizedName),
            MediaListViewModelRes.QuestionDeleteButtonConfirm,
            MediaListViewModelRes.QuestionDeleteButtonCancel);
        if (result.HasValue && result.Value)
        {
            bool keepUploadItems = false;
            var userSettings = await userSettingsService.GetCurrentUserSettings(AuthenticationState);
            if (userSettings?.QuestionKeepUploadWhenDelete is not null && userSettings.QuestionKeepUploadWhenDelete.Value)
            {
                var resultKeepUploadQuestion = await Question(
                    MediaListViewModelRes.QuestionKeepUploadTitle,
                    MediaListViewModelRes.QuestionKeepUploadMessage,
                    MediaListViewModelRes.QuestionKeepUploadButtonConfirm,
                    MediaListViewModelRes.QuestionKeepUploadButtonCancel,
                    false,
                    false);
                if (resultKeepUploadQuestion.HasValue)
                {
                    keepUploadItems = resultKeepUploadQuestion.Value;
                }
                else
                {
                    keepUploadItems = false;
                }
            }
            else
            {
                keepUploadItems = userSettings?.DefaultKeepUploadWhenDelete is not null && userSettings.DefaultKeepUploadWhenDelete.Value;
            }

            IsSaving = true;

            await mediaGroupService.DeleteMediaGroupAsync(model, keepUploadItems)
                .HandleSuccess(() =>
                {
                    var itemToRemove = Items.SingleOrDefault(x => x.Id == model.Id);

                    if (itemToRemove is not null)
                    {
                        Items.Remove(itemToRemove);
                    }

                    ShowSuccessToast(string.Format(MediaListViewModelRes.ToastDeleteSuccess,
                        model.LocalizedName));

                    return Task.CompletedTask;
                })
                .HandleStatus(APIResultErrorType.NotAuthorized, () =>
                {
                    ShowErrorToast(string.Format(MediaListViewModelRes.ToastDeleteErrorNotAuthorized,
                        model.LocalizedName));

                    return Task.CompletedTask;
                })
                .HandleStatus(APIResultErrorType.NotFound, () =>
                {
                    ShowErrorToast(string.Format(MediaListViewModelRes.ToastDeleteErrorNotFound,
                        model.LocalizedName));

                    return Task.CompletedTask;
                })
                .HandleStatus(APIResultErrorType.CommunicationError, () =>
                {
                    ShowErrorToast(string.Format(MediaListViewModelRes.ToastDeleteErrorCommunication,
                        model.LocalizedName));

                    return Task.CompletedTask;
                });

            IsSaving = false;
        }
    }

    [RelayCommand]
    private async Task RefreshDataAsync()
    {
        IsLoading = true;

        await mediaGroupService.GetAllMediaGroupsAsync()
            .HandleStatus(APIResultErrorType.NoError, (result) =>
            {
                Items.Clear();
                Items.AddRange(result);

                return Task.CompletedTask;
            })
            .HandleStatus(APIResultErrorType.NotAuthorized, (_) =>
            {
                ShowErrorToast(MediaListViewModelRes.ToastLoadingErrorNotAuthorized);

                return Task.CompletedTask;
            })
            .HandleStatus(APIResultErrorType.ServerError, (_) =>
            {
                ShowErrorToast(MediaListViewModelRes.ToastLoadingErrorServerError);

                return Task.CompletedTask;
            })
            .HandleStatus(APIResultErrorType.CommunicationError, (_) =>
            {
                ShowErrorToast(MediaListViewModelRes.ToastLoadingErrorCommunication);

                return Task.CompletedTask;
            });

        IsLoading = false;
    }

    [RelayCommand]
    private void NavigateToItems(IMediaGroupModel mediaGroup)
    {
        navigationManager.NavigateTo<MediaItemListViewModel>($"?{nameof(IMediaItemModel.MediaGroupId)}={mediaGroup.Id}");
    }
    #endregion

    #region EventAggregator
    public Task HandleAsync(AggMediaGroupCreated message)
    {
        var newModel = message.MediaGroup;

        Items.Add(newModel);

        NotifyStateChanged();

        return Task.CompletedTask;
    }

    public Task HandleAsync(AggMediaGroupChanged message)
    {
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
