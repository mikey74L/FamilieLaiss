using CommunityToolkit.Mvvm.Input;
using EventAggregator.Blazor;
using FamilieLaissEnums;
using FamilieLaissInterfaces;
using FamilieLaissInterfaces.DataServices;
using FamilieLaissInterfaces.Enums;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using FamilieLaissModels.EventAggregator.CategoryValue;
using FamilieLaissModels.Models.CategoryValue;
using FamilieLaissResources.Resources.ViewModels.Dialogs.BaseData.CategoryValue;
using FamilieLaissServices.Extensions;
using FamilieLaissSharedUI.ViewModels;
using MudBlazor;

namespace FamilieLaissFrontend.Client.ViewModels.Dialogs.BaseData.CategoryValue;

public partial class CategoryValueEditDialogViewModel : ViewModelBase
{
    #region Services
    public readonly IValidatorFl<ICategoryValueModel> Validator;
    private ICategoryValueDataService categoryValueService;
    private IEventAggregator eventAggregator;
    #endregion

    #region Private Members
    private ICategoryValueModel? _oldModel;
    #endregion

    #region Parameters
    public MudDialogInstance MudDialog { get; set; } = default!;
    public bool IsInEditMode { get; set; }
    public ICategoryValueModel? Model { get; set; } = default!;
    public ICategoryModel ModelCategory { get; set; } = default!;
    #endregion

    #region Public Properties
    public MudForm? Form { get; set; }
    #endregion

    #region C'tor
    public CategoryValueEditDialogViewModel(ISnackbar snackbarService, IMessageBoxService messageBoxService,
        IValidatorFl<ICategoryValueModel> validator, ICategoryValueDataService categoryValueService,
        IEventAggregator eventAggregator) : base(snackbarService, messageBoxService)
    {
        Validator = validator;
        this.categoryValueService = categoryValueService;
        this.eventAggregator = eventAggregator;
    }
    #endregion

    #region Lifecycle
    public override async Task OnInitializedAsync()
    {
        if (!IsInEditMode)
        {
            Model = new CategoryValueModel
            {
                CategoryId = ModelCategory.Id
            };
        }
        else
        {
            if (Model is not null)
            {
                IsLoading = true;

                _oldModel = Model;

                await categoryValueService.GetCategoryValueAsync(Model.Id)
                    .HandleStatus(APIResultErrorType.NoError, (result) =>
                    {
                        Model = result;

                        return Task.CompletedTask;
                    })
                    .HandleErrors((result) =>
                    {
                        ShowErrorToast(CategoryValueEditDialogViewModelRes.ToastInitializeError);

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
                    await categoryValueService.AddCategoryValueAsync(Model)
                        .HandleStatus(APIResultErrorType.NoError, async () =>
                        {
                            ShowSuccessToast(string.Format(
                                CategoryValueEditDialogViewModelRes.ToastSaveAddSuccess, Model.LocalizedName));

                            await eventAggregator.PublishAsync(new AggCategoryValueCreated() { CategoryValue = Model });
                        })
                        .HandleStatus(APIResultErrorType.NotAuthorized, () =>
                        {
                            ShowErrorToast(string.Format(
                                CategoryValueEditDialogViewModelRes.ToastSaveAddErrorNotAuthorized,
                                Model.LocalizedName));

                            return Task.CompletedTask;
                        })
                        .HandleStatus(APIResultErrorType.Conflict, () =>
                        {
                            ShowErrorToast(string.Format(
                                CategoryValueEditDialogViewModelRes.ToastSaveAddErrorConflict,
                                Model.LocalizedName));

                            return Task.CompletedTask;
                        })
                        .HandleStatus(APIResultErrorType.ServerError, () =>
                        {
                            ShowErrorToast(string.Format(
                                CategoryValueEditDialogViewModelRes.ToastSaveAddErrorServer,
                                Model.LocalizedName));

                            return Task.CompletedTask;
                        })
                        .HandleStatus(APIResultErrorType.CommunicationError, () =>
                        {
                            ShowErrorToast(string.Format(
                                CategoryValueEditDialogViewModelRes.ToastSaveAddErrorCommunication,
                                Model.LocalizedName));

                            return Task.CompletedTask;
                        });
                }
                else
                {
                    await categoryValueService.UpdateCategoryValueAsync(Model)
                        .HandleStatus(APIResultErrorType.NoError, async () =>
                        {
                            ShowSuccessToast(string.Format(
                                CategoryValueEditDialogViewModelRes.ToastSaveEditSuccess,
                                Model.LocalizedName));

                            if (_oldModel is not null)
                            {
                                _oldModel.TakeOverValues(Model);

                                await eventAggregator.PublishAsync(new AggCategoryValueChanged()
                                { CategoryValue = _oldModel });
                            }
                        })
                        .HandleStatus(APIResultErrorType.NotAuthorized, () =>
                        {
                            ShowErrorToast(string.Format(
                                CategoryValueEditDialogViewModelRes.ToastSaveEditErrorNotAuthorized,
                                Model.LocalizedName));

                            return Task.CompletedTask;
                        })
                        .HandleStatus(APIResultErrorType.NotFound, () =>
                        {
                            ShowErrorToast(string.Format(
                                CategoryValueEditDialogViewModelRes.ToastSaveEditErrorNotFound,
                                Model.LocalizedName));

                            return Task.CompletedTask;
                        })
                        .HandleStatus(APIResultErrorType.Conflict, () =>
                        {
                            ShowErrorToast(string.Format(
                                CategoryValueEditDialogViewModelRes.ToastSaveEditErrorConflict,
                                Model.LocalizedName));

                            return Task.CompletedTask;
                        })
                        .HandleStatus(APIResultErrorType.ServerError, () =>
                        {
                            ShowErrorToast(string.Format(
                                CategoryValueEditDialogViewModelRes.ToastSaveEditErrorServer,
                                Model.LocalizedName));

                            return Task.CompletedTask;
                        })
                        .HandleStatus(APIResultErrorType.CommunicationError, () =>
                        {
                            ShowErrorToast(string.Format(
                                CategoryValueEditDialogViewModelRes.ToastSaveEditErrorCommunication,
                                Model.LocalizedName));

                            return Task.CompletedTask;
                        });
                }

                if (stayInDialog)
                {
                    Model = new CategoryValueModel
                    {
                        CategoryId = ModelCategory.Id
                    };
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
                ? CategoryValueEditDialogViewModelRes.AlertCancelAddTitle
                : CategoryValueEditDialogViewModelRes.AlertCancelEditTitle;
            string message = !IsInEditMode
                ? CategoryValueEditDialogViewModelRes.AlertCancelAddMessage
                : CategoryValueEditDialogViewModelRes.AlertCancelEditMessage;
            string buttonCancel = !IsInEditMode
                ? CategoryValueEditDialogViewModelRes.AlertCancelAddCancel
                : CategoryValueEditDialogViewModelRes.AlertCancelEditCancel;
            string buttonConfirm = !IsInEditMode
                ? CategoryValueEditDialogViewModelRes.AlertCancelAddConfirm
                : CategoryValueEditDialogViewModelRes.AlertCancelEditConfirm;

            var dialogResult = await QuestionConfirmRed(title, message, buttonConfirm, buttonCancel);

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
