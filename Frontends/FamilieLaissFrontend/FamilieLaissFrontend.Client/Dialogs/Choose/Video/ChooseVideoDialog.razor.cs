using FamilieLaissFrontend.Client.ViewModels.Dialogs.Choose.Video;
using FamilieLaissSharedUI.Components;
using Microsoft.AspNetCore.Components;

namespace FamilieLaissFrontend.Client.Dialogs.Choose.Video;

public partial class ChooseVideoDialog(ChooseVideoDialogViewModel viewModel, 
    NavigationManager navigationManager): FlComponentBase<ChooseVideoDialogViewModel>(viewModel, navigationManager)
{
    
}