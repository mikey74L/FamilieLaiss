using FamilieLaissFrontend.Client.ViewModels.Controls.VideoControl;
using FamilieLaissSharedUI.Components;
using Microsoft.AspNetCore.Components;

namespace FamilieLaissFrontend.Client.Controls.VideoControl;

public partial class VideoControlActions(VideoControlActionsViewModel viewModel, 
    NavigationManager navigationManager): FlComponentBase<VideoControlActionsViewModel>(viewModel, navigationManager)
{
    
}