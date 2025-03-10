using FamilieLaissFrontend.Client.ViewModels.Controls.User;
using FamilieLaissSharedUI.Components;
using Microsoft.AspNetCore.Components;

namespace FamilieLaissFrontend.Client.Controls.User;

public partial class UserSettingsControl(UserSettingsControlViewModel viewModel, 
    NavigationManager navigationManager): FlComponentBase<UserSettingsControlViewModel>(viewModel, navigationManager)
{
    
}