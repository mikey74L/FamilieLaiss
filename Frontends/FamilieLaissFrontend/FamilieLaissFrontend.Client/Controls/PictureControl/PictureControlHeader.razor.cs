using FamilieLaissFrontend.Client.ViewModels.Controls.PictureControl;
using FamilieLaissSharedUI.Components;
using Microsoft.AspNetCore.Components;

namespace FamilieLaissFrontend.Client.Controls.PictureControl;

public partial class PictureControlHeader(PictureControlHeaderViewModel viewModel, 
    NavigationManager navigationManager): FlComponentBase<PictureControlHeaderViewModel>(viewModel, navigationManager)
{
    
}