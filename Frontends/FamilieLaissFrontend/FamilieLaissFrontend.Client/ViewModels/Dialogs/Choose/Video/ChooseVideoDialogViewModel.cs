using CommunityToolkit.Mvvm.Input;
using FamilieLaissInterfaces.DataServices;
using FamilieLaissInterfaces.Enums;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using FamilieLaissResources.Resources.ViewModels.Dialogs.Choose.Video;
using FamilieLaissServices.Extensions;
using FamilieLaissSharedUI.Helper;
using FamilieLaissSharedUI.ViewModels;
using MudBlazor;

namespace FamilieLaissFrontend.Client.ViewModels.Dialogs.Choose.Video;

public partial class ChooseVideoDialogViewModel(
    ISnackbar snackbarService,
    IMessageBoxService messageBoxService,
    IUploadVideoDataService uploadVideoDataService)
    : ViewModelBase(snackbarService, messageBoxService)
{
    #region Parameters

    public MudDialogInstance MudDialog { get; set; } = default!;

    #endregion

    #region Properties

    public ExtendedObservableCollection<IUploadVideoModel> UploadItems { get; } = [];

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

        await uploadVideoDataService.GetUploadVideosForChooseViewAsync()
            .HandleStatus(APIResultErrorType.NoError, (result) =>
            {
                UploadItems.Clear();
                UploadItems.AddRange(result);

                IsLoading = false;

                return Task.CompletedTask;
            })
            .HandleStatus(APIResultErrorType.NotAuthorized, (_) =>
            {
                ShowErrorToast(ChooseVideoDialogViewModelRes.ToastLoadingErrorNotAuthorized);

                HasError = true;

                IsLoading = false;

                return Task.CompletedTask;
            })
            .HandleStatus(APIResultErrorType.ServerError, (_) =>
            {
                ShowErrorToast(ChooseVideoDialogViewModelRes.ToastLoadingErrorServerError);

                HasError = true;

                IsLoading = false;

                return Task.CompletedTask;
            })
            .HandleStatus(APIResultErrorType.CommunicationError, (_) =>
            {
                ShowErrorToast(ChooseVideoDialogViewModelRes.ToastLoadingErrorCommunication);

                HasError = true;

                IsLoading = false;

                return Task.CompletedTask;
            });
    }

    #endregion

    #region Commands

    [RelayCommand]
    private void ChooseVideo(IUploadVideoModel uploadVideoModel)
    {
        MudDialog.Close(DialogResult.Ok(uploadVideoModel));
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