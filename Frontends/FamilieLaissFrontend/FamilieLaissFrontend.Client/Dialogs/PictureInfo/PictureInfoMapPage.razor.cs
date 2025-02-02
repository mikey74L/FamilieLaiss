using FamilieLaissFrontend.Client.ViewModels.Dialogs.PictureInfo;
using FamilieLaissSharedUI.Components;
using Microsoft.AspNetCore.Components;

namespace FamilieLaissFrontend.Client.Dialogs.PictureInfo;

public partial class PictureInfoMapPage(PictureInfoMapPageViewModel viewModel, 
    NavigationManager navigationManager): FlComponentBase<PictureInfoMapPageViewModel>(viewModel, navigationManager)
{
    
}