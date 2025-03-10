using FamilieLaissFrontend.Client.ViewModels.Controls.UploadControl;
using FamilieLaissSharedUI.Components;
using Microsoft.AspNetCore.Components;

namespace FamilieLaissFrontend.Client.Controls.UploadControl;

public partial class UploadControl(UploadControlViewModel viewModel, 
    NavigationManager navigationManager): FlComponentBase<UploadControlViewModel>(viewModel, navigationManager)
{
    
}