using FamilieLaissFrontend.Client.ViewModels.Dialogs.PictureInfo;
using FamilieLaissSharedUI.Components;
using Microsoft.AspNetCore.Components;

namespace FamilieLaissFrontend.Client.Dialogs.PictureInfo;

public partial class PictureInfoDialog(PictureInfoDialogViewModel viewModel, 
    NavigationManager navigationManager): FlComponentBase<PictureInfoDialogViewModel>(viewModel, navigationManager)
{
    
}