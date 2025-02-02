using FamilieLaissFrontend.Client.ViewModels.Dialogs.Choose.Picture;
using FamilieLaissSharedUI.Components;
using Microsoft.AspNetCore.Components;

namespace FamilieLaissFrontend.Client.Dialogs.Choose.Picture;

public partial class ChoosePictureDialog(ChoosePictureDialogViewModel viewModel, 
    NavigationManager navigationManager): FlComponentBase<ChoosePictureDialogViewModel>(viewModel, navigationManager)
{
    
}