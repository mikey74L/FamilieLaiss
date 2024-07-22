using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EventAggregator.Blazor;
using FamilieLaissFrontend.Client.Dialogs.BaseData.CategoryValue;
using FamilieLaissFrontend.Client.ViewModels.Pages.BaseData.Category;
using FamilieLaissInterfaces.DataServices;
using FamilieLaissInterfaces.Enums;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using FamilieLaissModels.EventAggregator.CategoryValue;
using FamilieLaissResources.Resources.ViewModels.Pages.BaseData.CategoryValue;
using FamilieLaissServices.Extensions;
using FamilieLaissSharedUI.Helper;
using FamilieLaissSharedUI.Interfaces;
using FamilieLaissSharedUI.ViewModels;
using MudBlazor;
using System.ComponentModel;

namespace FamilieLaissFrontend.Client.ViewModels.Pages.BaseData.CategoryValue;

public partial class CategoryValueListViewModel : ViewModelBase, IHandle<AggCategoryValueCreated>, IHandle<AggCategoryValueChanged>
{
    #region Private Services
    private readonly ICategoryDataService _categoryService;
    private readonly ICategoryValueDataService _categoryValueService;
    private readonly IDialogService _dialogService;
    private readonly IEventAggregator _eventAggregator;
    private readonly IMvvmNavigationManager _navigationManager;
    #endregion

    #region Query Parameters
    public long? CategoryId { get; set; }
    #endregion

    #region Public Properties
    [ObservableProperty]
    private ExtendedObservableCollection<ICategoryValueModel> _items = [];

    [ObservableProperty]
    private SortableObservableCollection<ICategoryModel> _categories = [];

    public string SearchString { get; set; } = string.Empty;

    [ObservableProperty]
    private ICategoryModel? _category;

    [ObservableProperty]
    private ICategoryModel? _selectedCategory;
    #endregion

    #region C'tor
    public CategoryValueListViewModel(ICategoryDataService categoryService, ICategoryValueDataService categoryValueService,
        IDialogService dialogService, IEventAggregator eventAggregator,
        IMvvmNavigationManager navigationManager, ISnackbar snackbarService, IMessageBoxService messageBoxService)
        : base(snackbarService, messageBoxService)
    {
        this.QueryStringParameters.Add(nameof(CategoryId), typeof(long));

        this._categoryService = categoryService;
        this._categoryValueService = categoryValueService;
        this._dialogService = dialogService;
        this._eventAggregator = eventAggregator;
        this._navigationManager = navigationManager;
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

        await _categoryService.GetAllCategoriesAsync()
            .HandleSuccess(async (result) =>
            {
                Categories.AddRange(result);

                Categories.Sort(x => x.LocalizedName, ListSortDirection.Ascending);

                await RefreshDataAsync(false);
            })
            .HandleErrors((_) =>
            {
                ShowErrorToast(CategoryValueListViewModelRes.ToastInitializeError);

                return Task.CompletedTask;
            });

        IsLoading = false;
    }
    #endregion

