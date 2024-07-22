using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using FamilieLaissResources.Resources.ViewModels.Controls.User;
using FamilieLaissServices.Extensions;
using FamilieLaissSharedUI.ViewModels;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using MudBlazor;

namespace FamilieLaissFrontend.Client.ViewModels.Controls.User;

public partial class UserSettingsControlViewModel(
    ISnackbar snackbarService,
    IMessageBoxService messageBoxService,
    IUserSettingsService userSettingsService)
    : ViewModelBase(snackbarService, messageBoxService)
{
    #region Properties
    public Task<AuthenticationState>? AuthenticationState { get; set; }
    #endregion

    #region Public Properties
    [ObservableProperty]
    private IUserSettingsModel? _model;

    public MudForm? Form { get; set; }
    #endregion

    #region Lifecycle
    public override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        Model = await userSettingsService.GetCurrentUserSettings(AuthenticationState);
    }
    #endregion

    #region Commands
    [RelayCommand]
    private async Task BeforeInternalNavigation(LocationChangingContext context)
    {
        if (Form is not null && Form.IsTouched)
        {
            var result = await QuestionConfirmWithCancel(UserSettingsControlViewModelRes.QuestionTitleLeave,
                UserSettingsControlViewModelRes.QuestionMessageLeave,
                UserSettingsControlViewModelRes.QuestionButtonSave,
                UserSettingsControlViewModelRes.QuestionButtonNotSave,
                UserSettingsControlViewModelRes.QuestionButtonCancel,
                false, false, false);

            if (result is null)
            {
                context.PreventNavigation();
            }
            else
            {
                if (result.Value)
                {
                    await userSettingsService.SaveUserSettings()
                        .HandleSuccess(() =>
                        {
                            ShowSuccessToast(UserSettingsControlViewModelRes.ToastSuccess);

                            return Task.CompletedTask;
                        })
                        .HandleErrors(() =>
                        {
                            ShowErrorToast(UserSettingsControlViewModelRes.ToastError);

                            return Task.CompletedTask;
                        });
                }
                else
                {
                    userSettingsService.ResetUserSettings();
                }
            }
        }
    }
    #endregion

    #region Abstract overrides
    public override void Dispose()
    {
    }
    #endregion
}
