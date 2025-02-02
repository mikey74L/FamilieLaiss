using FamilieLaissFrontend.Client.ViewModels.Dialogs.BaseData.MediaGroup;
using FamilieLaissSharedUI.Components;
using Microsoft.AspNetCore.Components;

namespace FamilieLaissFrontend.Client.Dialogs.BaseData.MediaGroup;

public partial class MediaGroupEditDialog(MediaGroupEditDialogViewModel viewModel, 
    NavigationManager navigationManager) : FlComponentBase<MediaGroupEditDialogViewModel>(viewModel, navigationManager)
{
    
}