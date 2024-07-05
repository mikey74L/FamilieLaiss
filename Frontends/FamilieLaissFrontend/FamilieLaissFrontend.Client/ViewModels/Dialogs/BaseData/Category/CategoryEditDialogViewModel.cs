using CommunityToolkit.Mvvm.Input;
using EventAggregator.Blazor;
using FamilieLaissEnums;
using FamilieLaissInterfaces;
using FamilieLaissInterfaces.DataServices;
using FamilieLaissInterfaces.Enums;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using FamilieLaissModels.EventAggregator.Category;
using FamilieLaissModels.Models.Category;
using FamilieLaissResources.Resources.ViewModels.Dialogs.BaseData.Category;
using FamilieLaissServices.Extensions;
using FamilieLaissSharedUI.ViewModels;
using MudBlazor;


namespace FamilieLaissFrontend.Client.ViewModels.Dialogs.BaseData.Category;

public partial class CategoryEditDialogViewModel : ViewModelBase
{
    #region Services
    public readonly IValidatorFl<ICategoryModel> Validator;
    private readonly ICategoryDataService categoryService;
    private readonly IEventAggregator eventAggregator;
    #endregion

    #region Private Members
    private ICategoryModel? _oldModel;
    #endregion

    #region Parameters
    public MudDialogInstance MudDialog { get; set; } = default!;
    public bool IsInEditMode { get; set; }
    public ICategoryModel? Model { get; set; } = default!;
    #endregion

    #region Public Properties
    public MudForm? Form { get; set; }
    #endregion

    #region C'tor
    public CategoryEditDialogViewModel(ISnackbar snackbarService, IMessageBoxService messageBoxService,
        IValidatorFl<ICategoryModel> validator, ICategoryDataService categoryService,
        IEventAggregator eventAggregator) : base(snackbarService, messageBoxService)
    {
        this.Validator = validator;
        this.categoryService = categoryService;
        this.eventAggregator = eventAggregator;
    }
    #endregion

    #region Lifecycle
    public override async Task OnInitializedAsync()
    {
        if (!IsInEditMode)
        {
            Model = new CategoryModel();
        }
        else
        {
            if (Model is not null)
            {
                IsLoading = true;

                _oldModel = Model;

                await categoryService.GetCategoryAsync(Model.Id)
                    .HandleStatus(APIResultErrorType.NoError, (result) =>
                    {
                        Model = result;

                        return Task.CompletedTask;
                    })
                    .HandleErrors((result) =>
                    {
                        ShowErrorToast(CategoryEditDialogViewModelRes.ToastInitializeError);

                        MudDialog.Close();

                        return Task.CompletedTask;
                    });

                IsLoading = false;
            }
        }
    }
    #endregion

