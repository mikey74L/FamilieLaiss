using FamilieLaissFrontend.Client.ViewModels.Controls.PictureControl;
using FamilieLaissSharedUI.Components;
using Microsoft.AspNetCore.Components;

namespace FamilieLaissFrontend.Client.Controls.PictureControl;

public partial class PictureControlActions(PictureControlActionsViewModel viewModel, 
    NavigationManager navigationManager): FlComponentBase<PictureControlActionsViewModel>(viewModel, navigationManager)
{
    
}