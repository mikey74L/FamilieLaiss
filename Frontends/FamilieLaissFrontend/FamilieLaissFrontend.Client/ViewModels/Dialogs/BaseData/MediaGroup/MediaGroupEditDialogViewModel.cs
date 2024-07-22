using CommunityToolkit.Mvvm.Input;
using EventAggregator.Blazor;
using FamilieLaissEnums;
using FamilieLaissInterfaces;
using FamilieLaissInterfaces.DataServices;
using FamilieLaissInterfaces.Enums;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using FamilieLaissModels.EventAggregator.MediaGroup;
using FamilieLaissModels.Models.MediaGroup;
using FamilieLaissResources.Resources.ViewModels.Dialogs.BaseData.MediaGroup;
using FamilieLaissServices.Extensions;
using FamilieLaissSharedUI.ViewModels;
using MudBlazor;

namespace FamilieLaissFrontend.Client.ViewModels.Dialogs.BaseData.MediaGroup;

public partial class MediaGroupEditDialogViewModel(
    ISnackbar snackbarService,
    IMessageBoxService messageBoxService,
    IValidatorFl<IMediaGroupModel> validator,
    IMediaGroupDataService mediaGroupService,
    IEventAggregator eventAggregator)
    : ViewModelBase(snackbarService, messageBoxService)
{
    #region Services
    public readonly IValidatorFl<IMediaGroupModel> Validator = validator;

    #endregion

    #region Private Fields
    private IMediaGroupModel? _oldModel;
    #endregion

    #region Parameters
    public MudDialogInstance? MudDialog { get; set; }
    public bool IsInEditMode { get; set; }
    public IMediaGroupModel? Model { get; set; }
    #endregion

    #region Public Properties
    public MudForm? Form { get; set; }
    public MudDatePicker? PickerEventDate { get; set; }
    #endregion

    #region Lifecycle
    public override async Task OnInitializedAsync()
    {
        if (!IsInEditMode)
        {
            Model = new MediaGroupModel();
        }
        else
        {
            if (Model is not null)
            {
                IsLoading = true;

                _oldModel = Model;

                await mediaGroupService.GetMediaGroupAsync(Model.Id)
                    .HandleStatus(APIResultErrorType.NoError, (result) =>
                    {
                        Model = result;

                        return Task.CompletedTask;
                    })
                .HandleErrors((_) =>
                {
                    ShowErrorToast(MediaGroupEditDialogViewModelRes.ToastInitializeError);

                    MudDialog?.Close();

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
                    await mediaGroupService.AddMediaGroupAsync(Model)
                        .HandleStatus(APIResultErrorType.NoError, async () =>
                        {
                            ShowSuccessToast(string.Format(MediaGroupEditDialogViewModelRes.ToastSaveAddSuccess,
                                Model.LocalizedName));

                            await eventAggregator.PublishAsync(new AggMediaGroupCreated() { MediaGroup = Model });
                        })
                        .HandleStatus(APIResultErrorType.NotAuthorized, () =>
                        {
                            ShowErrorToast(string.Format(
                                MediaGroupEditDialogViewModelRes.ToastSaveAddErrorNotAuthorized,
                            Model.LocalizedName));

                            return Task.CompletedTask;
                        })
                        .HandleStatus(APIResultErrorType.Conflict, () =>
                        {
                            ShowErrorToast(string.Format(
                                MediaGroupEditDialogViewModelRes.ToastSaveAddErrorConflict,
                            Model.LocalizedName));

                            return Task.CompletedTask;
                        })
                        .HandleStatus(APIResultErrorType.ServerError, () =>
                        {
                            ShowErrorToast(string.Format(
                                MediaGroupEditDialogViewModelRes.ToastSaveAddErrorServer, Model.LocalizedName));

                            return Task.CompletedTask;
                        })
                        .HandleStatus(APIResultErrorType.CommunicationError, () =>
                        {
                            ShowErrorToast(string.Format(
                                MediaGroupEditDialogViewModelRes.ToastSaveAddErrorCommunication,
                                Model.LocalizedName));

                            return Task.CompletedTask;
                        });
                }
                else
                {
                    await mediaGroupService.UpdateMediaGroupAsync(Model)
                        .HandleStatus(APIResultErrorType.NoError, async () =>
                        {
                            ShowSuccessToast(string.Format(MediaGroupEditDialogViewModelRes.ToastSaveEditSuccess,
                                Model.LocalizedName));

                            if (_oldModel is not null)
                            {
                                _oldModel.TakeOverValues(Model);

                                await eventAggregator.PublishAsync(new AggMediaGroupChanged() { MediaGroup = _oldModel });
                            }
                        })
                        .HandleStatus(APIResultErrorType.NotAuthorized, () =>
                        {
                            ShowErrorToast(string.Format(
                                MediaGroupEditDialogViewModelRes.ToastSaveEditErrorNotAuthorized,
                            Model.LocalizedName));

                            return Task.CompletedTask;
                        })
                        .HandleStatus(APIResultErrorType.NotFound, () =>
                        {
                            ShowErrorToast(string.Format(
                                MediaGroupEditDialogViewModelRes.ToastSaveEditErrorNotFound,
                            Model.LocalizedName));

                            return Task.CompletedTask;
                        })
                        .HandleStatus(APIResultErrorType.Conflict, () =>
                        {
                            ShowErrorToast(string.Format(
                                MediaGroupEditDialogViewModelRes.ToastSaveEditErrorConflict,
                            Model.LocalizedName));

                            return Task.CompletedTask;
                        })
                        .HandleStatus(APIResultErrorType.ServerError, () =>
                        {
                            ShowErrorToast(string.Format(
                                MediaGroupEditDialogViewModelRes.ToastSaveEditErrorServer,
                            Model.LocalizedName));

                            return Task.CompletedTask;
                        })
                        .HandleStatus(APIResultErrorType.CommunicationError, () =>
                        {
                            ShowErrorToast(string.Format(
                                MediaGroupEditDialogViewModelRes.ToastSaveEditErrorCommunication,
                                Model.LocalizedName));

                            return Task.CompletedTask;
                        });
                }

                if (stayInDialog)
                {
                    Model = new MediaGroupModel();
                }
                else
                {
                    MudDialog?.Close();
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
                ? MediaGroupEditDialogViewModelRes.AlertCancelAddTitle
                : MediaGroupEditDialogViewModelRes.AlertCancelEditTitle;
            string message = !IsInEditMode
                ? MediaGroupEditDialogViewModelRes.AlertCancelAddMessage
                : MediaGroupEditDialogViewModelRes.AlertCancelEditMessage;
            string buttonCancel = !IsInEditMode
                ? MediaGroupEditDialogViewModelRes.AlertCancelAddCancel
                : MediaGroupEditDialogViewModelRes.AlertCancelEditCancel;
            string buttonConfirm = !IsInEditMode
                ? MediaGroupEditDialogViewModelRes.AlertCancelAddConfirm
                : MediaGroupEditDialogViewModelRes.AlertCancelEditConfirm;

            var dialogResult =
                await QuestionConfirmRed(title, message, buttonConfirm, buttonCancel, Icons.Material.Filled.Cancel);

            if (dialogResult.HasValue && dialogResult.Value)
            {
                MudDialog?.Cancel();
            }
        }
        else
        {
            MudDialog?.Cancel();
        }
    }

    [RelayCommand]
    private async Task GoToToday()
    {
        if (PickerEventDate is not null)
        {
            await PickerEventDate.GoToDate(DateTime.Today);
        }
    }
    #endregion

    #region Abstract overrides
    public override void Dispose()
    {
    }
    #endregion
}