    #region Public Methods
    public Func<ICategoryValueModel, bool> QuickFilter => x =>
    {
        if (string.IsNullOrWhiteSpace(SearchString))
        {
            return true;
        }

        if (x.NameGerman!.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        if (x.NameEnglish!.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return false;
    };
    #endregion

    #region Commands
    [RelayCommand]
    private async Task SetSelectedCategoryAsync(ICategoryModel? selectedModel)
    {
        if (selectedModel is not null && SelectedCategory?.Id != selectedModel.Id)
        {
            CategoryId = selectedModel.Id;

            await RefreshDataAsync(true);
        }
    }

    [RelayCommand]
    private async Task AddCategoryValueAsync()
    {
        DialogParameters dialogParam = new()
        {
            { "IsInEditMode", false },
            { "ModelCategory", Category }
        };

        var dialogRef = await _dialogService.ShowAsync<CategoryValueEditDialog>(
            CategoryValueListViewModelRes.DialogTitleAdd, dialogParam, GetDialogOptions());

        await dialogRef.Result;
    }

    [RelayCommand]
    private async Task EditCategoryValueAsync(ICategoryValueModel model)
    {
        DialogParameters dialogParam = new()
        {
            { "IsInEditMode", true },
            { "Model", model },
            { "ModelCategory", Category }
        };

        var dialogRef = await _dialogService.ShowAsync<CategoryValueEditDialog>(
            CategoryValueListViewModelRes.DialogTitleEdit, dialogParam, GetDialogOptions());
        await dialogRef.Result;
    }

    [RelayCommand]
    private async Task DeleteCategoryValueAsync(ICategoryValueModel model)
    {
        var result = await QuestionConfirmRed(
            CategoryValueListViewModelRes.QuestionDeleteTitle,
            string.Format(CategoryValueListViewModelRes.QuestionDeleteMessage, model.LocalizedName),
            CategoryValueListViewModelRes.QuestionDeleteButtonConfirm,
            CategoryValueListViewModelRes.QuestionDeleteButtonCancel);

        if (result.HasValue && result.Value)
        {
            IsSaving = true;

            await _categoryValueService.DeleteCategoryValueAsync(model)
                .HandleSuccess(() =>
                {
                    var itemToRemove = Items.SingleOrDefault(x => x.Id == model.Id);

                    if (itemToRemove is not null)
                    {
                        Items.Remove(itemToRemove);
                    }

                    ShowSuccessToast(string.Format(CategoryValueListViewModelRes.ToastDeleteSuccess,
                        model.LocalizedName));

                    return Task.CompletedTask;
                })
                .HandleStatus(APIResultErrorType.NotAuthorized, () =>
                {
                    ShowErrorToast(string.Format(CategoryValueListViewModelRes.ToastDeleteErrorNotAuthorized,
                        model.LocalizedName));

                    return Task.CompletedTask;
                })
                .HandleStatus(APIResultErrorType.NotFound, () =>
                {
                    ShowErrorToast(string.Format(CategoryValueListViewModelRes.ToastDeleteErrorNotFound,
                        model.LocalizedName));

                    return Task.CompletedTask;
                })
                .HandleStatus(APIResultErrorType.CommunicationError, () =>
                {
                    ShowErrorToast(string.Format(CategoryValueListViewModelRes.ToastDeleteErrorCommunication,
                        model.LocalizedName));

                    return Task.CompletedTask;
                });

            IsSaving = false;
        }
    }

    [RelayCommand]
    private async Task RefreshDataAsync(bool calledDirectly)
    {
        if (calledDirectly)
        {
            IsLoading = true;
        }

        if (CategoryId is not null)
        {
            await _categoryValueService.GetCategoryValuesForCategoryAsync(CategoryId.Value)
                .HandleStatus(APIResultErrorType.NoError, (result) =>
                {
                    Items.Clear();
                    Items.AddRange(result?.CategoryValues);

                    Category = result;

                    if (Category is not null)
                    {
                        SelectedCategory = Categories.SingleOrDefault(x => x.Id == Category.Id);
                    }

                    return Task.CompletedTask;
                })
                .HandleStatus(APIResultErrorType.NotAuthorized, (_) =>
                {
                    ShowErrorToast(CategoryValueListViewModelRes.ToastLoadingErrorNotAuthorized);

                    return Task.CompletedTask;
                })
                .HandleStatus(APIResultErrorType.ServerError, (_) =>
                {
                    ShowErrorToast(CategoryValueListViewModelRes.ToastLoadingErrorServerError);

                    return Task.CompletedTask;
                })
                .HandleStatus(APIResultErrorType.CommunicationError, (_) =>
                {
                    ShowErrorToast(CategoryValueListViewModelRes.ToastLoadingErrorCommunication);

                    return Task.CompletedTask;
                });
        }

        if (calledDirectly)
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void JumpBackToCategory()
    {
        _navigationManager.NavigateTo<CategoryListViewModel>();
    }
    #endregion

    #region Event-Aggregator
    public Task HandleAsync(AggCategoryValueCreated message)
    {
        var newModel = message.CategoryValue;

        Items.Add(newModel);

        NotifyStateChanged();

        return Task.CompletedTask;
    }

    public Task HandleAsync(AggCategoryValueChanged message)
    {
        NotifyStateChanged();

        return Task.CompletedTask;
    }
    #endregion

    #region Abstract overrides
    public override void SetParameter(string name, object value)
    {
        if (name == nameof(CategoryId))
        {
            CategoryId = (long)value;
        }
    }

    public override void Dispose()
    {
        _eventAggregator.Unsubscribe(this);
    }
    #endregion
}
