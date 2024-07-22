using CommunityToolkit.Mvvm.Input;
using FamilieLaissInterfaces.Services;
using FamilieLaissSharedUI.Interfaces;
using FamilieLaissSharedUI.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FamilieLaissFrontend.Client.ViewModels.Controls.User;

public partial class LoginLogoutControlViewModel(
    ISnackbar snackbarService,
    IMessageBoxService messageBoxService,
    NavigationManager navManager,
    IMvvmNavigationManager navigationManager)
    : ViewModelBase(snackbarService, messageBoxService)
{
    #region Commands
    [RelayCommand]
    public void LogIn()
    {
        navManager.NavigateTo("Account/Login?redirectUri=/", true);
    }

    [RelayCommand]
    public void LogOut()
    {
        navManager.NavigateTo("Account/Logout", true);
    }

    [RelayCommand]
    public void UserSettings()
    {
        navigationManager.NavigateTo<UserSettingsControlViewModel>();
    }

    [RelayCommand]
    public void MyProfile()
    {

    }
    #endregion

    #region Abstract overrides
    public override void Dispose()
    {
    }
    #endregion
}
