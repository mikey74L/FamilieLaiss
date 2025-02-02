using FamilieLaissFrontend.Client.ViewModels.Dialogs.PictureInfo;
using FamilieLaissSharedUI.Components;
using Microsoft.AspNetCore.Components;

namespace FamilieLaissFrontend.Client.Dialogs.PictureInfo;

public partial class PictureInfoGeneralPart(PictureInfoGeneralPartViewModel viewModel, 
    NavigationManager navigationManager): FlComponentBase<PictureInfoGeneralPartViewModel>(viewModel, navigationManager)
{
    
}