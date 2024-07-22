using CommunityToolkit.Mvvm.Input;
using FamilieLaissInterfaces.DataServices;
using FamilieLaissInterfaces.Enums;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using FamilieLaissResources.Resources.ViewModels.Dialogs.Choose.Picture;
using FamilieLaissServices.Extensions;
using FamilieLaissSharedUI.Helper;
using FamilieLaissSharedUI.ViewModels;
using MudBlazor;

namespace FamilieLaissFrontend.Client.ViewModels.Dialogs.Choose.Picture;

public partial class ChoosePictureDialogViewModel(
    ISnackbar snackbarService,
    IMessageBoxService messageBoxService,
    IUploadPictureDataService uploadPictureDataService)
    : ViewModelBase(snackbarService, messageBoxService)
{
    #region Parameters
    public MudDialogInstance MudDialog { get; set; } = default!;
    #endregion

    #region Properties
    public ExtendedObservableCollection<IUploadPictureModel> UploadItems { get; } = [];
    #endregion

    #region Lifecycle Methods
    public override async Task OnInitializedAsync()
    {
        await LoadData();
    }
    #endregion

    #region Private Methods
    private async Task LoadData()
    {
        IsLoading = true;

        await uploadPictureDataService.GetUploadPicturesForChooseViewAsync()
            .HandleStatus(APIResultErrorType.NoError, (result) =>
            {
                UploadItems.Clear();
                UploadItems.AddRange(result);

                IsLoading = false;

                return Task.CompletedTask;
            })
            .HandleStatus(APIResultErrorType.NotAuthorized, (_) =>
            {
                ShowErrorToast(ChoosePictureDialogViewModelRes.ToastLoadingErrorNotAuthorized);

                HasError = true;

                IsLoading = false;

                return Task.CompletedTask;
            })
            .HandleStatus(APIResultErrorType.ServerError, (_) =>
            {
                ShowErrorToast(ChoosePictureDialogViewModelRes.ToastLoadingErrorServerError);

                HasError = true;

                IsLoading = false;

                return Task.CompletedTask;
            })
            .HandleStatus(APIResultErrorType.CommunicationError, (_) =>
            {
                ShowErrorToast(ChoosePictureDialogViewModelRes.ToastLoadingErrorCommunication);

                HasError = true;

                IsLoading = false;

                return Task.CompletedTask;
            });
    }
    #endregion

    #region Commands
    [RelayCommand]
    private void ChoosePicture(IUploadPictureModel uploadPictureModel)
    {
        MudDialog.Close(DialogResult.Ok(uploadPictureModel));
    }

    [RelayCommand]
    private void Cancel()
    {
        MudDialog.Cancel();
    }
    #endregion

    #region Abstract overrides
    public override void Dispose()
    {
    }
    #endregion 
}
