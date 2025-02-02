using FamilieLaissFrontend.Client.ViewModels.Controls.PictureControl;
using FamilieLaissSharedUI.Components;
using Microsoft.AspNetCore.Components;

namespace FamilieLaissFrontend.Client.Controls.PictureControl;

public partial class PictureControl(PictureControlViewModel viewModel, 
    NavigationManager navigationManager): FlComponentBase<PictureControlViewModel>(viewModel, navigationManager)
{
    
}