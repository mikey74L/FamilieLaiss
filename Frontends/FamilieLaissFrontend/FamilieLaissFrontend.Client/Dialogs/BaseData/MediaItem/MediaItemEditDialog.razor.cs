using FamilieLaissFrontend.Client.ViewModels.Dialogs.BaseData.MediaItem;
using FamilieLaissSharedUI.Components;
using Microsoft.AspNetCore.Components;

namespace FamilieLaissFrontend.Client.Dialogs.BaseData.MediaItem;

public partial class MediaItemEditDialog(MediaItemEditDialogViewModel viewModel, 
    NavigationManager navigationManager) : FlComponentBase<MediaItemEditDialogViewModel>(viewModel, navigationManager)
{
    
}