    #region Commands
    [RelayCommand]
    public async Task SaveAsync(bool stayInDialog)
    {
        IsSaving = true;

        if (Form is not null && Model is not null)
        {
            await Form.Validate();

            if (Form.IsValid)
            {
                if (!IsInEditMode)
                {
                    await categoryService.AddCategoryAsync(Model)
                        .HandleStatus(APIResultErrorType.NoError, async () =>
                        {
                            ShowSuccessToast(string.Format(CategoryEditDialogViewModelRes.ToastSaveAddSuccess,
                                Model.LocalizedName));

                            await eventAggregator.PublishAsync(new AggCategoryCreated() { Category = Model });
                        })
                        .HandleStatus(APIResultErrorType.NotAuthorized, () =>
                        {
                            ShowErrorToast(string.Format(
                                CategoryEditDialogViewModelRes.ToastSaveAddErrorNotAuthorized,
                                Model.LocalizedName));

                            return Task.CompletedTask;
                        })
                        .HandleStatus(APIResultErrorType.Conflict, () =>
                        {
                            ShowErrorToast(string.Format(
                                CategoryEditDialogViewModelRes.ToastSaveAddErrorConflict,
                                Model.LocalizedName));

                            return Task.CompletedTask;
                        })
                        .HandleStatus(APIResultErrorType.ServerError, () =>
                        {
                            ShowErrorToast(string.Format(
                                CategoryEditDialogViewModelRes.ToastSaveAddErrorServer, Model.LocalizedName));

                            return Task.CompletedTask;
                        })
                        .HandleStatus(APIResultErrorType.CommunicationError, () =>
                        {
                            ShowErrorToast(string.Format(
                                CategoryEditDialogViewModelRes.ToastSaveAddErrorCommunication,
                                Model.LocalizedName));

                            return Task.CompletedTask;
                        });
                }
                else
                {
                    await categoryService.UpdateCategoryAsync(Model)
                        .HandleStatus(APIResultErrorType.NoError, async () =>
                        {
                            ShowSuccessToast(string.Format(CategoryEditDialogViewModelRes.ToastSaveEditSuccess,
                                Model.LocalizedName));

                            if (_oldModel is not null)
                            {
                                _oldModel.TakeOverValues(Model);

                                await eventAggregator.PublishAsync(new AggCategoryChanged() { Category = _oldModel });
                            }
                        })
                        .HandleStatus(APIResultErrorType.NotAuthorized, () =>
                        {
                            ShowErrorToast(string.Format(
                                CategoryEditDialogViewModelRes.ToastSaveEditErrorNotAuthorized,
                                Model.LocalizedName));

                            return Task.CompletedTask;
                        })
                        .HandleStatus(APIResultErrorType.NotFound, () =>
                        {
                            ShowErrorToast(string.Format(
                                CategoryEditDialogViewModelRes.ToastSaveEditErrorNotFound,
                                Model.LocalizedName));

                            return Task.CompletedTask;
                        })
                        .HandleStatus(APIResultErrorType.Conflict, () =>
                        {
                            ShowErrorToast(string.Format(
                                CategoryEditDialogViewModelRes.ToastSaveEditErrorConflict,
                                Model.LocalizedName));

                            return Task.CompletedTask;
                        })
                        .HandleStatus(APIResultErrorType.ServerError, () =>
                        {
                            ShowErrorToast(string.Format(
                                CategoryEditDialogViewModelRes.ToastSaveEditErrorServer,
                                Model.LocalizedName));

                            return Task.CompletedTask;
                        })
                        .HandleStatus(APIResultErrorType.CommunicationError, () =>
                        {
                            ShowErrorToast(string.Format(
                                CategoryEditDialogViewModelRes.ToastSaveEditErrorCommunication,
                                Model.LocalizedName));

                            return Task.CompletedTask;
                        });
                }

                if (stayInDialog)
                {
                    Model = new CategoryModel();
                }
                else
                {
                    MudDialog.Close();
                }
            }
        }

        SaveMode = stayInDialog ? EnumSaveMode.SaveAndContinue : EnumSaveMode.Save;

        IsSaving = false;
    }

    [RelayCommand]
    public async Task CancelAsync()
    {
        if (Form is { IsTouched: true })
        {
            string title = !IsInEditMode
                ? CategoryEditDialogViewModelRes.AlertCancelAddTitle
                : CategoryEditDialogViewModelRes.AlertCancelEditTitle;
            string message = !IsInEditMode
                ? CategoryEditDialogViewModelRes.AlertCancelAddMessage
                : CategoryEditDialogViewModelRes.AlertCancelEditMessage;
            string buttonCancel = !IsInEditMode
                ? CategoryEditDialogViewModelRes.AlertCancelAddCancel
                : CategoryEditDialogViewModelRes.AlertCancelEditCancel;
            string buttonConfirm = !IsInEditMode
                ? CategoryEditDialogViewModelRes.AlertCancelAddConfirm
                : CategoryEditDialogViewModelRes.AlertCancelEditConfirm;

            var dialogResult =
                await QuestionConfirmRed(title, message, buttonConfirm, buttonCancel, Icons.Material.Filled.Cancel);

            if (dialogResult.HasValue && dialogResult.Value)
            {
                MudDialog.Cancel();
            }
        }
        else
        {
            MudDialog.Cancel();
        }
    }
    #endregion

    #region Abstract overrides
    public override void Dispose()
    {
    }
    #endregion
}
