using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EventAggregator.Blazor;
using FamilieLaissFrontend.Client.Dialogs.BaseData.Category;
using FamilieLaissFrontend.Client.ViewModels.Pages.BaseData.CategoryValue;
using FamilieLaissInterfaces.DataServices;
using FamilieLaissInterfaces.Enums;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using FamilieLaissModels.EventAggregator.Category;
using FamilieLaissResources.Resources.ViewModels.Pages.BaseData.Category;
using FamilieLaissServices.Extensions;
using FamilieLaissSharedUI.Helper;
using FamilieLaissSharedUI.Interfaces;
using FamilieLaissSharedUI.ViewModels;
using MudBlazor;

namespace FamilieLaissFrontend.Client.ViewModels.Pages.BaseData.Category;

public partial class CategoryListViewModel(
    ICategoryDataService categoryService,
    IDialogService dialogService,
    IEventAggregator eventAggregator,
    IMvvmNavigationManager navigationManager,
    ISnackbar snackbarService,
    IMessageBoxService messageBoxService)
    : ViewModelBase(snackbarService, messageBoxService), IHandle<AggCategoryCreated>, IHandle<AggCategoryChanged>
{
    #region Public Properties
    [ObservableProperty]
    // ReSharper disable once InconsistentNaming
    public ExtendedObservableCollection<ICategoryModel> _items = [];

    public string SearchString { get; set; } = string.Empty;
    #endregion

    #region Public Methods
    public Func<ICategoryModel, bool> QuickFilter => x =>
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

    public readonly Func<ICategoryModel, object> GroupByFunction = (x) => x.CategoryTypeText;
    #endregion

    #region Commands
    [RelayCommand]
    private void NavigateToValues(ICategoryModel model)
    {
        navigationManager.NavigateTo<CategoryValueListViewModel>($"?{nameof(ICategoryValueModel.CategoryId)}={model.Id}");
    }

    [RelayCommand]
    private async Task RefreshDataAsync()
    {
        IsLoading = true;

        await categoryService.GetAllCategoriesAsync()
            .HandleStatus(APIResultErrorType.NoError, (result) =>
            {
                Items.Clear();
                Items.AddRange(result);

                return Task.CompletedTask;
            })
            .HandleStatus(APIResultErrorType.NotAuthorized, (_) =>
            {
                ShowErrorToast(CategoryListViewModelRes.ToastLoadingErrorNotAuthorized);

                return Task.CompletedTask;
            })
            .HandleStatus(APIResultErrorType.ServerError, (_) =>
            {
                ShowErrorToast(CategoryListViewModelRes.ToastLoadingErrorServerError);

                return Task.CompletedTask;
            })
            .HandleStatus(APIResultErrorType.CommunicationError, (_) =>
            {
                ShowErrorToast(CategoryListViewModelRes.ToastLoadingErrorCommunication);

                return Task.CompletedTask;
            });

        IsLoading = false;
    }

    [RelayCommand]
    private async Task AddCategoryAsync()
    {
        DialogParameters dialogParam = new() { { "IsInEditMode", false } };

        var dialogRef = await dialogService.ShowAsync<CategoryEditDialog>(
            CategoryListViewModelRes.DialogTitleAdd, dialogParam, GetDialogOptions());

        await dialogRef.Result;
    }

    [RelayCommand]
    private async Task EditCategoryAsync(ICategoryModel model)
    {
        DialogParameters dialogParam = new()
        {
            { "IsInEditMode", true },
            { "Model", model }
        };

        var dialogRef = await dialogService.ShowAsync<CategoryEditDialog>(
            CategoryListViewModelRes.DialogTitleEdit, dialogParam, GetDialogOptions());

        await dialogRef.Result;
    }

    [RelayCommand]
    private async Task DeleteCategoryAsync(ICategoryModel model)
    {
        var result = await QuestionConfirmRed(
            CategoryListViewModelRes.QuestionDeleteTitle,
            string.Format(CategoryListViewModelRes.QuestionDeleteMessage, model.LocalizedName),
            CategoryListViewModelRes.QuestionDeleteButtonConfirm,
            CategoryListViewModelRes.QuestionDeleteButtonCancel);

        if (result.HasValue && result.Value)
        {
            IsSaving = true;

            await categoryService.DeleteCategoryAsync(model)
                .HandleSuccess(() =>
                {
                    var itemToRemove = Items.SingleOrDefault(x => x.Id == model.Id);

                    if (itemToRemove is not null)
                    {
                        Items.Remove(itemToRemove);
                    }

                    ShowSuccessToast(string.Format(CategoryListViewModelRes.ToastDeleteSuccess,
                        model.LocalizedName));

                    return Task.CompletedTask;
                })
                .HandleStatus(APIResultErrorType.NotAuthorized, () =>
                {
                    ShowErrorToast(string.Format(CategoryListViewModelRes.ToastDeleteErrorNotAuthorized,
                        model.LocalizedName));

                    return Task.CompletedTask;
                })
                .HandleStatus(APIResultErrorType.NotFound, () =>
                {
                    ShowErrorToast(string.Format(CategoryListViewModelRes.ToastDeleteErrorNotFound,
                        model.LocalizedName));

                    return Task.CompletedTask;
                })
                .HandleStatus(APIResultErrorType.CommunicationError, () =>
                {
                    ShowErrorToast(string.Format(CategoryListViewModelRes.ToastDeleteErrorCommunication,
                        model.LocalizedName));

                    return Task.CompletedTask;
                });

            IsSaving = false;
        }
    }
    #endregion

    #region Lifecycle Override
    public override void OnInitialized()
    {
        base.OnInitialized();

        eventAggregator.Subscribe(this);
    }

    public override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        await RefreshDataAsync();
    }
    #endregion

    #region Event-Aggregator
    public Task HandleAsync(AggCategoryCreated message)
    {
        var newModel = message.Category;

        Items.Add(newModel);

        NotifyStateChanged();

        return Task.CompletedTask;
    }

    public Task HandleAsync(AggCategoryChanged message)
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
