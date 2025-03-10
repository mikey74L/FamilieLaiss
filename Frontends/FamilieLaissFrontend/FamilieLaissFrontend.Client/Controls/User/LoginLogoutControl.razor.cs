using FamilieLaissFrontend.Client.ViewModels.Controls.User;
using FamilieLaissSharedUI.Components;
using Microsoft.AspNetCore.Components;

namespace FamilieLaissFrontend.Client.Controls.User;

public partial class LoginLogoutControl(LoginLogoutControlViewModel viewModel, 
    NavigationManager navigationManager): FlComponentBase<LoginLogoutControlViewModel>(viewModel, navigationManager)
{
    
}